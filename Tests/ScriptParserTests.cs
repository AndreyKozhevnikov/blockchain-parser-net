using BlockParser;
using NUnit.Framework;

namespace Tests;
[TestFixture]
public class ScriptParserTests {

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


    [Test]
    public void ParseAddress1_P2PK() {
        //arrange
        var fl = File.OpenRead("testdata\\scriptP2PK.dat");
        var reader = new BinaryReader(fl);
        var parser = new ScriptParser();
        var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
        //act
        var res = parser.GetAddressFromScript(bytes);
        //Assert
        Assert.AreEqual("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa", res);
    }
    [Test]
    [Ignore("failed p2pk transaction 7729045ec9b7cfe62811c7fee5b9b2839a1e45f3827867776b6671cabae84462")]
    //transaction 7729045ec9b7cfe62811c7fee5b9b2839a1e45f3827867776b6671cabae84462 public key is not valid https://learnmeabitcoin.com/technical/keys/address/#base58-address-tool
    public void ParseAddress1_P2PK_1() {
        //arrange
        var fl = File.OpenRead("testdata\\scriptP2PK_1.dat");
        var reader = new BinaryReader(fl);
        var parser = new ScriptParser();

      //  var tst = reader.Read();


        var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
        //act
        var res = parser.GetAddressFromScript(bytes);
        //Assert
        Assert.AreEqual("1HSrPfMA5joCS5vTnRWQF7GyeodLQZHu6e", res);
    }
}

