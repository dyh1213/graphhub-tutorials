

using System;
using GraphHub.Server;
using GraphHub.Shared;
using Markdig;

namespace Pandah.Server.GraphQL
{
    public class DataQueryResolver
    {
        public TempDatabase database { get; set; }

        public DataQueryResolver()
        {
            database = TempDatabase.Instance;
        }

        public async Task<ConceptPageDTO> GetConcept(string GraphName, string ConceptId)
        {
            var concept = database.FileGraphDatabase[GraphName].ConceptsDictionary[ConceptId];

            var dto = new ConceptPageDTO
            {
                Id = concept.Id,
                Title = concept.Title,
                Description = concept.Description,
                Markdown = concept.Markdown
            };

            if (concept.OwnerOfLists != null)
            {
                dto.OwnerOfLists = new List<ChildListDTO>();
                foreach (var list in concept.OwnerOfLists)
                {
                    dto.OwnerOfLists.Add(new ChildListDTO
                    {
                        Id = list.Id,
                        PluralTitle = list.PluralTitle,
                        Description = list.Description,
                        NumberOfMembersIncludingInheritence = list.ConceptsCountIncInherited()
                    }); ;
                }
            }

            if (concept.MemberOfLists != null)
            {
                dto.MemberOfLists = new List<ListDTO>();
                foreach (var list in concept.MemberOfLists)
                {
                    dto.MemberOfLists.Add(new ListDTO
                    {
                        Id = list.Id,
                        PluralTitle = list.PluralTitle,
                        Description = list.Description,
                        ParentConceptName = list.ParentConcept?.Title,
                        ParentConceptId = list.ParentConcept?.Id,
                        NumberOfMembersIncludingInheritence = list.ConceptsCountIncInherited()
                    });
                }
            }

            return dto;
        }

        public async Task<ListPageDTO> GetList(string GraphName, string ListId)
        {
            ConceptList conceptList = database.FileGraphDatabase[GraphName].ListsDictionary[ListId];

            List<ListDTO> pullFromListDTOs = conceptList.PullFromLists?.Select(l => new ListDTO
            {
                Id = l.Id,
                PluralTitle = l.PluralTitle,
                Description = l.Description,
                ParentConceptName = l.ParentConcept?.Title,
                ParentConceptId = l.ParentConcept?.Id,
                NumberOfMembersIncludingInheritence = l.ConceptsCountIncInherited()
            }).ToList();

            List<ListDTO> pushToListDTOs = conceptList.PushToLists?.Select(l => new ListDTO
            {
                Id = l.Id,
                PluralTitle = l.PluralTitle,
                Description = l.Description,
                ParentConceptName = l.ParentConcept?.Title,
                ParentConceptId = l.ParentConcept?.Id,
                NumberOfMembersIncludingInheritence = l.ConceptsCountIncInherited()
            }).ToList();

            var items = conceptList.ConceptsIncludingInherited();
            List<ListConceptDTO> listConcepts = items.Select(l => new ListConceptDTO
            {
                Id = l.Item1.Id,
                Title = l.Item1.Title,
                Description = l.Item1.Description,
                InheritedFromListId = l.Item2?.Id,
                InheritedFromListName = l.Item2?.PluralTitle,
            }).Take(25).ToList();

            ListPageDTO conceptPageDTO = new ListPageDTO
            {
                Id = conceptList.Id,
                PluralTitle = conceptList.PluralTitle,
                Description = conceptList.Description,
                ParentConceptName = conceptList.ParentConcept?.Title,
                ParentConceptId = conceptList.ParentConcept?.Id,
                NumberOfMembersIncludingInheritence = conceptList.ConceptsCountIncInherited(),
                PullFromLists = pullFromListDTOs,
                PushToLists = pushToListDTOs,
                ListConcepts = listConcepts
            };

            return conceptPageDTO;
        }

        public async Task<List<ListConceptDTO>> GetAllListMembers(string GraphName, string ListId, int Skip = 0, int take = int.MaxValue)
        {
            ConceptList conceptList = database.FileGraphDatabase[GraphName].ListsDictionary[ListId];
            var items = conceptList.ConceptsIncludingInherited();
            List<ListConceptDTO> listConcepts = items.Select(l => new ListConceptDTO
            {
                Id = l.Item1.Id,
                Title = l.Item1.Title,
                Description = l.Item1.Description,
                InheritedFromListId = l.Item2?.Id,
                InheritedFromListName = l.Item2?.PluralTitle,
            }).Skip(Skip).Take(take).ToList();
            return listConcepts;
        }

        public async Task<List<MinimalList>> GetRelatedConcepts(string GraphName)
        {
            int countOfItems = 0;
            List<MinimalList> returnitems = new List<MinimalList>();

            GraphDatabase graphDatabase = database.FileGraphDatabase[GraphName];
            var items = graphDatabase.ConceptsDictionary.Values
                        .Where(x => x.MemberOfLists == null || x.MemberOfLists.Count == 0)
                        .Take(10)
                        .Select(x=> new MinimalConcept()
                        {
                            Id = x.Id,
                            Title = x.Title
                        })
                        .ToList();

            var itemWithNoParent = new MinimalList()
            {
                Id = "",
                Title = "",
                Concepts = items
            };

            returnitems.Add(itemWithNoParent);

            countOfItems += itemWithNoParent.Concepts.Count;

            foreach(var item in graphDatabase.ListsDictionary.Values)
            {
                if (countOfItems < 100)
                {
                    var newItem = new MinimalList()
                    {
                        Id = item.Id,
                        Title = item.PluralTitle,
                        Concepts = item.Concepts.Take(10).Select(x => new MinimalConcept()
                        {
                            Id = x.Id,
                            Title = x.Title
                        }).ToList()
                    };

                    returnitems.Add(newItem);

                    countOfItems += newItem.Concepts.Count;
                }
            }

            return returnitems;
        }

        public async Task<GraphData> GetDatabase(string ConceptName)
        {
            return null;
        }
    }
}
