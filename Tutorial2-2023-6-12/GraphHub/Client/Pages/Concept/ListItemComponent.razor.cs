using System;
using GraphHub.Client.Pages;
using GraphHub.Shared;
using Microsoft.AspNetCore.Components;

namespace GraphHub.Pages.Concept
{
    public class ListItemComponentBase : GraphComponentBase
    {
        [Parameter] public string ItemTitle { get; set; }
        [Parameter] public string ItemId { get; set; }
        [Parameter] public string ItemDescription { get; set; }
        [Parameter] public ItemTypeEnum ItemType { get; set; }

        [Parameter] public string? TokenItemTitle { get; set; }
        [Parameter] public string? TokenItemId { get; set; }

        [Parameter] public int? ItemCount { get; set; }

        public string ItemPath => (ItemType == ItemTypeEnum.Concept) ? getConceptPath(ItemId) : getListPath(ItemId);

        public void OnItemCilck()
        {
            if (ItemType == ItemTypeEnum.Concept)
            {
                goToConcept(ItemId);
            }
            else
            {
                goToList(ItemId);
            }
        }

        public void OnTokenCilck()
        {
            if (ItemType == ItemTypeEnum.Concept)
            {
                goToList(TokenItemId);
            }
            else
            {
                goToConcept(TokenItemId);
            }
        }

    }
}