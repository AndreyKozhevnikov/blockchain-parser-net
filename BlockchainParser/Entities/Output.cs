using System.Diagnostics;

namespace BlockParser;
[DebuggerDisplay("Address: {Address}")]
public class Output {
    public int Value { get; set; }
    public long ScriptSize { get; set; }
    public string Script { get; set; }

    public string Address { get; set; }
}

