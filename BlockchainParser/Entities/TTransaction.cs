namespace BlockParser.Entities {
    public class TTransaction {
        public uint Version { get; set; }
        public long InputCount { get; set; }
        public long OutputCount { get; set; }
        public bool HasWitness { get; set; }
        
        public List<Input> Inputs { get; set; }
        public List<Output> Outputs{ get; set; }
        public List<Witness> Witnesses{ get; set; }
        public string LockTime { get; set; }
        public string Hash { get; set; }


    }
}
