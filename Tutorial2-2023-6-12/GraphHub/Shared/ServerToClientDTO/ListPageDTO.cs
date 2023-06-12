using System;
using GraphHub.Shared;

namespace GraphHub.Shared
{
	public class ListPageDTO
	{
        public string? Id { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
        public string? ParentConceptName { get; set; }
        public string? ParentConceptId { get; set; }
        public int? NumberOfMembersIncludingInheritence { get; set; }

        public List<ListConceptDTO>? ListConcepts { get; set; }
        public List<ListDTO>? PullFromLists { get; set; }
        public List<ListDTO>? PushToLists { get; set; }
    }

    public class ListDTO
    {
        public string? Id { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
        public string? ParentConceptName { get; set; }
        public string? ParentConceptId { get; set; }
        public int? NumberOfMembersIncludingInheritence { get; set; }
    }

    public class ListConceptDTO
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? InheritedFromListId { get; set; }
        public string? InheritedFromListName { get; set; }
    }

    public class MinimalList
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public List<MinimalConcept>? Concepts { get; set; }
    }
}

