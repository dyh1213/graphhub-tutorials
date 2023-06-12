using GraphHub.Shared;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;

public class TempDatabase
{
    public Dictionary<string, GraphDatabase> FileGraphDatabase { get; set; } = new Dictionary<string, GraphDatabase>();

    private static TempDatabase? instance = null;
    private static readonly object padlock = new object();

    private TempDatabase()
    {
        List<string> fileNames = new List<string> { "tesla.json", "midjourney.json" }; // Add or get your list of filenames here

        foreach (var fileName in fileNames)
        {
            var graphData = LoadJsonData(fileName);
            if (graphData != null)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                foreach(var concept in graphData.Concepts)
                {
                    if (concept.PluralTitle == null)
                    {
                        concept.PluralTitle = concept.Title;
                    }
                }
                foreach (var list in graphData.Lists)
                {
                    if (list.PluralTitle == null)
                    {
                        list.PluralTitle = list.Title;
                    }
                }
                var database = new GraphDatabase(graphData.Concepts, graphData.Lists, graphData.Memberships, graphData.ConceptMarkdown);
                FileGraphDatabase.Add(fileNameWithoutExtension, database);
            }
        }
    }

    private GraphData? LoadJsonData(string fileName)
    {
        string content = "";
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"ConceptsData/{fileName}");

        try
        {
            content = File.ReadAllText(path);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File not found: {path}");
            return null;
        }
        catch (Exception ex)
        {
            // other error handling here
            Console.WriteLine(ex.ToString());
            return null;
        }

        return JsonSerializer.Deserialize<GraphData>(content);
    }

    public static TempDatabase Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TempDatabase();
                }
                return instance;
            }
        }
    }
}