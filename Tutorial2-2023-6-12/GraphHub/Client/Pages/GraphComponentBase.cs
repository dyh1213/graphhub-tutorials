using System;
using GraphHub.Client.Shared;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GraphHub.Client.Pages
{
    public class GraphComponentBase : ComponentBase
    {
        [Inject]
        public GraphDatabaseService GraphDatabaseService { get; set; }

        [Inject]
        public NavigationManager _nav { get; set; }

        [Parameter]
        public string GraphName { get; set; }

        public string getListPath(string? listId) => (listId != null) ? $"{GraphName}/list/{listId}" : "NULL";
        public string getConceptPath(string? conceptId) => (conceptId != null) ? $"{GraphName}/concept/{conceptId}" : "NULL";

        public void goToList(string? listId) => _nav.NavigateTo(getListPath(listId));
        public void goToConcept(string? conceptId) => _nav.NavigateTo(getConceptPath(conceptId));
    }
}

