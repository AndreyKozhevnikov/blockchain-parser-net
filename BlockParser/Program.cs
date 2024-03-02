using BlockParser;
using System;

namespace MyApp;
internal class Program {
    static void Main(string[] args) {
        Console.WriteLine("Hello World!");
        var filePath = "blk03331.dat";
        var prs = new BlockChainParser();
        List<TBlock> result = prs.Parse(filePath,false);
    }
}
