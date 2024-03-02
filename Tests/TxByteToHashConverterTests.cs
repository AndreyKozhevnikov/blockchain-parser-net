using BlockParser;
using NUnit.Framework;

namespace Tests;
[TestFixture]
public class TxByteToHashConverterTests {
    [Test]
    public void ConvertTest() {
        //arrange
        var conv = new TxByteToHashConverter();
        var fl = File.OpenRead("testdata\\txHashBytes.dat");
        var reader = new BinaryReader(fl);
        var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
        //act
        var res = conv.Convert(bytes);
        //assert
        Assert.AreEqual("fb15902777b7cac07fe2e99f2009fb6b34f2da7a7451b6afd316cd4e1bf8cc8c", res);
    }
}

