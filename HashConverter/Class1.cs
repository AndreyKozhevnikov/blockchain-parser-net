using NUnit.Framework;

namespace HashConverterNS {
    public class HashConverter {
        public static string Convert(string hash) {
            hash = hash.ToLower();
            hash = Reverse(hash);

            string res = string.Empty;

            for (int i = 0; i < 64; i += 2) {
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
    }

    [TestFixture]
    public class HashConverterTest {
        [Test]
        public void Test() {
            var input = "11F53BE6DC18F1DD5DC9F111D83EB93F0F9B09BDA1812B000000000000000000";
            var res = HashConverter.Convert(input);
            Assert.AreEqual("0000000000000000002b81a1bd099b0f3fb93ed811f1c95dddf118dce63bf511", res);

        }
    }
}
