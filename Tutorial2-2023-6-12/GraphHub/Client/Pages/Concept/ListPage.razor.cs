using System;
using GraphHub.Client;
using GraphHub.Client.Pages;
using GraphHub.Client.Shared;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;

namespace GraphHub.Pages.Concept
{
    public class ListBase : GraphComponentBase
    {
        [Parameter]
        public string? listId { get; set; } = default!;

        public ListPageDTO _list { get; set; }

        public bool loaded = false;

        public int activeIndex = 0;

        public GraphDatabase Graph { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(listId))
            {
                _list = await GraphDatabaseService.GetList(GraphName, listId);
                activeIndex = 0;
                loaded = true;
            }
        }

        public async Task LoadAllListItems()
        {
            var items = await GraphDatabaseService.GetListMembers(GraphName, listId, _list.ListConcepts.Count());
            _list.ListConcepts.AddRange(items);
        }
    }
}
