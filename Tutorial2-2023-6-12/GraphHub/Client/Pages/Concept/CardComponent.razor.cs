using System;
using GraphHub.Client.Pages;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;

namespace GraphHub.Pages.Concept
{
    public class CardComponentBase : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int ItemCount { get; set; }

        [Parameter]
        public ItemTypeEnum ItemType { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        string color => (ItemCount > 0) ? "--mud-palette-action-default" : "--mud-palette-action-disabled";
        public string colorStyle => $"color: var({color})";

        public string containerCSS => (IsSelected) ?  "mud-width-full pa-8 hover-lighten purple-bottom-border" : "mud-width-full pa-8 hover-lighten";

        public string ItemCountDisplayString()
        {
            var returnString = "";

            if (ItemCount == 1)
            {
                returnString += (ItemType  == ItemTypeEnum.Concept) ? "Concept" : "List";
            }
            else
            {
                returnString += (ItemType == ItemTypeEnum.Concept) ? "Concepts" : "Lists";
            }

            return returnString;
        }
    }
}