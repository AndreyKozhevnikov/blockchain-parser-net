using NBitcoin;
using NBitcoin.Crypto;
using System.Reflection.Emit;
using BlockParser.Classes;
namespace BlockParser.Classes {

    public class CPubKey {

        /**
         * secp256k1:
         */
        public static int SIZE = 65;
        public static int COMPRESSED_SIZE = 33;
        public static int SIGNATURE_SIZE = 72;
        public static int COMPACT_SIGNATURE_SIZE = 65;
        public static bool ValidSize(byte[] data) {
            return data.Length > 0 && GetLen(data[0]) == data.Length;

        }
        static int GetLen(byte chHeader) {
            if (chHeader == 2 || chHeader == 3)
                return COMPRESSED_SIZE;
            if (chHeader == 4 || chHeader == 6 || chHeader == 7)
                return SIZE;
            return 0;
        }
    }

    //https://github.com/bitcoin/bitcoin/blob/0cda5573405d75d695aba417e8f22f1301ded001/src/script/standard.cpp#L91
    //https://github.com/bitcoin/bitcoin/blob/master/src/addresstype.cpp
    //https://gist.github.com/t4sk/e251e6f298b533039f0f276cf6f5fb28
    public class ScriptParser {
        public const byte OP_HASH160 = 0xa9;

        public const byte OP_EQUAL = 0x87;

        public const byte OP_0 = 0x00;
        public const byte OP_1 = 0x51;
        public const byte OP_TRUE = OP_1;
        public const byte OP_2 = 0x52;
        public const byte OP_3 = 0x53;
        public const byte OP_4 = 0x54;
        public const byte OP_5 = 0x55;
        public const byte OP_6 = 0x56;
        public const byte OP_7 = 0x57;
        public const byte OP_8 = 0x58;
        public const byte OP_9 = 0x59;
        public const byte OP_10 = 0x5a;
        public const byte OP_11 = 0x5b;
        public const byte OP_12 = 0x5c;
        public const byte OP_13 = 0x5d;
        public const byte OP_14 = 0x5e;
        public const byte OP_15 = 0x5f;
        public const byte OP_16 = 0x60;
        public const byte OP_EQUALVERIFY = 0x88;
        public const byte OP_DUP = 0x76;
        public const byte OP_RETURN = 0x6a;
        public const byte OP_CHECKSIG = 0xac;
        static int WITNESS_V0_SCRIPTHASH_SIZE = 32;
        static int WITNESS_V0_KEYHASH_SIZE = 20;
        static int WITNESS_V1_TAPROOT_SIZE = 32;


        public string GetAddressFromScript(byte[] data) {
            var res = ExtractDestination(data);
            return res;
        }

        string ExtractDestination(byte[] data) {
            byte[] solutions;
            var whichType = Solver(data,out solutions);
            string address=null;
            switch(whichType) {
                case "TxoutType::PUBKEYHASH":
                    var publicKeyHash = new KeyId(solutions);
                    address = publicKeyHash.GetAddress(Network.Main).ToString() ;
                    break;
            }
            return address;
        }

        string Solver(byte[] data,out byte[] solutions) {
            var isPayTOHash = IsPayToScriptHash(data);
            if (isPayTOHash) {
                solutions = data.SubArray(2, data.Length - 2);
                return "SCRIPTHASH";
            }


            int witnessversion;
            byte[] witnessprogram;
            var isWitness = IsWitness(data, out witnessversion, out witnessprogram);
            if (isWitness) {
                solutions = witnessprogram;
                if (witnessversion == 0 && witnessprogram.Length == WITNESS_V0_KEYHASH_SIZE) {
                    return "TxoutType::WITNESS_V0_KEYHASH";
                }
                if (witnessversion == 0 && witnessprogram.Length == WITNESS_V0_SCRIPTHASH_SIZE) {
                    return "TxoutType::WITNESS_V0_SCRIPTHASH";
                }
                if (witnessversion == 1 && witnessprogram.Length == WITNESS_V1_TAPROOT_SIZE) {
                    return "TxoutType::WITNESS_V1_TAPROOT";
                }
                if (witnessversion != 0) {
                    return "TxoutType::WITNESS_UNKNOWN";
                }
                return "TxoutType::NONSTANDARD";
            }
            //todo?
            //if (data.Length >= 1 && data[0] == OP_RETURN && IsPushOnly(data.SubArray(1,data.Length-1))) {
            //    return "TxoutType::NULL_DATA";
            //}

            //todo!
            //if (MatchPayToPubkey(scriptPubKey, data)) {
            //    vSolutionsRet.push_back(std::move(data));
            //    return TxoutType::PUBKEY;
            //}
            byte[] adrData;
            if (MatchPayToPubkeyHash(data,out adrData)) {
                //  vSolutionsRet.push_back(std::move(data));
                //   var publHash = data.SubArray(3, 20);
                solutions = adrData;
            
                return "TxoutType::PUBKEYHASH";
            }
            solutions = null;
            return null;
        }

        bool MatchPayToPubkey(byte[] data) {
            if (data.Length == CPubKey.SIZE + 2 && data[0] == CPubKey.SIZE && data.Last() == OP_CHECKSIG) {
                var pubkey = data.SubArray(1, CPubKey.SIZE + 1);
                return CPubKey.ValidSize(pubkey);
            }
            //todo
            //if (script.size() == CPubKey::COMPRESSED_SIZE + 2 && script[0] == CPubKey::COMPRESSED_SIZE && script.back() == OP_CHECKSIG) {
            //    pubkey = valtype(script.begin() + 1, script.begin() + CPubKey::COMPRESSED_SIZE + 1);
            //    return CPubKey::ValidSize(pubkey);
            //}
            return false;
        }
        bool MatchPayToPubkeyHash(byte[] data,out byte[] adrData) {
            if (data.Length == 25 && data[0] == OP_DUP && data[1] == OP_HASH160 && data[2] == 20 && data[23] == OP_EQUALVERIFY && data[24] == OP_CHECKSIG) {
                adrData = data.SubArray(3, 20);
                return true;
            }
            adrData = null;
            return false;
        }

        bool IsPayToScriptHash(byte[] data) {
            return data.Length == 23 &&
                data[0] == OP_HASH160 &&
                data[1] == 0x14 &&
                data[22] == OP_EQUAL;
        }

        //public static void ParseScript(byte[] data) {
        //    var b = IsPayToScriptHash(data);
        //    int witnessversion = -1;
        //    byte[] witnessprogram = null;
        //    var b1 = IsWitness(data, witnessversion, witnessprogram);
        //    if (b1) {
        //        if (witnessversion == 0 && witnessprogram.Length == WITNESS_V0_KEYHASH_SIZE) {
        //            vSolutionsRet.push_back(witnessprogram);
        //            return TX_WITNESS_V0_KEYHASH;
        //        }
        //        if (witnessversion == 0 && witnessprogram.size() == WITNESS_V0_SCRIPTHASH_SIZE) {
        //            vSolutionsRet.push_back(witnessprogram);
        //            return TX_WITNESS_V0_SCRIPTHASH;
        //        }
        //        if (witnessversion != 0) {
        //            vSolutionsRet.push_back(std::vector < unsigned char >{ (unsigned char)witnessversion});
        //            vSolutionsRet.push_back(std::move(witnessprogram));
        //            return TX_WITNESS_UNKNOWN;
        //        }
        //    }
        //    var publHash = data.SubArray(3, 20);
        //    var publicKeyHash = new KeyId(publHash);
        //    var mainNetAddress = publicKeyHash.GetAddress(Network.Main); //https://github.com/bitcoin/bitcoin/blob/0cda5573405d75d695aba417e8f22f1301ded001/src/script/standard.cpp#L156
        //}

        static bool IsWitness(byte[] data, out int version, out byte[] program) {
            version = -1;
            program = null;
            if (data.Length < 4 || data.Length > 42) {

                return false;
            }
            if (data[0] != OP_0 && (data[0] < OP_1 || data[0] > OP_16)) {
                return false;
            }

            var b = data[1];
            var l = data.Length;
            if (data[1] + 2 == data.Length) {
                version = DecodeOP_N(data[0]);
                program = data.SubArray(2, data.Length - 2);
                //    program = std::vector < unsigned char> (this->begin() + 2, this->end());
                return true;
            }
            return false;
        }

        static int DecodeOP_N(byte opcode) {
            if (opcode == OP_0)
                return 0;
            // assert(opcode >= OP_1 && opcode <= OP_16);
            return (int)opcode - (int)(OP_1 - 1);
        }


    }
}
