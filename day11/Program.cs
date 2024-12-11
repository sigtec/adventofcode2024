using System.Data;
using System.Runtime.CompilerServices;

var stones = File.ReadAllText("../../../input.txt")
      .Trim()
      .Split(' ')
      .Select( s => long.Parse(s))
      .ToList();

var part1 = stones.Select( s => GetNumberOfStonesInStep(s, 25) ).Sum();
Console.WriteLine($"part1: number of stones after blinking 25 times: {part1}");

var cache = new Dictionary<(long, int), long>();
var part2 = stones.Select( s => GetNumberOfStonesInStepWithCache(s, 75, cache) ).Sum();
Console.WriteLine($"part2: number of stones after blinking 75 times: {part2}");

//recursion call; divide & conquer
long GetNumberOfStonesInStep(long value, int step)
{
  // no more steps left?
  if(step == 0) return 1L;
  // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
  if(value == 0L) return GetNumberOfStonesInStep(1L, step-1);
  // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. 
  // The left half of the digits are engraved on the new left stone, 
  // and the right half of the digits are engraved on the new right stone. 
  // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
  var s = value.ToString();
  if(s.Length % 2 == 0)
  {
    var i = s.Length / 2;
    var newStone1 = long.Parse(s.Substring(0, i));
    var newStone2 = long.Parse(s.Substring(i));
    return GetNumberOfStonesInStep(newStone1, step-1) + GetNumberOfStonesInStep(newStone2, step-1);
  }
  // If none of the other rules apply, the stone is replaced by a new stone; 
  // the old stone's number multiplied by 2024 is engraved on the new stone.
  return GetNumberOfStonesInStep(2024L * value, step-1);
}

// same with caching results for re-using them
long GetNumberOfStonesInStepWithCache(long value, int step, Dictionary<(long, int), long> cache)
{
  if(cache.ContainsKey((value, step))) return cache[(value, step)];
  
  // no more steps left?
  if(step == 0) return 1L;

  var result = default(long);
  // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
  if(value == 0L) 
  {
    result = GetNumberOfStonesInStepWithCache(1L, step-1, cache);
    cache.Add((value, step), result);
    return result;
  }
  // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
  var s = value.ToString();
  if(s.Length % 2 == 0)
  {
    var i = s.Length / 2;
    var newStone1 = long.Parse(s.Substring(0, i));
    var newStone2 = long.Parse(s.Substring(i));
    result = GetNumberOfStonesInStepWithCache(newStone1, step-1, cache) + GetNumberOfStonesInStepWithCache(newStone2, step-1, cache);
    cache.Add((value, step), result);
    return result;
  }
  // If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
  result = GetNumberOfStonesInStepWithCache(2024L * value, step-1, cache);
  cache.Add((value, step), result);
  return result;
  
}