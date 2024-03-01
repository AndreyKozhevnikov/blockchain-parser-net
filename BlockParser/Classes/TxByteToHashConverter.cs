using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockParser.Classes {
    public class TxByteToHashConverter {
        public string Convert(List<byte> data) {
            return Convert(data.ToArray());
        }
        public string Convert(byte[] data) {
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(data.ToArray());
            byte[] hashValue2 = mySHA256.ComputeHash(hashValue);
            var tmp = hashValue2.Reverse().ToArray();
            var res = BitConverter.ToString(tmp.ToArray()).Replace("-", null).ToLower();
            return res;
        }
    }
}
