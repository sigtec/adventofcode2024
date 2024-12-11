var map = File.ReadAllLines("../../../input.txt")
  .Select( line => line.Select( c => int.Parse(c.ToString())).ToArray() )
  .ToArray();

var trailHeads = new List<TrailHead>();

for(var row = 0; row < map.Length; ++row)
{
  for(var col = 0; col < map[row].Length; ++col)
  {
    if(map[row][col] == 0)
    {
      trailHeads.Add(new TrailHead(map, new Position(row, col)));
    }
  }
}

Console.WriteLine($"sum of all trailhead scores: {trailHeads.Sum( x => x.Score)}");
Console.WriteLine($"sum of all trailhead ratings: {trailHeads.Sum( x => x.Rating)}");

