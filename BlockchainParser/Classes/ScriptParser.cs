using NBitcoin;

namespace BlockParser;
//https://en.bitcoin.it/wiki/Script
public class ScriptParser {

    public string? GetAddressFromScript(byte[] data) {
        var scr = Script.FromBytesUnsafe(data);
        var address = scr.GetDestinationAddress(Network.Main)?.ToString();
        if(address == null) {
            byte last = data[data.Length - 1];
            if(last == 0xac) { //p2pk
                var publKeyBT = data.SubArray(1, data.Length - 2);
                var publicKey = new PubKey(publKeyBT);
                address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();
            }
        }
        return address;
    }
}

