
var input = File.ReadAllLines("../../../input.txt");

var (row, col) = FindStartingPosition(input);
var guard = new Guard(input, row, col);
// step 1: run through the lab
guard.Run();
// step 2: try to place the additional obstacle at every single field of the trace (but the starting point!)
// and check if it leads into a loop (kind of brute-force, but optimized to place obstacles only on visited fields)
int numberOfLoopsDetected = 0;
foreach(var (r, c) in guard.Visited.Skip(1))
{
  // store the line to restore it later
  var line = input[r];
  var chars = line.ToCharArray();
  chars[c] = '#';
  input[r] = new string(chars);

  var guard2 = new Guard(input, row, col);
  if(guard2.Run(silent: true))
  {
    ++numberOfLoopsDetected;
  }

  // restore original line;
  input[r] = line;
}
Console.WriteLine($"{numberOfLoopsDetected} loops detected.");


(int row, int col) FindStartingPosition(string[] input)
{
    for(var r = 0; r < input.Length; ++r)
    {
      for(var c = 0; c < input[r].Length; ++c)
      {
        if(input[r][c] == '^')
          return (r, c);
      }
    }
    throw new("Starting Position not found.");
}

public class Guard
{
  readonly string[] _lab;
  int _row;
  int _col;
  Direction _dir = Direction.Up;
  readonly HashSet<(int row, int col, Direction dir)> _trace = new();
  public IEnumerable<(int row, int col)> Visited => _trace.Select( x => (x.row, x.col) ).Distinct();

  public Guard(string[] lab, int row, int col)
  {
      _lab = lab;
      _row = row;
      _col = col;
  }

  public void StepForward()
  {
    switch (_dir)
    {
      case Direction.Up:
        --_row;
        break;
      case Direction.Down:
        ++_row;
        break;
      case Direction.Left:
        --_col;
        break;
      case Direction.Right:
        ++_col;
        break;
    }
  }

  public void StepBack()
  {
    switch (_dir)
    {
      case Direction.Up:
        ++_row;
        break;
      case Direction.Down:
        --_row;
        break;
      case Direction.Left:
        ++_col;
        break;
      case Direction.Right:
        --_col;
        break;
    }
  }

  public void TurnRight()
  {
    switch(_dir)
    {
      case Direction.Up:
        _dir = Direction.Right;
        break;
      case Direction.Down:
        _dir = Direction.Left;
        break;
      case Direction.Left:
        _dir = Direction.Up;
        break;
      case Direction.Right:
        _dir = Direction.Down;
        break;
    }
  }

  public bool IsPositionInLab()
    => _row >= 0 && _row < _lab.Length && _col >= 0 && _col < _lab[0].Length;

  public bool IsObstacleAtCurrentPosition()
    => IsPositionInLab() && _lab[_row][_col] == '#';


  //optimization for step 2: return true if a loop was detected, false if the guard left the lab
  public bool Run(bool silent = false)
  {
    var loopDeteced = false;
    while(true)
    {
      StepForward();
      if(IsObstacleAtCurrentPosition())
      {
        StepBack();
        TurnRight();
      }
      if(_trace.Contains((_row, _col, _dir)))
      {
        loopDeteced = true;
        if(!silent) 
        {
          Console.WriteLine($"loop detected at {_row}, {_col} in direction {_dir} after {_trace.Count} steps.");
        }
        break;
      }
      if(!IsPositionInLab())
      {
        if(!silent)
        {
          Console.WriteLine($"left lab at {_row}, {_col} after {_trace.Count} steps.");
        }
        break;
      }
      _trace.Add((_row, _col, _dir));
    }
    if(!silent)
    {
      Console.WriteLine($"visited {_trace.GroupBy( x => (x.row, x.col) ).Count()} distinct positions.");
    }
    return loopDeteced;
  }  
}

public enum Direction : byte
{
  Up,
  Down,
  Left,
  Right
}