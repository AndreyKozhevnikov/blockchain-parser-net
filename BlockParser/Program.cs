using BlockParser;
using System;

namespace MyApp;
internal class Program {
    static void Main(string[] args) {
        Console.WriteLine("Hello World!");
        //var filePath = @"f:\bt\blk00003.dat";
        // var filePath = @"c:\bt\blocks\blk00000.dat";

        var watch = System.Diagnostics.Stopwatch.StartNew();
       // var filePath = @"blk00032.dat";
        var filePath = @"blk00306.dat";
        var prs = new BlockChainParser();
        List<TBlock> result = prs.Parse(filePath);
        var t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds );
        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
    t.Hours,
    t.Minutes,
    t.Seconds
    );
        Console.WriteLine();
        Console.WriteLine("finish import -{0} {1}",answer,result.Count);
    }
}
