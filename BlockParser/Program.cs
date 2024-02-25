using BlockParser.Classes;
using System;

namespace MyApp {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var prs = new Parser();
            prs.Parse();
        }
    }
}