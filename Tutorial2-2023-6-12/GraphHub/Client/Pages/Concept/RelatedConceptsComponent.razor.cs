using System;
using GraphHub.Client.Pages;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;

namespace GraphHub.Pages.Concept
{
    public class RelatedConceptComponentBase : GraphComponentBase
    {
        public List<MinimalList>? graphSummary { get; set; }

        public bool loaded = false;

        protected override async Task OnParametersSetAsync()
        {
            if (graphSummary == null)
            {
                graphSummary = await GraphDatabaseService.GetGraphSummary(GraphName);
            }

            loaded = true;
        }
    }
}