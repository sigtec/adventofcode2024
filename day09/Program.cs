// indicates that the block is free (instead of a fileId)
const int FREE_BLOCK = -1;
// returned as an offset if no offset is available.
const int NOT_FOUND = -1;

var input = File.ReadAllText("../../../input.txt")
  .Where(c => char.IsDigit(c))
  .Select(c => byte.Parse(c.ToString()))
  .ToArray();

// each block refers to the file id - or to null, if it's free
var blocks = Init(input);

// begin fragmenting
var firstFreeBlock = FindNextFreeBlock(blocks, 0);
var lastUsedBlock = FindLastUsedBlock(blocks, blocks.Length - 1);

while (firstFreeBlock < lastUsedBlock)
{
    // swap block for file
    blocks[firstFreeBlock] = blocks[lastUsedBlock];
    blocks[lastUsedBlock] = FREE_BLOCK;

    // find next blocks
    firstFreeBlock = FindNextFreeBlock(blocks, firstFreeBlock);
    lastUsedBlock = FindLastUsedBlock(blocks, lastUsedBlock);
}

var part1Checksum = Enumerable.Range(0, blocks.Length).Sum(i => (blocks[i] == FREE_BLOCK) ? 0L : i * blocks[i]);
Console.WriteLine($"part 1 checksum: {part1Checksum}");

// part 2

//restore original blocks
blocks = Init(input);

var offset = FindPreviousFileOffset(blocks, blocks.Length-1);
while(offset > 0)
{
    var fileSize = GetSize(blocks, offset);

    // find first free block of at least this size
    var freeBlock = FindNextFreeBlock(blocks, 0);
    while(freeBlock != NOT_FOUND && freeBlock < offset)
    {
      if(GetSize(blocks, freeBlock) >= fileSize)
      {
        // move file
        Array.Copy(blocks, offset, blocks, freeBlock, fileSize);
        // free old postion
        for(int j = 0; j < fileSize; ++j )
        {
          blocks[offset + j] = FREE_BLOCK;
        }
        break;
      }
      freeBlock = FindNextFreeBlock(blocks, freeBlock+1);
    }
    // next file
    offset = FindPreviousFileOffset(blocks, offset-1);
}

var part2Checksum = Enumerable.Range(0, blocks.Length).Sum(i => (blocks[i] == FREE_BLOCK) ? 0L : i * blocks[i]);
Console.WriteLine($"part 2 checksum: {part2Checksum}");




int[] Init(byte[] input)
{
  var diskSize = input.Sum(size => size);
  var blocks = new int[diskSize];

  //init the blocks from the input
  var isFile = true;
  var fileId = 0;
  var i = 0;
  foreach (var size in input)
  {
      for (var j = 0; j < size; ++j)
      {
          blocks[i++] = isFile ? fileId : FREE_BLOCK;
      }
      if (isFile)
      {
          ++fileId;
      }
      isFile = !isFile;
  }
  if (i != diskSize)
  {
      throw new("block counter does not run to the end.");
  }

  return blocks;
}

int FindNextFreeBlock(int[] blocks, int i)
{
  if(i < 0 || i >= blocks.Length) 
  {
    return NOT_FOUND;
  }
  while (blocks[i] != FREE_BLOCK)
  {
      ++i;
      if(i >= blocks.Length)
      {
        return NOT_FOUND;
      }
  }
  return i;
}

int FindLastUsedBlock(int[] blocks, int i)
{
  while (blocks[i] == FREE_BLOCK)
  {
      --i;
  }
  return i;
}

int FindPreviousFileOffset(int[] blocks, int i)
{
  i = FindLastUsedBlock(blocks, i);
  var fileId = blocks[i];
  do
  {
    --i;
  } while(i >= 0 && blocks[i] == fileId);
  return i+1;
}

int GetSize(int[] blocks, int start)
{
  var i = start;
  var fileId = blocks[i];
  do
  {
    ++i;
  } while(i < blocks.Length && blocks[i] == fileId);
  return i - start;
}

