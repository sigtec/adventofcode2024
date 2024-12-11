using System.Data;

var map = File.ReadAllLines("../../../input.txt");

var frequencies = map.SelectMany( line => line.ToArray().Where( c => c != '.') ).Distinct().ToList();

// part 1
var antinodes1 = new HashSet<(int row, int col)>();

foreach(var frequency in frequencies)
{
  // find all pairs
  foreach(var firstAntenna in GetAllAntennasForFrequency(map, frequency))
  {
    foreach(var secondAntenna in GetAllAntennasForFrequency(map, frequency).Where( a => a != firstAntenna))
    {
      // as each pair is found twice, we only need to add the antinode at one side in each iteration
      var row = 2 * firstAntenna.row - secondAntenna.row;
      var col = 2 * firstAntenna.col - secondAntenna.col;

      // check if the antinode coordinate is in the map
      if(IsLocationOnMap(row, col, map) && !antinodes1.Contains((row, col)))
      {
        antinodes1.Add((row, col));
      }
    }
  }
}
Console.WriteLine($"part 1: unique locations within the bounds of the map that contain an antinode: {antinodes1.Count}");

// part 2 - regard resonant harmonics
var antinodes2 = new HashSet<(int row, int col)>();

foreach(var frequency in frequencies)
{
  // find all pairs
  foreach(var firstAntenna in GetAllAntennasForFrequency(map, frequency))
  {
    foreach(var secondAntenna in GetAllAntennasForFrequency(map, frequency).Where( a => a != firstAntenna))
    {
      // as each pair is found twice, we only need to add the antinodes at one side in each iteration
      // increments
      var dr = firstAntenna.row - secondAntenna.row;
      var dc = firstAntenna.col - secondAntenna.col;
      
      // start with first antenna position
      var row = firstAntenna.row;
      var col = firstAntenna.col;

      while(IsLocationOnMap(row, col, map))
      {
        if(!antinodes2.Contains((row, col)))
        {
          antinodes2.Add((row, col));
        }
        row += dr;
        col += dc;
      }
    }
  }
}
Console.WriteLine($"part 2: unique locations within the bounds of the map that contain an antinode with regarding resonant harmonics: {antinodes2.Count}");

IEnumerable<(int row, int col)> GetAllAntennasForFrequency(string[] map, char frequency)
{
  for(var r = 0; r < map.Length; ++r)
  {
    for(var c = 0; c < map[r].Length; ++c)
    {
      if(map[r][c] == frequency)
      {
        yield return (r, c);
      }
    }
  }
}

bool IsLocationOnMap(int row, int col, string[] map)
  => (row >= 0 && row < map.Length && col >= 0 && col < map[row].Length);