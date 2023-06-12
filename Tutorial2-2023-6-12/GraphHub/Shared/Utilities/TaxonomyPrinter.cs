using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GraphHub.Shared.Utilities
{
	public class TaxonomyPrinter
	{
        public static string BuildMarkdownMJ(Concept concept, int depth = 0)
        {
            if (concept == null || depth > 10)
            {
                return string.Empty;
            }

            StringBuilder markdown = new StringBuilder();
            if (concept.OwnerOfLists != null)
            {
                foreach (var conceptList in concept.OwnerOfLists)
                {
                    markdown.AppendLine(new string('-', depth * 2) + "* [" + conceptList.Title + $"]({conceptList.Id})");
                    if (conceptList.PullFromLists != null)
                    {
                        foreach (var sublist in conceptList.PullFromLists)
                        {
                            markdown.AppendLine(BuildListMarkdownMJ(sublist, depth + 1));
                        }
                    }
                }
            }

            return markdown.ToString();
        }

        public static string BuildListMarkdownMJ(ConceptList list, int depth)
        {
            if (list == null || depth > 10)
            {
                return string.Empty;
            }

            StringBuilder markdown = new StringBuilder();
            markdown.AppendLine(new string('-', depth * 2) + "* [" + list.Title + $"]({list.Id})");
            if (list.PullFromLists != null)
            {
                foreach (var sublist in list.PullFromLists)
                {
                    markdown.AppendLine(BuildListMarkdownMJ(sublist, depth + 1));
                }
            }

            return markdown.ToString();
        }

        public static string BuildMarkdownTSLA(Concept concept, int depth = 0)
        {
            if (concept == null || depth > 10)
            {
                return string.Empty;
            }

            StringBuilder markdown = new StringBuilder();

            markdown.AppendLine(new string('-', depth * 2) + "* [" + concept.Title + $"]({concept.Id})");

            if (concept.OwnerOfLists != null && concept.OwnerOfLists.Count() > 0)
            {
                foreach (var conceptList in concept.OwnerOfLists)
                {
                    markdown.AppendLine(new string('-', (depth + 1) * 2) + "* [" + conceptList.Title + $"]({conceptList.Id})");

                    if (conceptList.Concepts != null && conceptList.Concepts.Count > 0)
                    {
                        foreach (var concepta in conceptList.Concepts)
                        {
                            markdown.AppendLine(BuildMarkdownTSLA(concepta, depth + 1));
                        }
                    }

                    if (conceptList.PullFromLists != null && conceptList.PullFromLists.Count > 0)
                    {
                        foreach (var sublist in conceptList.PullFromLists)
                        {
                            markdown.AppendLine(BuildListMarkdownTSLA(sublist, depth + 1));
                        }
                    }
                }                
            }
            return markdown.ToString();
        }

        public static string BuildListMarkdownTSLA(ConceptList list, int depth)
        {
            if (list == null || depth > 10)
            {
                return string.Empty;
            }

            StringBuilder markdown = new StringBuilder();

            markdown.AppendLine(new string('-', depth * 2) + "* [" + list.Title + $"]({list.Id})");

            if (list.PullFromLists != null && list.PullFromLists.Count > 0)
            {
                foreach (var sublist in list.PullFromLists)
                {
                    markdown.AppendLine(BuildListMarkdownTSLA(sublist, depth + 1));
                }            
            }
            else if (list.Concepts != null && list.Concepts.Count > 0)
            {
                foreach (var concept in list.Concepts)
                {
                    markdown.AppendLine(BuildMarkdownTSLA(concept, depth + 1));
                }
            }

            return markdown.ToString();
        }
    }
}

