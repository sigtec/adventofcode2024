using System.Text.RegularExpressions;

var dim = (x: 101, y: 103);
var mid = (x: (dim.x-1)/2, y: (dim.y-1)/2);
var regex = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
var input = File.ReadAllText("../../../input.txt");

var robots = regex.Matches(input).OfType<Match>()
  .Select( m => new Robot( int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), dim.x, dim.y) )
  .ToList();

var q1 = robots.Select( r => r.P(100) ).Count( p => p.x > mid.x && p.y > mid.y );
var q2 = robots.Select( r => r.P(100) ).Count( p => p.x > mid.x && p.y < mid.y );
var q3 = robots.Select( r => r.P(100) ).Count( p => p.x < mid.x && p.y > mid.y );
var q4 = robots.Select( r => r.P(100) ).Count( p => p.x < mid.x && p.y < mid.y );

Console.WriteLine($"part1: total safety factor: {q1 * q2 * q3 * q4} ");

// check for chrismas tree shape
// it's unclear how the christmas tree looks like;
// for now, we search for symmetric diagonal rows of robots...
for(int t = 0; t < int.MaxValue; ++t)
{
  foreach(var robot in robots)
  {
    var (x, y) = robot.P(t);
    if (Enumerable.Range(1, 5).All( i => robots.Any( r => r.P(t) == (x - i, y + i)) && robots.Any( r => r.P(t) == (x + i, y + i)) ))
    {
      Console.WriteLine($"part2: fewest number of seconds that must elapse for the robots to display the Easter egg: {t} ");
      return;
    }
  }
  if(t % 1000 == 0) 
  {
    Console.WriteLine($"still running at t={t}");
  }
}


class Robot
{
  readonly int x0, y0, vx, vy, mx, my;

    public Robot(int x0, int y0, int vx, int vy, int mx, int my)
    {
        this.x0 = x0;
        this.y0 = y0;
        this.vx = vx;
        this.vy = vy;
        this.mx = mx;
        this.my = my;
    }

    public (int x, int y) P(int t) => ( ((vx * t + x0)%mx + mx)%mx, ((vy * t + y0)%my + my)%my );

}