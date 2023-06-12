using System;
namespace GraphHub.Shared
{
    public class GraphData
    {
        public List<ConceptData> Concepts { get; set; }
        public List<ConceptListData> Lists { get; set; }
        public List<MembershipData> Memberships { get; set; }
        public List<ConceptMarkdown> ConceptMarkdown { get; set; }
    }
}

