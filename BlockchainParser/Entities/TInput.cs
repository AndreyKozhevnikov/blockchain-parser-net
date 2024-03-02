using System.Diagnostics;

namespace BlockParser;
[DebuggerDisplay("TxId: {TxId}")]
public class TInput {
    public string TxId { get; set; }
    public int OutputNumber { get; set; }
    public long ScriptLength { get; set; }
    public string Script { get; set; }
    public string Sequence { get; set; }

}

