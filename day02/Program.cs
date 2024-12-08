var input = File.ReadAllLines("../../../input.txt");

var reports = input.Select( s => s.Split(' ').Select( s => int.Parse(s) ).ToArray() );

var numberOfSafeReports = reports.Count( r => IsReportSafe(r) );
Console.WriteLine($"part 1: numberOfSafeReports={numberOfSafeReports}");

var numberOfSafeReportsWithProblemDampener = reports.Count( r => IsReportSafeWithProblemDampener(r) );
Console.WriteLine($"part 2: numberOfSafeReportsWithProblemDampener={numberOfSafeReportsWithProblemDampener}");

bool IsReportSafe(int[] levels)
{
    // report only counts as safe if both of the following are true:
    var sign = Math.Sign(levels[1] - levels[0]);
    for( var i = 1; i < levels.Length; ++i )
    {
        var diff = levels[i] - levels[i-1];
        // The levels are either all increasing or all decreasing.
        if(Math.Sign(diff) != sign)
            return false;
        // Any two adjacent levels differ by at least one and at most three.
        if(Math.Abs(diff) < 1)
            return false;
        if(Math.Abs(diff) > 3)
            return false;
    }
    return true;
}

bool IsReportSafeWithProblemDampener(int[] levels)
{
    if(IsReportSafe(levels)) 
        return true;
    // try to remove the (i)th entry and check if it's save then.
    for(var i = 0; i < levels.Length; ++i)
    {
        var reducedLevels = levels.Take(i).Concat(levels.Skip(i).Skip(1)).ToArray();
        if(IsReportSafe(reducedLevels))
            return true;
    }
    return false;
}

