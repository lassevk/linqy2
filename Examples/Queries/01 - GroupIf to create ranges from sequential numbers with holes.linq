<Query Kind="Program">
  <Reference Relative="..\..\Linqy\bin\Debug\Linqy.dll">C:\dev\vs.net\linqy\Linqy\bin\Debug\Linqy.dll</Reference>
  <Namespace>Linqy</Namespace>
</Query>

void Main()
{
    var input = new[] { 1, 2, 3, 5, 6, 8, 10, 11, 12, 14, 15, 16, 17 };
    var ranges = input
        .GroupIf((previous, current) => current == previous + 1)
        .Select(g => Tuple.Create(g.Min(), g.Max()));

    string.Join(", ", ranges.Select(range => $"{range.Item1}..{range.Item2}")).Dump();
    Util.HorizontalRun(true, input, ranges).Dump();
}