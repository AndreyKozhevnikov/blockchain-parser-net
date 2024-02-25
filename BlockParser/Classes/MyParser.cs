using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashConverterNS;
namespace BlockParser.Classes {
    public static class MyExtension {
       
    }

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

            if (txCntFirst < 0xfd) txTemp = 0;
            if (txCntFirst == 0xfd) txTemp = 1;
            if (txCntFirst == 0xfe) txTemp = 2;
            if (txCntFirst == 0xff) txTemp = 3;
            // var txCount = BitConverter.ToInt32(txCountArr2);
        }

        public void Parse() {
            var fl = "blk01307.dat";
            var memFile = MemoryMappedFile.CreateFromFile(fl, FileMode.Open, Path.GetFileName(fl), 0, MemoryMappedFileAccess.Read);
            var viewStream = memFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            var reader = new BinaryReader(viewStream);
         //   while (ReadMagic(reader)) {
            while (true) {
                var stream = reader.BaseStream;
                var _position = stream.Position;
              //  stream.Position = _position + 4;
              
                var r = new BinaryReader(stream);
                var magic = r.ReadBytes(4);
                var magicST = BitConverter.ToString(magic);
                // var sz = r.ReadVarInt();


                // var test = r.ReadBytes(1000000);
                // var testSt=BitConverter.ToString(test).Replace("-",null);

                // var newMagicC = testSt.IndexOf("F9BEB4D9");
              //  var sizeA2 = r.ReadVarInt();
                var sizeA = r.ReadBytes(4);

                var sizeArev=ReverseBytes(sizeA);


                var size=BitConverter.ToInt32(sizeA); //!!
                //var sizeAA = ReverseBytes(sizeA);
                //var size=BitConverter.ToInt32(sizeAA);


                //test11

             //   var allBlock = r.ReadBytes((int)sz);

                //var newBlockMagic = r.ReadBytes(40);



                //test
                var _versionNumber = r.ReadUInt32();
                var _previousBlockHash = r.ReadBytes(32);
                var st0 = BitConverter.ToString(_previousBlockHash).Replace("-", null);
                var prevH = HashConverter.Convert(st0); //!!
                var _merkleRoot = r.ReadBytes(32);
                var merkleString = BitConverter.ToString(_merkleRoot);
                var _timeStamp = _epochBaseDate.AddSeconds(r.ReadUInt32());
                var _bits = r.ReadUInt32();
                var _nonce = r.ReadUInt32();
                var _transactionCount = r.ReadVarInt();

               // var testTx = r.ReadBytes(200);

                for (int i=0;i< _transactionCount; i++) {
                    var tVers= r.ReadUInt32();

                     var testTx = r.ReadBytes(200);
                    var inputCount = r.ReadVarInt();
                    for(int j = 0; j < inputCount; j++) {

                    }
                    var outputCount = r.ReadVarInt();
                    for (int j = 0; j < outputCount; j++) {
                        var outVala = r.ReadBytes(8);
                        var outValaa = ReverseBytes(outVala);
                        var outVal= BitConverter.ToInt32(outValaa);
                        var outSizeA = r.ReadBytes(4);
                        var outSize = BitConverter.ToInt16(outSizeA);
                    }
                }
            }
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
            for (var i = 0; i < rawData.Length; i += 2) // Step two for 2x8 bits=16
                Swap(ref rawData[i], ref rawData[i + 1]);
            return rawData;
        }

        static byte[] FlipInt32(byte[] rawData) {
            for (var i = 0; i < rawData.Length; i += 4) {// Step four for 4x8 bits=32
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
            if (t < 0xfd) return t;
            if (t == 0xfd) return reader.ReadInt16();
            if (t == 0xfe) return reader.ReadInt32();
            if (t == 0xff) return reader.ReadInt64();

            throw new InvalidDataException("Reading Var Int");
        }
    }
}
