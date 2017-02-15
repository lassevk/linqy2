<Query Kind="Program">
  <Reference Relative="..\..\Linqy\bin\Debug\Linqy.dll">C:\Dev\VS.NET\Linqy\Linqy\bin\Debug\Linqy.dll</Reference>
  <Namespace>Linqy</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    var collection = new[]
    {
        new Item { Key = "A", Value = 10 },
        new Item { Key = "A", Value = 25 },
        new Item { Key = "A", Value = 7 },
        new Item { Key = "B", Value = 17 },
        new Item { Key = "B", Value = 2 },
    };
    
    var aggregatedValues = collection
        .AggregateIf(
        
            // Should we aggregate into the same group as the previous item
            (previous, current) => previous.Key == current.Key,
            
            // How do we start aggregating for new groups of items
            () => new List<Item>(),
            
            // Aggregate by adding to the list
            (list, item) => { list.Add(item); return list; },
            
            // Create final result by calculating average
            list => Tuple.Create(list[0].Key, list.Average(item => item.Value + 0.0)));
            
    aggregatedValues.Dump();
}

public class Item
{
    public string Key;
    public int Value;
}