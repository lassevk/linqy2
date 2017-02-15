<Query Kind="Program">
  <Reference Relative="..\..\Linqy\bin\Debug\Linqy.dll">C:\dev\vs.net\linqy\Linqy\bin\Debug\Linqy.dll</Reference>
  <Namespace>Linqy</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    var input = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.MonthNames;
    var distinctByFirstLetter = input
        .Where(name => !string.IsNullOrWhiteSpace(name))
        .DistinctBy(name => name[0]);
        
    distinctByFirstLetter.Dump();
}