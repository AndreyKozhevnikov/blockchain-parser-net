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
            Assert.AreEqual(tx.Inputs[0].OutputNonce, "008453b569bd2210f221a22e4daeb4e89a2940398693769d6dd743ad97e7990f99");
        }

        [Test]
        public void P2WPKH() {
            //arrange
            var parser = new BlockChainParser();
            var fl = File.OpenRead("testdata\\oneBlockData.dat");
            var reader = new BinaryReader(fl);
            //act
            var blockList = parser.ParseCore(reader);
            var tx = blockList[0].Transactions.Where(x => x.Hash == "5aa66188e9555e3826476bff817d2136db6d9ae98b21cd94488fc349eb857ccf").First();
            //assert

            Assert.AreEqual("0234f1b2f7fcd95a0541290feacd759f5cc4494de524b266055300e059688554a9", tx.Inputs[0].OutputPublicKey);
            Assert.AreEqual("bc1qrnk7j7hyvfcm372x7dl0n82jckyve8ys2dgy93", tx.Inputs[0].OutputAddress);
            Assert.AreEqual(tx.Inputs[0].OutputNonce, "00a9f52a377e32afcdbac79374c204b603dd5e0b9a6562b07cc547a37795651e19");
        }

        [Test]
        public void GetNonceFromScript() {
            //arrange
            var parser = new BlockChainParser();
            var script = "30450221008453b569bd2210f221a22e4daeb4e89a2940398693769d6dd743ad97e7990f9902204833964af36e956cc46e623199741515680bf18f92fe7e0e97589deda9893a4f01";
            //act
            var nonce = parser.GetNonceFromScript(script);
            //assert
            Assert.AreEqual("008453b569bd2210f221a22e4daeb4e89a2940398693769d6dd743ad97e7990f99", nonce);
        }
    }
}
