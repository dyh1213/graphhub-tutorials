using System;
using GraphHub.Client;
using GraphHub.Client.Pages;
using GraphHub.Client.Shared;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;

namespace GraphHub.Pages.Concept
{
    public class ConceptBase : GraphComponentBase
    {
        [Inject]
        public GraphDatabaseService GraphDatabaseService { get; set; }

        [Parameter]
        public string? conceptId { get; set; }

        public ConceptPageDTO _concept { get; set; }

        public bool loaded = false;

        public int activeIndex = 0;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(conceptId))
            {
                _concept = await GraphDatabaseService.GetConcept(GraphName, conceptId);
                if (_concept.Markdown == null)
                {
                    activeIndex = (_concept.OwnerOfLists != null && _concept.OwnerOfLists.Count() > 0) ? 1 : 2;
                }
                loaded = true;
            }
        }
    }
}