using NBitcoin;

namespace BlockParser;
//https://en.bitcoin.it/wiki/Script
public class ScriptParser {

    public string? GetAddressFromScript(byte[] data) {
        var scr = Script.FromBytesUnsafe(data);
        var address = scr.GetDestinationAddress(Network.Main)?.ToString();
        if(address == null) {
            if(data.Length == 67 && data[0]==65) {//p2pk

                //byte first = data[0];
                byte last = data[data.Length - 1];

             //   var tst = BitConverter.ToInt16(new byte[1] { data[0] });


                if(last == 0xac) { //p2pk
                    var publKeyBT = data.SubArray(1, data.Length - 2);
                    var dataST = BitConverter.ToString(data).Replace("-", null).ToLower();
                    //if(dataST == "4104f816b480f87144ec4de5862adf028ff66cc6964250325d53fd22bf8922824b6f1e1f2c881ae0608ec77ebf88a75c66d3099113a7343238f2f7a0ebb91a4ed335ac") {
                    //    using(FileStream fileStream = new FileStream("scriptp2pk_1.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
                    //        fileStream.Write(data, 0, data.Length);
                    //    }
                    //}
                    //todo 7729045ec9b7cfe62811c7fee5b9b2839a1e45f3827867776b6671cabae84462 fails
                    //TxoutType::PUBKEY
                  
                    
                    try {
                        PubKey publicKey = new PubKey(publKeyBT);
                        address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();
                    }
                    catch { }

                }
            }
        }
        return address;
    }
}

