using System;
namespace GraphHub.Shared
{
    public class ConceptList
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }

        //Not saved
        public Concept? ParentConcept { get; set; }
        public List<Concept>? Concepts { get; set; }
        public List<ConceptList>? PullFromLists { get; set; }
        public List<ConceptList>? PushToLists { get; set; }

        public List<Tuple<Concept, ConceptList>> ConceptsIncludingInherited(HashSet<string> visitedLists = null, bool isInitialCall = true)
        {
            visitedLists ??= new HashSet<string>();

            // Return empty list if the current list has been visited before.
            if (!string.IsNullOrEmpty(Id) && visitedLists.Contains(Id))
                return new List<Tuple<Concept, ConceptList>>();

            // Add the current list to the visited ones.
            if (!string.IsNullOrEmpty(Id))
                visitedLists.Add(Id);

            var result = new List<Tuple<Concept, ConceptList>>();

            // Recursively gather the concepts from the lists this list pulls from.
            if (PullFromLists != null)
            {
                foreach (var list in PullFromLists)
                {
                    var inheritedConcepts = list.ConceptsIncludingInherited(visitedLists, false);
                    result.AddRange(inheritedConcepts);
                }
            }

            // Gather the concepts from the current list.
            if (Concepts != null)
            {
                foreach (var concept in Concepts)
                {
                    // Mark them with null in the tuple only if this is the initial call, 
                    // otherwise associate them with the current list.
                    result.Add(Tuple.Create(concept, isInitialCall ? null : this));
                }
            }

            // Remove duplicates. This is based on the assumption that the combination of Concept Id and ConceptList Id is unique.
            return result.GroupBy(x => new { ConceptId = x.Item1.Id, ConceptListId = x.Item2?.Id })
                         .Select(g => g.First())
                         .ToList();
        }

        public int ConceptsCountIncInherited(HashSet<string> visitedLists = null, HashSet<string> uniqueConcepts = null, bool isInitialCall = true)
        {
            visitedLists ??= new HashSet<string>();
            uniqueConcepts ??= new HashSet<string>();

            // Return 0 if the current list has been visited before.
            if (!string.IsNullOrEmpty(Id) && visitedLists.Contains(Id))
                return 0;

            // Add the current list to the visited ones.
            if (!string.IsNullOrEmpty(Id))
                visitedLists.Add(Id);

            // Recursively gather the concepts from the lists this list pulls from.
            if (PullFromLists != null)
            {
                foreach (var list in PullFromLists)
                {
                    list.ConceptsCountIncInherited(visitedLists, uniqueConcepts, false);
                }
            }

            // Gather the concepts from the current list.
            if (Concepts != null)
            {
                foreach (var concept in Concepts)
                {
                    uniqueConcepts.Add(concept.Id);
                }
            }

            // Return the count of unique concepts. 
            return isInitialCall ? uniqueConcepts.Count : 0;
        }
    }

    public class ConceptListData
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
        public string? ParentConceptId { get; set; }
        public List<string>? PullFromListsIds { get; set; }
        public bool DisableDirectMembers { get; set; }
    }
}
