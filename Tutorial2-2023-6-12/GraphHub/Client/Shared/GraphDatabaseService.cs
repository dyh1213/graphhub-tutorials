using System;
using GraphHub.Shared;
using System.Text.Json;
using Markdig;
using System.Text.RegularExpressions;

namespace GraphHub.Client.Shared
{
    public class GraphDatabaseService
    {
        private readonly GHGQLAPI _apiClient;
        private string? _currentGraph;
        private Dictionary<string, ConceptPageDTO> conceptsCache;
        private Dictionary<string, ListPageDTO> listsCache;
        private Dictionary<string, List<ListConceptDTO>> listMembersCache;    
        private List<MinimalList>? graphSummaryCache;

        public GraphDatabaseService(GHGQLAPI apiClient)
        {
            _apiClient = apiClient;
        }

        private void ResetIfNeeded(string graphName)
        {
            if (graphName != null && !graphName.Equals(_currentGraph))
            {
                ResetItems(graphName);
            }
        }

        private void ResetItems(string graphName)
        {
            _currentGraph = graphName;
            conceptsCache = new Dictionary<string, ConceptPageDTO>();
            listsCache = new Dictionary<string, ListPageDTO>();
            listMembersCache = new Dictionary<string, List<ListConceptDTO>>();
            graphSummaryCache = null;
        }

        public async Task<ConceptPageDTO> GetConcept(string graphName, string conceptId)
        {
            ResetIfNeeded(graphName);
            if (conceptsCache.ContainsKey(conceptId))
            {
                //Console.WriteLine("Chache hit for CONCEPT");
                return conceptsCache[conceptId];
            }

            var response = await _apiClient.GetConceptPage.ExecuteAsync(graphName, conceptId);
            var conceptData = response.Data.Concept;



            // Mapping from GraphQL Response to DTO
            var result = new ConceptPageDTO
            {
                Id = conceptData.Id,
                Title = conceptData.Title,
                Description = conceptData.Description,
                Markdown = ReplaceGuidWithListPath(conceptData.Markdown, graphName),
                // Convert the lists as well
                OwnerOfLists = conceptData.OwnerOfLists?.Select(o => new ChildListDTO
                {
                    Id = o.Id,
                    PluralTitle = o.PluralTitle,
                    Description = o.Description,
                    NumberOfMembersIncludingInheritence = o.NumberOfMembersIncludingInheritence
                }).ToList(),
                MemberOfLists = conceptData.MemberOfLists?.Select(m => new ListDTO
                {
                    Id = m.Id,
                    PluralTitle = m.PluralTitle,
                    Description = m.Description,
                    ParentConceptName = m.ParentConceptName,
                    ParentConceptId = m.ParentConceptId,
                    NumberOfMembersIncludingInheritence = m.NumberOfMembersIncludingInheritence
                }).ToList()
            };

            conceptsCache.Add(result.Id, result);

            return result;
        }

        public async Task<ListPageDTO> GetList(string graphName, string listId)
        {
            ResetIfNeeded(graphName);
            if (listsCache.ContainsKey(listId))
            {
                //Console.WriteLine("Chache hit for LIST");
                return listsCache[listId];
            }

            var response = await _apiClient.GetListPage.ExecuteAsync(graphName, listId);
            var list = response.Data.List;

            // Mapping from GraphQL Response to DTO
            var result = new ListPageDTO
            {
                Id = list.Id,
                PluralTitle = list.PluralTitle,
                Description = list.Description,
                ParentConceptName = list.ParentConceptName,
                ParentConceptId = list.ParentConceptId,
                NumberOfMembersIncludingInheritence = list.NumberOfMembersIncludingInheritence,
                // Convert the lists as well
                ListConcepts = list.ListConcepts?.Select(lc => new ListConceptDTO
                {
                    Id = lc.Id,
                    Title = lc.Title,
                    Description = lc.Description,
                    InheritedFromListId = lc.InheritedFromListId,
                    InheritedFromListName = lc.InheritedFromListName
                }).ToList(),
                PullFromLists = list.PullFromLists?.Select(p => new ListDTO
                {
                    Id = p.Id,
                    PluralTitle = p.PluralTitle,
                    Description = p.Description,
                    ParentConceptName = p.ParentConceptName,
                    ParentConceptId = p.ParentConceptId,
                    NumberOfMembersIncludingInheritence = p.NumberOfMembersIncludingInheritence
                }).ToList(),
                PushToLists = list.PushToLists?.Select(pl => new ListDTO
                {
                    Id = pl.Id,
                    PluralTitle = pl.PluralTitle,
                    Description = pl.Description,
                    ParentConceptName = pl.ParentConceptName,
                    ParentConceptId = pl.ParentConceptId,
                    NumberOfMembersIncludingInheritence = pl.NumberOfMembersIncludingInheritence
                }).ToList()
            };

            listsCache.Add(result.Id, result);

            return result;
        }

        public async Task<List<ListConceptDTO>> GetListMembers(string graphName, string listId, int skip = 0, int take = int.MaxValue)
        {
            ResetIfNeeded(graphName);
            if (listMembersCache.ContainsKey(listId))
            {
                var cachedItem = listMembersCache[listId];

                if (cachedItem.Count >= skip + take)
                {
                    //Console.WriteLine("Chache hit for LISTMEMBERS");
                    return cachedItem;
                }
                
            }
         

            var response = await _apiClient.GetAllListMembers.ExecuteAsync(graphName, listId, skip, take);
            var allListMembers = response.Data.AllListMembers;

            // Mapping from GraphQL Response to DTO
            var result = allListMembers.Select(lm => new ListConceptDTO
            {
                Id = lm.Id,
                Title = lm.Title,
                Description = lm.Description,
                InheritedFromListId = lm.InheritedFromListId,
                InheritedFromListName = lm.InheritedFromListName
            }).ToList();

            listMembersCache.Add(listId, result);

            return result;
        }

        public async Task<List<MinimalList>?> GetGraphSummary(string graphName)
        {
            ResetIfNeeded(graphName);
            if (graphSummaryCache != null)
            {
                //Console.WriteLine("Chache hit for GRAPHSUMMARY");
                return graphSummaryCache;
            }

            var response = await _apiClient.GetRelatedConcepts.ExecuteAsync(graphName);
            var allListMembers = response.Data?.RelatedConcepts?.ToList();

            // Mapping from GraphQL Response to DTO
            var graphSummary = allListMembers?.Select(lm => new MinimalList
            {
                Id = lm.Id,
                Title = lm.Title,
                Concepts = lm.Concepts.Select(x=> new MinimalConcept()
                {
                    Id = x.Id,
                    Title = x.Title,               
                }).ToList()
            }).ToList();

            graphSummaryCache = graphSummary;

            return graphSummary;
        }

        public static string ReplaceGuidWithListPath(string? markdown, string graphName)
        {
            if (markdown == null) return null;
            var regex = new Regex(@"\b[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}\b");
            var evaluator = new MatchEvaluator(match => $"{getListPath(graphName, match.Groups[0].Value)}");
            return regex.Replace(markdown, evaluator);
        }

        public static string getListPath(string graphName, string? listId) => (listId != null) ? $"{graphName}/list/{listId}" : "NULL";
    }
}