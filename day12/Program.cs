

var map = File.ReadAllLines("../../../input.txt")
  .Select( line => line.ToCharArray() )
  .ToArray();

var regions = Region.Scan(map);

foreach(var region in regions )
{
  var corners = region.GetNumberOfCorners();
  Console.WriteLine($"- region of {region.PlantType} plants with area {region.Area}, perimeter {region.Perimeter} and {corners} corners.");
}

var part1FencePrice = regions.Sum( r => r.Area * r.Perimeter );
Console.WriteLine($"part 1: total price of fencing all regions: {part1FencePrice}");

// part 2: the number of edges and number of sides are equal.
var part2FencePrice = regions.Sum( r => r.Area * r.GetNumberOfCorners() );
Console.WriteLine($"part 2: total price of fencing all regions: {part2FencePrice}");




class Region 
{
  private Region(char plantType, int perimeter)
  {
      PlantType = plantType;
      Perimeter = perimeter;
  }

  // anchor where is was found first
  readonly HashSet<(int row, int col)> _coordinates = new();
  public char PlantType {get;}
  public int Perimeter {get; private set;}
  public int Area => _coordinates.Count;
  
  public int Row => _coordinates.First().row;
  public int Col => _coordinates.First().col;

  public int FencePrice => Area * Perimeter;

  public Region? JoinedWith {get; private set;} = null;

  public Region GetRootRegion() => JoinedWith?.GetRootRegion() ?? this;

  public void Add(int row, int col) => _coordinates.Add((row, col));

  public void Join(Region otherRegion)
  {
    if(otherRegion.PlantType != this.PlantType)
    {
      throw new("cannot join different plant types.");
    }
    if(this.JoinedWith != null)
    {
      throw new("this region already has been joined.");
    }
    otherRegion = otherRegion.GetRootRegion();
    foreach(var (row, col) in this._coordinates)
    {
      otherRegion._coordinates.Add((row, col));
    }
    this._coordinates.Clear();
    otherRegion.Perimeter += this.Perimeter;
    this.Perimeter = 0;

    this.JoinedWith = otherRegion;
  }

  
  public int GetNumberOfCorners()
  {
    var minRow = _coordinates.Select( x => x.row ).Min();
    var maxRow = _coordinates.Select( x => x.row ).Max()+1;
    var minCol = _coordinates.Select( x => x.col ).Min();
    var maxCol = _coordinates.Select( x => x.col ).Max()+1;
    // index is the corner coordinate, value is the number of corners (0, 1 or 2 in case of diagonal hits)
    var corners = new Dictionary<(int row, int col), int>();
    for(var row = minRow; row <= maxRow; ++row )
    {
      for(var col = minCol; col <= maxCol; ++col)
      {
        if(!corners.ContainsKey((row, col)))
        {
          var a = _coordinates.Contains((row-1, col-1));
          var b = _coordinates.Contains((row-1, col));
          var c = _coordinates.Contains((row, col-1));
          var d = _coordinates.Contains((row, col));
          if (a != b && a == d && b == c)
          {
            corners.Add((row, col), 2);
          }
          else if ((a != b && c == d) || (a == b && c != d))
          {
            corners.Add((row, col), 1);
          }
          else
          {
            corners.Add((row, col), 0);
          }
        }
      }
    }

    return corners.Sum( vkp => vkp.Value );
  }

  public static IEnumerable<Region> Scan(char[][] map)
  {
    var regionMap = map.Select( row => row.Select( _ => default(Region) ).ToArray() ).ToArray();

    var height = map.Length;
    var width = map[0].Length;

    for(var row = 0; row < height; ++row)
    {
      for(var col = 0; col < width; ++col)
      {
        var plant = map[row][col];
        var perimeter = 4;
        var region = default(Region);
        if(col > 0 && map[row][col-1] == plant) 
        {
          region = regionMap[row][col-1];
          --perimeter;
        }
        if(row > 0 && map[row-1][col] == plant) 
        {
          --perimeter;
          //take over region from one row above
          var rootRegion = regionMap[row-1][col]!.GetRootRegion();
          if(region == null)
          {
            region = rootRegion;
          }
          else if(region != rootRegion)
          {
            // join the regions together
            region.Join(rootRegion);
            region = rootRegion;
          }
        }

        if(row < height - 1 && map[row+1][col] == plant) 
        {
          --perimeter;
        }
        if(col < width - 1 && map[row][col+1] == plant) 
        {
          --perimeter;
        }

        if(region == null)
        {
          region = new Region(plant, perimeter);
        }
        else
        {
          region.Perimeter += perimeter;
        }

        region.Add(row, col);
        regionMap[row][col] = region;
      }
    }
    return regionMap
      .SelectMany( r => r )
      .Select( r => r!.GetRootRegion() )
      .Distinct();
  }
}