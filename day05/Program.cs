using System.Runtime.CompilerServices;

var input = File.ReadAllLines("../../../input.txt");

// rules lookup; the right number is the index, the left numbers are the values
var rules = input
    .Where( s => s.Contains('|') )
    .Select( s => s.Split('|') )
    .ToLookup( a => int.Parse(a[0]), a => int.Parse(a[1]) );

var updates = input
    .Where( s => s.Contains(',') )
    .Select( s => s.Split(',').Select( s => int.Parse(s)).ToArray() );

var sumOfMedians = updates
    .Where( u => IsInRightOrder(u, rules) )
    .Sum( u => Median(u) );
    
Console.WriteLine($"part1: sumOfMedians={sumOfMedians}");    

sumOfMedians = sumOfMedians = updates
    .Where( u => !IsInRightOrder(u, rules) )
    .Select( u => Reorder(u, rules) )
    .Sum( u => Median(u) );

Console.WriteLine($"part2: sumOfMedians={sumOfMedians}");    

bool IsInRightOrder(int[] update, ILookup<int, int> rules)
{
    for(var i = 0; i < update.Length; ++i)
    {
        if(rules.Contains(update[i]))
        {
            // check if one of the constraints is violated
            // for the pages before
            for (var j = 0; j < i; ++j)
            {
                if(rules[update[i]].Contains(update[j]))
                    return false;
            }
        }

    }
    return true;
}

int Median(int[] values) => values[values.Length / 2];

int[] Reorder(int[] update, ILookup<int, int> rules)
{
    for(var i = 0; i < update.Length; ++i)
    {
        if(rules.Contains(update[i]))
        {
            // check if one of the constraints is violated
            // for the pages before
            for (var j = 0; j < i; ++j)
            {
                if(rules[update[i]].Contains(update[j]))
                {
                    // swap the two values
                    var tmp = update[i];
                    update[i] = update[j];
                    update[j] = tmp;
                    // recursion call to check next
                    return Reorder(update, rules);
                }
            }
        }
    }
    return update;
}