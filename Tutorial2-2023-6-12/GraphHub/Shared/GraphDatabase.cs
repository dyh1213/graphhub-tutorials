using System;
using GraphHub.Shared;
using GraphHub.Shared.Utilities;

namespace GraphHub.Shared
{
	public class GraphDatabase
	{
        public Dictionary<string, Concept>? ConceptsDictionary { get; set; }
        public Dictionary<string, ConceptList>? ListsDictionary { get; set; }

        public GraphDatabase(List<ConceptData> ConceptsData, List<ConceptListData> ListsData, List<MembershipData> membershipData, List<ConceptMarkdown> conceptMarkdown)
		{
            LoadGraph(ConceptsData, ListsData, membershipData, conceptMarkdown);

            if (ConceptsDictionary.ContainsKey("1c1b4890-7e8a-4f1f-a7e3-5b71e8e303d0"))
            {
                var item = ConceptsDictionary["1c1b4890-7e8a-4f1f-a7e3-5b71e8e303d0"];
                var results = TaxonomyPrinter.BuildMarkdownTSLA(item);
                int tasdas = 5;
            }

        }

        public void LoadGraph(List<ConceptData> ConceptsData, List<ConceptListData> ListsData, List<MembershipData> membershipData, List<ConceptMarkdown> conceptMarkdown)
        {

            var ConceptsDataDictionary = ConceptsData.ToDictionary(x => x.Id, x => x);
            var ListsDataDictionary = ListsData.ToDictionary(x => x.Id, x => x);
            var MembershipsDictionary = membershipData.GroupBy(x=>x.ListId).ToDictionary(x => x.Key, x => x.Select(x=> x.ConceptId).ToList());
            var MarkdownDictionary = conceptMarkdown?.ToDictionary(x => x.ConceptId, x => x) ?? new Dictionary<string?, ConceptMarkdown>();

            try
            {
                var conceptsDictionary = ConceptsDataDictionary.ToDictionary(kvp => kvp.Key, kvp => new Concept
                {
                    Id = kvp.Value.Id,
                    Title = kvp.Value.Title,
                    PluralTitle = kvp.Value.PluralTitle,
                    Description = kvp.Value.Description,
                    Markdown = MarkdownDictionary.ContainsKey(kvp.Key) ? MarkdownDictionary[kvp.Key].Markdown : null
                });

                var listsDictionary = ListsDataDictionary.ToDictionary(kvp => kvp.Key, kvp => new ConceptList
                {
                    Id = kvp.Value.Id,
                    Title = kvp.Value.Title,
                    PluralTitle = kvp.Value.PluralTitle,
                    Description = kvp.Value.Description,
                });

                foreach (var list in listsDictionary.Values)
                {
                    list.ParentConcept = conceptsDictionary[ListsDataDictionary[list.Id].ParentConceptId];
                    list.PullFromLists = ListsDataDictionary[list.Id].PullFromListsIds?.Select(id => listsDictionary[id]).ToList() ?? new List<ConceptList>();
                    list.Concepts = (MembershipsDictionary.ContainsKey(list.Id)) ? MembershipsDictionary[list.Id]?.Select(id => conceptsDictionary[id]).ToList() ?? new List<Concept>() : new List<Concept>();
                }

                // Inverting the resolution of PushToLists
                foreach (var list in listsDictionary.Values)
                {
                    if (list.PullFromLists == null) continue;
                    foreach (var pullFromList in list.PullFromLists)
                    {
                        if (pullFromList.PushToLists == null)
                        {
                            pullFromList.PushToLists = new List<ConceptList>();
                        }
                        pullFromList.PushToLists.Add(list);
                    }
                }

                foreach (var concept in conceptsDictionary.Values)
                {
                    concept.OwnerOfLists = listsDictionary.Values.Where(list => list.ParentConcept?.Id == concept.Id).ToList();
                    concept.MemberOfLists = listsDictionary.Values.Where(list => list.Concepts?.Any(x => x.Id?.Equals(concept.Id) ?? false) ?? false).ToList();
                }

                ConceptsDictionary = conceptsDictionary;
                ListsDictionary = listsDictionary;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

