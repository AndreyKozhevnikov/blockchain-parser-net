using BlockParser.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    [TestFixture]
    public  class ScriptParserTests {

        [Test]
        public void ParseAddress1() {
            //arrange
            var fl = File.OpenRead("testdata\\script1.dat");
            var reader = new BinaryReader(fl);
            var parser = new ScriptParser();
            var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
            //act
            var res = parser.GetAddressFromScript(bytes);
            //Assert
            Assert.AreEqual("1CK6KHY6MHgYvmRQ4PAafKYDrg1ejbH1cE", res);
        }
        [Test]
        public void ParseAddress3() {
            //arrange
            var fl = File.OpenRead("testdata\\script3.dat");
            var reader = new BinaryReader(fl);
            var parser = new ScriptParser();
            var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
            //act
            var res = parser.GetAddressFromScript(bytes);
            //Assert
            Assert.AreEqual("31zVTJ78SqX9z9hYZLfSiJnwiEfqqGSNHQ", res);
        }
        [Test]
        public void ParseAddressB() {
            //arrange
            var fl = File.OpenRead("testdata\\scriptB.dat");
            var reader = new BinaryReader(fl);
            var parser = new ScriptParser();
            var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
            //act
            var res = parser.GetAddressFromScript(bytes);
            //Assert
            Assert.AreEqual("bc1qwqdg6squsna38e46795at95yu9atm8azzmyvckulcc7kytlcckxswvvzej", res);
        }
    }
}
