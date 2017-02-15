<Query Kind="Program">
  <Reference Relative="..\..\Linqy\bin\Debug\Linqy.dll">C:\Dev\VS.NET\Linqy\Linqy\bin\Debug\Linqy.dll</Reference>
  <Namespace>Linqy</Namespace>
</Query>

void Main()
{
    var input = Enumerable.Range(1, 10);
    var pairwiseSum = input.Lag(1).Skip(1).Select(pair => pair.Element + pair.LaggingElement);

    Util.HorizontalRun(true, input, pairwiseSum).Dump();
}
