using System.Diagnostics;

namespace BlockParser;
[DebuggerDisplay("TxId: {TxId}")]
public class TInput {
    public string TxId { get; set; }
    public long OutputNumber { get; set; }
    public long ScriptLength { get; set; }
    public string Script { get; set; }
    public string Sequence { get; set; }

    public string OutputPublicKey { get; set; }
    public string OutputAddress { get; set; }
    public string OutputNonce { get; set; }

}

