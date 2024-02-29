using BlockParser.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    [TestFixture]
    public class ParserTests {

        [Test]
        public void BlockCommonInfo() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
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
            Assert.AreEqual(new DateTime(2018,7,7,7,52,59), b.TimeStamp);
        }

        [Test]
        public void Block_Hash() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            Assert.AreEqual("000000000000000000280e2daa0be4346cf4d52d3f8ee7aaf4839432a00d1a1a", b.Hash);
        }

        [Test]
        public void Transaction_Hash() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            var t = b.Transactions[0];
            Assert.AreEqual("fb15902777b7cac07fe2e99f2009fb6b34f2da7a7451b6afd316cd4e1bf8cc8c", t.Hash);
        }

        [Test]
        public void Output_publicAddress1() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            var t = b.Transactions[0];
            var o = t.Outputs[0];
            Assert.AreEqual("1CK6KHY6MHgYvmRQ4PAafKYDrg1ejbH1cE", o.Address);
        }
        [Test]
        public void Output_publicAddress2() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            var t = b.Transactions[1];
            var o = t.Outputs[0];
            Assert.AreEqual("14MMSkD4HyDrWetxD6TgZwe7nRXb8uKbSL", o.Address);
        }
        [Test]
        public void Output_publicAddress3() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            var t = b.Transactions[1];
            var o = t.Outputs[1];
            Assert.AreEqual("bc1qwqdg6squsna38e46795at95yu9atm8azzmyvckulcc7kytlcckxswvvzej", o.Address);
        }
        
                [Test]
        public void Output_publicAddress4() {
            //arrange
            var parser = new Parser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            //assert
            var b = blockList[0];
            var t = b.Transactions[2];
            var o = t.Outputs[0];
            Assert.AreEqual("31zVTJ78SqX9z9hYZLfSiJnwiEfqqGSNHQ", o.Address);
        }
    }
}
