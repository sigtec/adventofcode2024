using System.Xml.XPath;

var input = File.ReadAllLines("../../../input.txt")
  .Select( line => line.Split(':') )
  .Select( tokens => (expectedResult: long.Parse(tokens[0]), operands: tokens[1]
    .Trim()
    .Split(' ')
    .Select( s => long.Parse(s) )
    .ToArray()
  ) );

var totalCalibrationResult = input.Where( eq => TestIfEquationCanBeTrue(eq) ).Sum( eq => eq.expectedResult );
Console.WriteLine($"total calibration result: {totalCalibrationResult}");

bool TestIfEquationCanBeTrue((long expectedResult, long[] operands) equation)
{
  // hack: adding the first operand to zero does not change the start value,
  // but is a fine entry point into recursion
  foreach(var actualResult in ApplyOperation(0, equation.operands, 0, Operator.Add))
  {
    if(equation.expectedResult == actualResult)
      return true;
  }
  return false;
}

IEnumerable<long> ApplyOperation(long actual, long[] operands, int i, Operator op)
{
  var result = 0L;
  switch (op)
  {
    case Operator.Add:
      result = actual + operands[i];
      break;
    case Operator.Multiply:
      result = actual * operands[i];
      break;
    // comment this out to solve part 1
    case Operator.Concatenation:
      // dirty hack: concartenate as strings
      result = long.Parse($"{actual}{operands[i]}");
      break;
  }

  if (i == operands.Length - 1)
  {
    yield return result;
  }
  else
  {
    foreach(var op2 in Enum.GetValues<Operator>())
    {
      foreach(var next in ApplyOperation(result, operands, i+1, op2))
      {
        yield return next;
      }
    }
  }
}

public enum Operator
{
  Add,
  Multiply,
  // comment this out to solve part 1
  Concatenation
}


