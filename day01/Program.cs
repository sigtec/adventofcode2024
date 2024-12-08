using System.Text.RegularExpressions;

var lineParserRegex = new Regex(@"^(\d+)\W+(\d+)$");

var lists = File.ReadAllLines("../../../input.txt")
    .Select( s => lineParserRegex.Match(s) )
    .Where( m => m.Success )
    .Select( m => (left: int.Parse(m.Groups[1].Value), right: int.Parse(m.Groups[2].Value)) );

var left = lists.Select( x => x.left ).Order().ToArray();
var right = lists.Select( x => x.right ).Order().ToArray();

// part 1
var totalDistance = 0L;
for(int i = 0; i < left.Length; ++i)
{
    totalDistance += Math.Abs(left[i] - right[i]);
}

Console.WriteLine($"part1: totalDistance={totalDistance}");

// part 2
var similarityScore = 0L;
for(int i = 0; i < left.Length; ++i)
{
    similarityScore += left[i] * right.Count( x => x == left[i]);
}

Console.WriteLine($"part2: similarityScore={similarityScore}");