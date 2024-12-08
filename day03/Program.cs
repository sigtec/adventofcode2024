using System.Text.RegularExpressions;

var input = File.ReadAllText("../../../input.txt");

var part1Regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
var part1Result = part1Regex.Matches(input).Sum( m => Int32.Parse(m.Groups[1].Value) * Int32.Parse(m.Groups[2].Value) );
Console.WriteLine($"part1: part1Result={part1Result}");

var part2Regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
var enabled = true;
var part2Result = 0;
foreach(Match match in part2Regex.Matches(input))
{
    if (match.Value == "do()")
        enabled = true;
    else if (match.Value == "don't()")
        enabled = false;
    else if (match.Value.StartsWith("mul") && enabled)
        part2Result += Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
}
Console.WriteLine($"part2: part1Result={part2Result}");
