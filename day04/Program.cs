var input = File.ReadAllLines("../../../input.txt");

var search = "XMAS";
var part1 = 0;
var part2 = 0;
for(var r = 0; r < input.Length; ++r)
{
    for(var c = 0; c < input[r].Length; ++c)
    {
        part1 += SearchForStringAtPosition(input, r, c, search);
        part2 += SearchForXMASAtPosition(input, r, c) ? 1 : 0;
    }
}

Console.WriteLine($"part1: {part1}");
Console.WriteLine($"part1: {part2}");


int SearchForStringAtPosition(string[] input, int r, int c, string search)
{
    if(input[r][c] != search[0])
        return 0;
    var result = 0;
    // iterate over deltas for rows and columns: dr in (-1, 0, 1), dc in (-1, 0, 1)
    for(var dr = -1; dr <= 1; ++dr)
    {
        for(var dc = -1; dc <= 1; ++dc)
        {
            if (SearchForStringAtPositionInDirection(input, r, c, search, dr, dc))
                ++result;
        }
    }
    return result;
}

bool SearchForStringAtPositionInDirection(string[] input, int r, int c, string s, int dr, int dc)
{
    try
    {
        for(var i = 0; i < s.Length; ++i)
        {
            if(input[r + dr * i][c + dc * i] != s[i])
                return false;
        }
        return true;
    } 
    catch(IndexOutOfRangeException)
    {
        return false;
    }
}

bool SearchForXMASAtPosition(string[] input, int r, int c)
{
    // bound guards
    if(r == 0) 
        return false;
    if(c == 0)
        return false;
    if(r == input.Length - 1)    
        return false;
    if(c == input[r].Length - 1)
        return false;
    // find the 'A' in the middle
    if(input[r][c] != 'A')
        return false;
    return 
        (input[r-1][c-1] == 'M' && input[r-1][c+1] == 'M' && input[r+1][c-1] == 'S' && input[r+1][c+1] == 'S') || 
        (input[r-1][c-1] == 'M' && input[r-1][c+1] == 'S' && input[r+1][c-1] == 'M' && input[r+1][c+1] == 'S') ||
        (input[r-1][c-1] == 'S' && input[r-1][c+1] == 'M' && input[r+1][c-1] == 'S' && input[r+1][c+1] == 'M') ||
        (input[r-1][c-1] == 'S' && input[r-1][c+1] == 'S' && input[r+1][c-1] == 'M' && input[r+1][c+1] == 'M');
}