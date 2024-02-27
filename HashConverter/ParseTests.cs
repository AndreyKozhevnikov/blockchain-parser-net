using BlockParser.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    [TestFixture]
    public class ParseTests {

        [Test]
        public void TestBase1() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("oneBlockData.dat");
            var reader=new BinaryReader(fl);
            //act
            var blockList=parser.ParseCore(reader);
            //assert
            Assert.AreEqual(1, blockList.Count);
            var b = blockList[0];
            Assert.AreEqual("0000000000000000002b81a1bd099b0f3fb93ed811f1c95dddf118dce63bf511", b.PrevBlockHash);
            Assert.AreEqual("a7135c27e96c5d39fd6a96b8c9dfa9cade3865802b1ca77a6d513f48ca3734e4", b.MerkleRoot);
            Assert.AreEqual(591554, b.Size);
            Assert.AreEqual(1305, b.TransactionCount);
            Assert.AreEqual("0x20000000", b.VersionNumber);
            Assert.AreEqual("0x17347a28", b.Bits);
            Assert.AreEqual(2853688004, b.Nonce);
        //    Assert.AreEqual("5363678461481", b.Difficulty);
            Assert.AreEqual(new DateTime(2018,7,7,7,52,59), b.TimeStamp);

            


            Assert.AreEqual("000000000000000000280E2DAA0BE4346CF4D52D3F8EE7AAF4839432A00D1A1A", b.Hash);






        }
    }
}
