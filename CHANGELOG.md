# Release Notes #

## Version 1.0.2.1 - March 17th 2016 ##

* Fixed incorrect documentation for SelectorEqualityComparer

## Version 1.0.2 - March 17th 2016 ##

**Added new extension methods**

* AllWithMax
* AllWithMin

These two methods extract a value from each element and returns a collection containing all the elements that has the same extracted value as the maximum extracted value.

## Version 1.0.1 - March 4th 2016 ##

**Added new extension methods**

* Partition (into separate lists with at most N items)
* Append (concat with single or multiple items, basically just syntactic sugar)
* Except (single item, also just syntactic sugar)
* NonNull (all non-null elements of a collection)
* NonEmpty (all non-null and non-empty strings in a collection)
* NonEmptyOrWhiteSpace (all non-null, non-empty, and non-whitespace-only strings in a collection)

**Added new classes**

* SelectorEqualityComparer (IEqualityComparer that extracts a value from both elements and compares using this value instead of the actual elements)

**Fixes**

* Fixed some incorrect documentation, and replaced all mentions of the word "item" with "element"

Also increased code coverage by writing more tests

## Version 1.0.0 ##

Initial internal release, implementing the following extension methods:

* Lag / Lead (combine one element with either a previous or future element from the same collection)
* AddIndex (return a new collection that has both an index and the element)
* GroupIf (continue adding to the current group as long as a predicate says so)
* AggregateIf (continue aggregating into the current value as long as a predicate says so)
* First(N) (return the first N items, similar to Take, more for completeness in the API)
* Last(N) (return the last N items)
* ExceptFirst(N) (return the whole collection except for the first N items, similar to Skip, more for completeness in the API)
* ExceptLast(N) (return the whole collection except for the last N items)