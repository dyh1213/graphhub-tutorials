using System;
using GraphHub.Shared;

namespace GraphHub.Shared
{
	public class ConceptPageDTO
	{
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Markdown { get; set; }
        //Not saved
        public List<ChildListDTO>? OwnerOfLists { get; set; }
        public List<ListDTO>? MemberOfLists { get; set; }
    }

    public class ChildListDTO
    {
        public string? Id { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
        public int? NumberOfMembersIncludingInheritence { get; set; }
    }

    public class MinimalConcept
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
    }
}