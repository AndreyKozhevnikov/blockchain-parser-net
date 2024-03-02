Parses blk*.dat files into .NET classes


# Creates a list of objects with the following data structure:

Block
- hash
- previous block hash
- time
- list of transactions
- other common info (merkleroot, nonce, etc.)

Transaction
- hash
- inputs
- outputs
- common info

Input
- transaction id
- output number
- script

Output
- value
- output address
- script

# How to use

```cs
var filePath = "blk01307.dat";
var prs = new BlockChainParser();
List<TBlock> result = prs.Parse(filePath);
```


# Tips (BTC)

bc1q7uas0hqke0cdp43rnh6u43d2yclhzqzkjav9cj 
