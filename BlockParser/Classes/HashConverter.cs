namespace HashConverterNS {
    public class HashConverter {
        public static string Convert(string hash) {
            hash = hash.ToLower();
            hash = Reverse(hash);

            string res = string.Empty;
            var k = hash.Length ;
            for (int i = 0; i < k; i += 2) {
                res = res + hash[i + 1];
                res = res + hash[i];
            }
            return res;


        }
        public static string Reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static byte[] StringToByteArray(String hex) {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
