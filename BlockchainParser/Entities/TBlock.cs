using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockParser;
public class TBlock {
    public int Size { get; set; }
    public string VersionNumber { get; set; }
    public string PrevBlockHash { get; set; }
    public string Hash { get; set; }
    public string MerkleRoot { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Bits { get; set; }
    public uint Nonce { get; set; }
    public long TransactionCount { get; set; }
    public List<TTransaction> Transactions { get; set; }

}

