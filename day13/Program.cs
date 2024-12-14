using System.Text.RegularExpressions;

var regex = new Regex(@"Button A: X\+(\d+), Y\+(\d+)\s+Button B: X\+(\d+), Y\+(\d+)\s+Prize: X=(\d+), Y=(\d+)");
var input = File.ReadAllText("../../../input.txt");

// part 1
long tokens = 0L;
foreach (var match in regex.Matches(input).OfType<Match>())
{
  var ax = long.Parse(match.Groups[1].Value);
  var ay = long.Parse(match.Groups[2].Value);
  var bx = long.Parse(match.Groups[3].Value);
  var by = long.Parse(match.Groups[4].Value);
  var px = long.Parse(match.Groups[5].Value);
  var py = long.Parse(match.Groups[6].Value);

  tokens += Solve(ax, ay, bx, by, px, py) ?? 0L;
}
Console.WriteLine($"part 1: fewest tokens to spend to win all possible prizes: {tokens}");

// part 2
tokens = 0L;
foreach (var match in regex.Matches(input).OfType<Match>())
{
  var ax = long.Parse(match.Groups[1].Value);
  var ay = long.Parse(match.Groups[2].Value);
  var bx = long.Parse(match.Groups[3].Value);
  var by = long.Parse(match.Groups[4].Value);
  var px = long.Parse(match.Groups[5].Value) + 10000000000000L;
  var py = long.Parse(match.Groups[6].Value) + 10000000000000L;

  tokens += Solve(ax, ay, bx, by, px, py) ?? 0L;
}
Console.WriteLine($"part 2: fewest tokens to spend to win all possible prizes: {tokens}");



long? Solve(long ax, long ay, long bx, long by, long px, long py)
{
  // solve the linear equation system for variables a and b:
  // a * ax + b * bx == px
  // a * ay + b * by == py

  // divisor determinant
  var d = ax * by - bx * ay;

  if (d == 0L)
  {
    // d==0 means the two equations are linearly dependent.
    // in that case, a solution of one quation will always also solve the other.
    throw new("multiple solutions are valid. Best solution needs to be found using GcdExt(). To be implemented if needed.");
  }

  // divident determinant for a
  var da = px * by - bx * py;
  if (da % d != 0)
  {
    // no integer solution for a
    return null;
  }
  
  var a = da / d;
  if (a < 0)
  {
    // no non-negative integer solution for a
    return null;
  }

  // divident determinant for b
  var db = ax * py - px * ay;
  if (db % d != 0)
  {
    // no integer solution for b
    return null;
  }
 
  var b = db / d;
  if (b < 0)
  {
    // no non-negative integer solution for b
    return null;
  }

  // returns number of tokens to be spent
  return 3L * a + b;
}

/*
// gcd(a,b) = x * a + y * b
static (long gcd, long x, long y) GcdExt(long a, long b)
{
  if (b == 0)
  {
      return (a, 1, 0);
  }

  var (gcd, x1, y1) = GcdExt(b, a % b);
  return (gcd, y1, x1 - (a / b) * y1);
}

*/