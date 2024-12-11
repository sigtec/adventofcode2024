using System.Runtime.InteropServices;

public struct TrailHead
{
  public TrailHead(int[][] map, Position position) : this()
  {
      this.Position = position;
      var reachableTargets = FindReachableTargets(map, position).ToArray();
      this.Rating = reachableTargets.Length;
      this.Score = reachableTargets.Distinct().Count();
  }

  public Position Position {get;}
  
  public int Score {get;}
  public int Rating {get;}

  private static IEnumerable<Position> FindReachableTargets(int[][] map, Position position)
  {
    var currentHeight = map[position.Row][position.Col];
    if(currentHeight == 9)
    {
      yield return new Position(position.Row, position.Col);
    }
    else
    {
      // top
      if(position.Row > 0 && map[position.Row-1][position.Col] == currentHeight + 1)
      {
        foreach(var target in FindReachableTargets(map, new Position(position.Row-1, position.Col)))
        {
          yield return target;
        }
      }
      // right
      if(position.Col < map[position.Row].Length - 1 && map[position.Row][position.Col+1] == currentHeight + 1)
      {
        foreach(var target in FindReachableTargets(map, new Position(position.Row, position.Col+1)))
        {
          yield return target;
        }
      }
      // bottom
      if(position.Row < map.Length - 1 && map[position.Row+1][position.Col] == currentHeight + 1)
      {
        foreach(var target in FindReachableTargets(map, new Position(position.Row+1, position.Col)))
        {
          yield return target;
        }
      }
      // left
      if(position.Col > 0 && map[position.Row][position.Col-1] == currentHeight + 1)
      {
        foreach(var target in FindReachableTargets(map, new Position(position.Row, position.Col-1)))
        {
          yield return target;
        }
      }
    }
  }

}