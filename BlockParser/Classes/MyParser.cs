﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockParser.Entities;
using HashConverterNS;
using Microsoft.VisualBasic;
using NBitcoin.Crypto;
using static NBitcoin.RPC.SignRawTransactionRequest;
namespace BlockParser.Classes {

    public class Parser {

        private static DateTime _epochBaseDate = new DateTime(1970, 1, 1);
        public void Parse2() {
            //530849
            //000000000000000000280e2daa0be4346cf4d52d3f8ee7aaf4839432a00d1a1a


            //prev 0000000000000000002b81a1bd099b0f3fb93ed811f1c95dddf118dce63bf511
            var fl = "blk01307.dat";
            byte[] flDdata = File.ReadAllBytes(fl);
            var firsArr = flDdata.SubArray(0, 4);
            var sizeArr = flDdata.SubArray(4, 4);
            var size = BitConverter.ToInt16(sizeArr);
            var headerArr = flDdata.SubArray(8, 80);

            var versA = headerArr.SubArray(0, 4);
            var prevHA = headerArr.SubArray(4, 32);
            var st0 = BitConverter.ToString(prevHA).Replace("-", null);
            var prevH = HashConverter.Convert(st0); //!!

            var timeA = headerArr.SubArray(68, 4);

            var timeSec = BitConverter.ToInt32(timeA);
            var time = _epochBaseDate.AddSeconds(timeSec); //!!
            var header = BitConverter.ToString(headerArr);

            var txCountArr = flDdata.SubArray(88, 16);
            var txCountArr2 = flDdata.SubArray(88, 2);

            var txCntFirst = flDdata.SubArray(88, 1)[0];
            var txCntSec = flDdata.SubArray(89, 2);
            var txCount = BitConverter.ToInt16(txCntSec); //!!

            long txTemp = -1;

            if(txCntFirst < 0xfd) txTemp = 0;
            if(txCntFirst == 0xfd) txTemp = 1;
            if(txCntFirst == 0xfe) txTemp = 2;
            if(txCntFirst == 0xff) txTemp = 3;
            // var txCount = BitConverter.ToInt32(txCountArr2);
        }





        public void Parse() {
            var fl = "blk01307.dat";
            var memFile = MemoryMappedFile.CreateFromFile(fl, FileMode.Open, Path.GetFileName(fl), 0, MemoryMappedFileAccess.Read);
            var viewStream = memFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            var reader = new BinaryReader(viewStream);
            var lst = ParseCore(reader);
        }

        public List<TBlock> ParseCore(BinaryReader reader) {


            //test
            //var testArr = reader.ReadBytes(591562);
            //var testArrSt = BitConverter.ToString(testArr).Replace("-", null);
            //using (FileStream fileStream = new FileStream("oneBlockData.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
            //    fileStream.Write(testArr, 0, testArr.Length);
            //}
            //
            ScriptParser scriptParser = new ScriptParser();

            var blocks = new List<TBlock>();
            while(ReadMagic(reader)) {
                //while (true) {
                var block = new TBlock();
                var stream = reader.BaseStream;
                var _position = stream.Position;


                var r = new BinaryReader(stream);
                //  var magic = r.ReadBytes(4); 
                //  var magicST = BitConverter.ToString(magic); F9BEB4D9

                var sizeA = r.ReadBytes(4);

                var sizeArev = ReverseBytes(sizeA);


                block.Size = BitConverter.ToInt32(sizeA);
                var versionNumberVal = r.ReadBytes(4);
                var versionNumberValST = BitConverter.ToString(versionNumberVal).Replace("-", null);
                block.VersionNumber = "0x" + HashConverter.Convert(versionNumberValST);
                var _previousBlockHash = r.ReadBytes(32);
                var st0 = BitConverter.ToString(_previousBlockHash).Replace("-", null);
                byte[] bytes = Encoding.Unicode.GetBytes(st0);
                byte[] bytes2 = HashConverter.StringToByteArray(st0);

                var prevH = HashConverter.Convert(st0); //!!
                block.PrevBlockHash = prevH;
                var _merkleRoot = r.ReadBytes(32);
                var tmpMerkle = BitConverter.ToString(_merkleRoot).Replace("-", null);
                block.MerkleRoot = HashConverter.Convert(tmpMerkle);
                block.TimeStamp = _epochBaseDate.AddSeconds(r.ReadUInt32());
                var bitsBytes = r.ReadBytes(4);

                var bitST = BitConverter.ToString(bitsBytes).Replace("-", null);  //https://learnmeabitcoin.com/technical/bits
                block.Bits = "0x" + HashConverter.Convert(bitST);
                // block.Bits = r.re();
                block.Nonce = r.ReadUInt32();
                var _transactionCount = r.ReadVarInt();
                block.TransactionCount = _transactionCount;
                block.Transactions = new List<TTransaction>();
                for(int i = 0; i < _transactionCount; i++) {
                    var transaction = new TTransaction();
                    transaction.Version = r.ReadUInt32();

                    //var testTx = r.ReadBytes(50000);
                    //var testST = BitConverter.ToString(testTx).Replace("-", null);

                    transaction.InputCount = r.ReadVarInt();

                    // bool IsWitness = false;
                    if(transaction.InputCount == 0) {
                        r.Read();
                        transaction.InputCount = r.ReadVarInt();
                        transaction.HasWitness = true;
                    }
                    transaction.Inputs = new List<Input>();
                    for(int j = 0; j < transaction.InputCount; j++) {
                        var input = new Input();
                        var txfromhash = r.ReadBytes(32);
                        input.TxId = BitConverter.ToString(txfromhash).Replace("-", null);

                        var nOutput = r.ReadBytes(4);
                        var nOutputTx = BitConverter.ToString(nOutput).Replace("-", null);
                        input.OutputNumber = nOutputTx;
                        input.ScriptLength = r.ReadVarInt();
                        var script = r.ReadBytes((int)input.ScriptLength);
                        input.Script = BitConverter.ToString(script).Replace("-", null);
                        var seqNumber = r.ReadBytes(4);
                        input.Sequence = BitConverter.ToString(seqNumber).Replace("-", null);
                        transaction.Inputs.Add(input);
                    }
                    transaction.OutputCount = r.ReadVarInt();
                    transaction.Outputs = new List<Output>();
                    for(int j = 0; j < transaction.OutputCount; j++) {
                        var output = new Output();
                        //var testTx = r.ReadBytes(50000);
                        //var testST = BitConverter.ToString(testTx).Replace("-", null);

                        var outVala = r.ReadBytes(8);
                        var outValaa = ReverseBytes(outVala);
                        output.Value = BitConverter.ToInt32(outVala);


                        output.ScriptSize = r.ReadVarInt();
                        var script = r.ReadBytes((int)output.ScriptSize);
                        output.Script = BitConverter.ToString(script).Replace("-", null);

                        var adr = scriptParser.GetAddressFromScript(script);
                        output.Address = adr;


                        //get key - to do
                        //var publHash = script.SubArray(3, 20);
                        //var publicKeyHash = new KeyId(publHash);
                        //var mainNetAddress = publicKeyHash.GetAddress(Network.Main); https://github.com/bitcoin/bitcoin/blob/0cda5573405d75d695aba417e8f22f1301ded001/src/script/standard.cpp#L156
                        //
                        transaction.Outputs.Add(output);

                    }
                    transaction.Witnesses = new List<Witness>();
                    if(transaction.HasWitness) {
                        for(int j = 0; j < transaction.InputCount; j++) {
                            var witnessCnt = r.ReadVarInt();
                            for(int k = 0; k < witnessCnt; k++) {
                                var witness = new Witness();
                                // witness.Size
                                witness.Size = r.ReadVarInt();
                                var witnessVl = r.ReadBytes((int)witness.Size);
                                witness.WitnessValue = BitConverter.ToString(witnessVl).Replace("-", null);
                                transaction.Witnesses.Add(witness);
                            }
                        }

                    }
                    var lockTime = r.ReadBytes(4);
                    transaction.LockTime = BitConverter.ToString(lockTime).Replace("-", null);
                    block.Transactions.Add(transaction);
                }
                blocks.Add(block);
            }
            return blocks;
        }

        private bool ReadMagic(BinaryReader reader) {
            try {
            ini:
                byte b0 = reader.ReadByte();
                if(b0 != 0xF9) goto ini;
                b0 = reader.ReadByte();
                if(b0 != 0xbe) goto ini;
                b0 = reader.ReadByte();
                if(b0 != 0xb4) goto ini;
                b0 = reader.ReadByte();
                if(b0 != 0xd9) goto ini;
                return true;
            }
            catch(EndOfStreamException) {
                return false;
            }
        }


        public static byte[] ReverseBytes(byte[] arr) {
            var res = arr.Reverse().ToArray();
            return res;
        }

        public static string Reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        static void Swap(ref byte a, ref byte b) {
            var temp = a;
            a = b;
            b = temp;
        }

        static byte[] FlipInt16(byte[] rawData) {
            for(var i = 0; i < rawData.Length; i += 2) // Step two for 2x8 bits=16
                Swap(ref rawData[i], ref rawData[i + 1]);
            return rawData;
        }

        static byte[] FlipInt32(byte[] rawData) {
            for(var i = 0; i < rawData.Length; i += 4) {// Step four for 4x8 bits=32
                Swap(ref rawData[i + 0], ref rawData[i + 2]);
                Swap(ref rawData[i + 1], ref rawData[i + 3]);
            }
            return rawData;
        }

    }
    public static class Extensions {
        public static T[] SubArray<T>(this T[] array, int offset, int length) {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
        public static long ReadVarInt(this BinaryReader reader) {
            var t = reader.ReadByte();
            if(t < 0xfd) return t;
            if(t == 0xfd) return reader.ReadInt16();
            if(t == 0xfe) return reader.ReadInt32();
            if(t == 0xff) return reader.ReadInt64();

            throw new InvalidDataException("Reading Var Int");
        }
    }
}
