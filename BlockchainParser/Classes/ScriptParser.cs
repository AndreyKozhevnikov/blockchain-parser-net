using NBitcoin;

namespace BlockParser;

public class ScriptParser {

    public string? GetAddressFromScript(byte[] data) {
        var scr = Script.FromBytesUnsafe(data);
        var address = scr.GetDestinationAddress(Network.Main)?.ToString(); ;
        return address;
    }
}

