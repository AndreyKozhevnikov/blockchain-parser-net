using System.IO.MemoryMappedFiles;
namespace BlockParser;

public class BlockChainParser {

    private static DateTime _epochBaseDate = new DateTime(1970, 1, 1);
    public List<TBlock> Parse(string filePath) {

        var memFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, Path.GetFileName(filePath), 0, MemoryMappedFileAccess.Read);
        var viewStream = memFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
        var reader = new BinaryReader(viewStream);
        var lst = ParseCore(reader);
        return lst;
    }

    public List<TBlock> ParseCore(BinaryReader reader) {
        //using (FileStream fileStream = new FileStream("oneBlockData.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
        //    fileStream.Write(testArr, 0, testArr.Length);
        //}

        ScriptParser scriptParser = new ScriptParser();

        var blocks = new List<TBlock>();
        while (ReadMagic(reader)) {
            var block = new TBlock();
            List<byte> blockHex = new List<byte>();
            var stream = reader.BaseStream;
            var r = new BinaryReader(stream);

            var sizeBT = r.ReadBytes(4);
            block.Size = BitConverter.ToInt32(sizeBT);

            var versionNumberBT = r.ReadBytes(4);
            blockHex.AddRange(versionNumberBT);
            versionNumberBT = ReverseBytes(versionNumberBT);
            block.VersionNumber = "0x" + GetStringFromBytes(versionNumberBT);

            var _previousBlockHashBT = r.ReadBytes(32);
            blockHex.AddRange(_previousBlockHashBT);
            _previousBlockHashBT = ReverseBytes(_previousBlockHashBT);
            block.PrevBlockHash = GetStringFromBytes(_previousBlockHashBT);

            var _merkleRootBT = r.ReadBytes(32);
            blockHex.AddRange(_merkleRootBT);
            _merkleRootBT = ReverseBytes(_merkleRootBT);
            block.MerkleRoot = GetStringFromBytes(_merkleRootBT);

            List<byte> tmpByte = new List<byte>(r.ReadBytes(4));
            var _timeStampTx = BitConverter.ToUInt32(tmpByte.ToArray());
            block.TimeStamp = _epochBaseDate.AddSeconds(_timeStampTx);
            blockHex.AddRange(tmpByte);
            tmpByte.Clear();

            var _bitsBT = r.ReadBytes(4);
            blockHex.AddRange(_bitsBT);
            _bitsBT = ReverseBytes(_bitsBT);
            block.Bits = "0x" + GetStringFromBytes(_bitsBT);

            tmpByte = new List<byte>(r.ReadBytes(4));
            var _nonceBT = BitConverter.ToUInt32(tmpByte.ToArray());
            block.Nonce = _nonceBT;
            blockHex.AddRange(tmpByte);
            tmpByte.Clear();

            var _transactionCount = r.ReadVarInt();
            block.TransactionCount = _transactionCount;
            block.Transactions = new List<TTransaction>();

            var txHashConverter = new TxByteToHashConverter();
            block.Hash = txHashConverter.Convert(blockHex);

            for (int i = 0; i < _transactionCount; i++) {

                List<byte> transactionHEX = new List<byte>();
                var transaction = new TTransaction();

                tmpByte = new List<byte>(r.ReadBytes(4));
                transactionHEX.AddRange(tmpByte);
                transaction.Version = BitConverter.ToUInt32(tmpByte.ToArray());

                transaction.InputCount = r.ReadVarIntOut(tmpByte);

                if (transaction.InputCount == 0) {
                    r.Read();
                    transaction.InputCount = r.ReadVarIntOut(tmpByte);
                    transactionHEX.AddRange(tmpByte);
                    tmpByte.Clear();
                    transaction.HasWitness = true;
                } else {
                    transactionHEX.AddRange(tmpByte);
                    tmpByte.Clear();
                }

                transaction.Inputs = new List<Input>();
                for (int j = 0; j < transaction.InputCount; j++) {
                    var input = new Input();
                    var _txIDBT = r.ReadBytes(32);
                    transactionHEX.AddRange(_txIDBT);
                    _txIDBT = ReverseBytes(_txIDBT);
                    input.TxId = GetStringFromBytes(_txIDBT);

                    var _outputNumberBT = r.ReadBytes(4);
                    transactionHEX.AddRange(_outputNumberBT);
                    input.OutputNumber = GetStringFromBytes(_outputNumberBT); ;

                    input.ScriptLength = r.ReadVarIntOut(tmpByte);
                    transactionHEX.AddRange(tmpByte);

                    var _scriptBT = r.ReadBytes((int)input.ScriptLength);
                    transactionHEX.AddRange(_scriptBT);
                    input.Script = GetStringFromBytes(_scriptBT);

                    var _seqNumberBT = r.ReadBytes(4);
                    transactionHEX.AddRange(_seqNumberBT);
                    input.Sequence = GetStringFromBytes(_seqNumberBT);
                    transaction.Inputs.Add(input);
                }

                transaction.OutputCount = r.ReadVarIntOut(tmpByte);
                transactionHEX.AddRange(tmpByte);

                transaction.Outputs = new List<Output>();
                for (int j = 0; j < transaction.OutputCount; j++) {
                    var output = new Output();
                    var _outValueBT = r.ReadBytes(8);
                    transactionHEX.AddRange(_outValueBT);
                    output.Value = BitConverter.ToInt32(_outValueBT);

                    output.ScriptSize = r.ReadVarIntOut(tmpByte);
                    transactionHEX.AddRange(tmpByte);

                    var _scriptBT = r.ReadBytes((int)output.ScriptSize);
                    transactionHEX.AddRange(_scriptBT);
                    output.Script = GetStringFromBytes(_scriptBT);

                    var address = scriptParser.GetAddressFromScript(_scriptBT);
                    output.Address = address;

                    transaction.Outputs.Add(output);
                }
                transaction.Witnesses = new List<Witness>();
                if (transaction.HasWitness) {
                    for (int j = 0; j < transaction.InputCount; j++) {
                        var witnessCnt = r.ReadVarInt();
                        for (int k = 0; k < witnessCnt; k++) {
                            var witness = new Witness();
                            witness.Size = r.ReadVarInt();
                            var witnessVl = r.ReadBytes((int)witness.Size);
                            witness.WitnessValue = GetStringFromBytes(witnessVl);
                            transaction.Witnesses.Add(witness);
                        }
                    }
                }
                var _lockTimeBT = r.ReadBytes(4);
                transactionHEX.AddRange(_lockTimeBT);
                transaction.LockTime = GetStringFromBytes(_lockTimeBT);

                transaction.Hash = txHashConverter.Convert(transactionHEX);

                block.Transactions.Add(transaction);
                //using(FileStream fileStream = new FileStream("txHashBytes.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
                //    fileStream.Write(transactionHEX.ToArray(), 0, transactionHEX.Count);
                //}
            }
            blocks.Add(block);
        }
        return blocks;
    }

    private bool ReadMagic(BinaryReader reader) {
        try {
        ini:
            byte b0 = reader.ReadByte();
            if (b0 != 0xF9) goto ini;
            b0 = reader.ReadByte();
            if (b0 != 0xbe) goto ini;
            b0 = reader.ReadByte();
            if (b0 != 0xb4) goto ini;
            b0 = reader.ReadByte();
            if (b0 != 0xd9) goto ini;
            return true;
        }
        catch (EndOfStreamException) {
            return false;
        }
    }

    public string GetStringFromBytes(byte[] data) {
        return BitConverter.ToString(data).Replace("-", null).ToLower();
    }


    public static byte[] ReverseBytes(byte[] arr) {
        var res = arr.Reverse().ToArray();
        return res;
    }
    public static byte[] StringToByteArray(String hex) {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

}

