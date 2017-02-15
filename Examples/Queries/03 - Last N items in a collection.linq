<Query Kind="Program">
  <Reference Relative="..\..\Linqy\bin\Debug\Linqy.dll">C:\dev\vs.net\linqy\Linqy\bin\Debug\Linqy.dll</Reference>
  <Namespace>Linqy</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    var bigCollection = Enumerable.Range(0, 100000);
    var lastTwenty = bigCollection.Last(20);
    
    lastTwenty.Dump();
}