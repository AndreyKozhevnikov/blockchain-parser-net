using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockParser.Entities {
    public class TBlock {
        public int Size { get; set; }
        public string VersionNumber { get; set; }
        public string PrevBlockHash { get; set; }
        public string Hash { get; set; }
        public string MerkleRoot { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Bits { get; set; }
        public uint Nonce { get; set; }
        public string Locktime { get; set; }

        public long TransactionCount { get; set; }

        public List<TTransaction> Transactions { get; set; }
        public string Difficulty { get; set; }
    }
    public class TTransaction {
        public uint Version { get; set; }
        public long InputCount { get; set; }
        public long OutputCount { get; set; }
        public bool HasWitness { get; set; }
        
        public List<Input> Inputs { get; set; }
        public List<Output> Outputs{ get; set; }
        public List<Witness> Witnesses{ get; set; }
        public string LockTime { get; set; }
    }
    public class Input {
        public string TxId { get; set; }
        public string OutputNumber { get; set; }
        public long ScriptLength { get; set; }
        public string Script { get; set; }
        public string Sequence { get; set; }

    }
    public class Output {
        public int Value { get; set; }
        public long ScriptSize { get; set; }
        public string Script { get; set; }
    }
    public class Witness {
        public long Size { get; set; }
        public string WitnessValue { get; set; }
    }
}
