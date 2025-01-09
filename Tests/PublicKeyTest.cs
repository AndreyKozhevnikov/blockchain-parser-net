using BlockParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    [TestFixture]
    public class PublicKeyTest {
        [Test]
        public void p2sh() {
            //arrange
            var parser = new BlockChainParser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            var tx = blockList[0].Transactions.Where(x => x.Hash == "ba4815d5457d6b2c32a1c613dc8270d6cf61b78c6c91dc9eeafc525e40690729").First();
            //assert

            Assert.AreEqual("033bf7a71b30f06669d6114e0f13ea1fe0407db6db431033d3f2a3168322714fd8", tx.Inputs[0].OutputPublicKey);
            Assert.AreEqual("3BThywauYTyDbUnNBCgH6boLDwwjW7GtGU", tx.Inputs[0].OutputAddress);
            //Assert.AreEqual(tx.Inputs[0].OutputNonce, "008453b569bd2210f221a22e4daeb4e89a2940398693769d6dd743ad97e7990f99");
        }
    }
}
