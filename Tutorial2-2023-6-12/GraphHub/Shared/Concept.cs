using System;
namespace GraphHub.Shared
{
	public class Concept
	{
		public string? Id { get; set; }
        public string? Title { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
        public string? Markdown { get; set; }

        //Not saved
        public List<ConceptList>? OwnerOfLists { get; set; }
        public List<ConceptList>? MemberOfLists { get; set; }
    }

    public class ConceptData
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? PluralTitle { get; set; }
        public string? Description { get; set; }
    }
}

