using System.Diagnostics;

namespace BlockParser;
[DebuggerDisplay("Transaction: {Hash}")]
public class TTransaction {
    public uint Version { get; set; }
    public long InputCount { get; set; }
    public long OutputCount { get; set; }
    public bool HasWitness { get; set; }

    public List<TInput> Inputs { get; set; }
    public List<TOutput> Outputs { get; set; }
    public List<Witness> Witnesses { get; set; }
    public string LockTime { get; set; }
    public string Hash { get; set; }
}

