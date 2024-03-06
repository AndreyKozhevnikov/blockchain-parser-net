using BlockParser;
using NUnit.Framework;

namespace Tests;
[TestFixture]
public class ParserTests {

    [Test]
    public void BlockCommonInfo() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockData.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
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
        Assert.AreEqual(new DateTime(2018, 7, 7, 7, 52, 59), b.TimeStamp);
        var o = b.Transactions[0].Outputs[0];
        Assert.AreEqual(1262779908, o.Value);
        
    }
    [Test]
    public void TransactionCommonInfo() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockData.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
        //assert
        Assert.AreEqual(1, blockList.Count);
        var b = blockList[0];
        Assert.AreEqual(1305, b.TransactionCount);
        var t = b.Transactions[0];

        Assert.AreEqual(true, t.HasWitness);
        Assert.AreEqual(1, t.InputCount);
        Assert.AreEqual(1, t.Version);
        Assert.AreEqual("00000000", t.LockTime);
        Assert.AreEqual(3, t.OutputCount);

    }
    [Test]
    public void Block_Hash() {
        //arrange
        var parser = new BlockChainParser();
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
        var parser = new BlockChainParser();
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
    public void Input_TxId() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockData.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
        //assert
        var b = blockList[0];
        var i = b.Transactions[1].Inputs[0];
        Assert.AreEqual("b0150d929fe9bf73cc065e74410bf828914f49e90d5cfbf6c12457c42364b9cc", i.TxId);
    }
    [Test]
    public void Input_OutputNumber() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockData.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
        //assert
        var b = blockList[0];
        var i = b.Transactions[3].Inputs[0];
        Assert.AreEqual(26, i.OutputNumber);
    }
    [Test]
    public void Output_publicAddress1() {
        //arrange
        var parser = new BlockChainParser();
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
        var parser = new BlockChainParser();
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
        var parser = new BlockChainParser();
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
        var parser = new BlockChainParser();
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

    [Test]
    public void BlockCommonInfo_P2PK() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockP2PK.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
        //assert
        Assert.AreEqual(1, blockList.Count);
        var b = blockList[0];
        Assert.AreEqual("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f", b.Hash);
        Assert.AreEqual("0000000000000000000000000000000000000000000000000000000000000000", b.PrevBlockHash);
        Assert.AreEqual("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b", b.MerkleRoot);
        Assert.AreEqual(285, b.Size);
        Assert.AreEqual(1, b.TransactionCount);
        Assert.AreEqual("0x00000001", b.VersionNumber);
        Assert.AreEqual("0x1d00ffff", b.Bits);
        Assert.AreEqual(2083236893, b.Nonce);
        Assert.AreEqual(new DateTime(2009, 1, 3, 18, 15, 05), b.TimeStamp);
        Assert.AreEqual(1, b.TransactionCount);
        var t = b.Transactions[0];
        Assert.AreEqual(1, t.InputCount);
        Assert.AreEqual(1, t.OutputCount);
        var o = t.Outputs[0];
        Assert.AreEqual(5000000000, o.Value);
        Assert.AreEqual("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa", o.Address);


    }

    [Test]
    public void BlockCommonInfo_case1() {
        //arrange
        var parser = new BlockChainParser();
        var fl = File.OpenRead("testdata\\oneBlockcase1.dat");
        var reader = new BinaryReader(fl);
        //act
        var blockList = parser.ParseCore(reader);
        //assert
        Assert.AreEqual(1, blockList.Count);
        var b = blockList[0];
        Assert.AreEqual("00000000afe94c578b4dc327aa64e1203283c5fd5f152ce886341766298cf523", b.Hash);
        Assert.AreEqual("000000007da75864998b222da93fc061191ec3cb0063d42b55ee864ee8d82b60", b.PrevBlockHash);
        Assert.AreEqual("c5997d1cad40afec154aa99b8988e97b1f113d8076357a77572455574765a533", b.MerkleRoot);
   
        Assert.AreEqual(2, b.TransactionCount);
   
        Assert.AreEqual(new DateTime(2009, 12, 14, 3, 28, 09), b.TimeStamp);
        var t0 = b.Transactions[0];
        var t1 = b.Transactions[1];
        Assert.AreEqual("0e0abb91667c0bb906e9ed8bbbfb5876fccb707c2d9e7dab3603b57f41ec431f", t0.Hash);
        Assert.AreEqual("3a5769fb2126d870aded5fcaced3bc49fa9768436101895931adb5246e41e957", t1.Hash);
        var o = b.Transactions[1].Outputs[0];
        Assert.AreEqual(1595435000000, o.Value); //to change
        Assert.AreEqual("1LPaBQDXUkzav1hNAHZKpAf1upqjzuoprU", o.Address);

    }
}

