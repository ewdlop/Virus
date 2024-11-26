<Query Kind="Statements">
  <Namespace>System.Collections.Frozen</Namespace>
</Query>



//immutable dictionary
System.Collections.Frozen.FrozenDictionary<int, StudentName> frozenDictionary = Enumerable.Range(1, 10)
	.Select(static i => new StudentName($"First-{i}", LastName: $"Last-{i}", ID: i))
	.ToFrozenDictionary(x => x.ID, x => x);

//immutable dictionary
System.Collections.Frozen.FrozenDictionary<int, StudentName> frozenDictionary2 = Enumerable.Range(1, 10)
	.Select(static i => new StudentName($"First-{i}", LastName: $"Last-{i}", ID: i))
	.ToFrozenDictionary(x => x.ID, x => x);
	
//immutable dictionary

frozenDictionary.Equals(frozenDictionary2).Dump();
frozenDictionary.SequenceEqual(frozenDictionary2).Dump();


record StudentName(string FirstName, string LastName, int ID);