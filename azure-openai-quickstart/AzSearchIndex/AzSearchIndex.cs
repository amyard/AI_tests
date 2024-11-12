using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;

namespace azure_openai_quickstart.AzSearchIndex;

public static class AzSearchIndex
{
    private static string searchServiceEndpoint = "*********";
    private static string adminApiKey = "*********";
    private static string indexName = "workspace-index";
    private static string semanticConfigName = "bundledocs-semantic-configs";

    public static void Start(bool skipIndexCreating = false, bool deleteIndex = false)
    {
        Console.WriteLine("{0}", "Start");
        
        // Create a SearchIndexClient to send create/delete index commands
        Uri serviceEndpoint = new Uri(searchServiceEndpoint);
        AzureKeyCredential credential = new AzureKeyCredential(adminApiKey);
        SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);

        if (deleteIndex)
        {
            // Delete index if it exists
            Console.WriteLine("{0}", "Deleting index...\n");
            DeleteIndexIfExists(indexName, adminClient);
        }

        if (skipIndexCreating)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            
            // Create index
            Console.WriteLine("{0}", "Creating index...");
            CreateIndex(indexName, adminClient);
        }

        // we can have a problem if Index wasn't created before
        // SearchClient ingesterClient = adminClient.GetSearchClient(indexName);
        Thread.Sleep(TimeSpan.FromSeconds(2));
        
        // Create a SearchClient to load and query documents
        SearchClient searchClient = new SearchClient(serviceEndpoint, indexName, credential);
        
        Console.WriteLine("{0}", "Insert document...");
        IndexDocument(searchClient);
        
        Console.WriteLine("{0}", "End");
    }

    public static void RunQueries()
    {
        // Create a SearchClient to load and query documents
        Uri serviceEndpoint = new Uri(searchServiceEndpoint);
        AzureKeyCredential credential = new AzureKeyCredential(adminApiKey);
        SearchClient searchClient = new SearchClient(serviceEndpoint, indexName, credential);
        
        string query = "The car is red";
        string query2 = "color of the car";

        // Console.WriteLine("LiteralQuery - query 1");
        // ExecuteLiteralQuery(searchClient, query);
        
        // Console.WriteLine("SemanticQuery - query 1");
        // ExecuteSemanticQuery(searchClient, query);
        
        Console.WriteLine("SemanticQuery - query 2");
        ExecuteSemanticQuery(searchClient, query2);
    }
    
    private static void ExecuteLiteralQuery(SearchClient client, string query)
    {
       
        SearchOptions options = new SearchOptions
        {
            // Filter = "Title eq 'workspace1'",
            OrderBy = { "" },
            Select = { "Title", "Content" },
        };
        
        // Query 1. THIS WORK !!!
        // SearchResults<PdfDocument> response = client.Search<PdfDocument>("*", options);
        
        // Query 2. Full text search on '{query}' with BM25 ranking...\n
        SearchResults<PdfDocument> response = client.Search<PdfDocument>(query, options);

        foreach (SearchResult<PdfDocument> result in response.GetResults())
        {
            // Console.WriteLine($"Literal Search - Document WorkspaceId: {result.Document.WorkspaceId}");
            Console.WriteLine($"Literal Search - Document Title: {result.Document.Title}");
            Console.WriteLine($"Literal Search - Document Content: {result.Document.Content}");
        }
    }
    
    private static void ExecuteSemanticQuery(SearchClient client, string query)
    {
        var options = new SearchOptions
        {
            QueryType = SearchQueryType.Semantic,
            SemanticSearch = new()
            {
                SemanticConfigurationName = semanticConfigName,
                QueryCaption = new(QueryCaptionType.Extractive)
            },
            // Filter = "Title eq 'workspace1'",
            OrderBy = { "" },
            Select = { "Title", "Content" },
        };
    
        SearchResults<PdfDocument> response = client.Search<PdfDocument>(query, options);
    
        foreach (SearchResult<PdfDocument> result in response.GetResults())
        {
            // Console.WriteLine($"Semantic Search - Document WorkspaceId: {result.Document.WorkspaceId}");
            Console.WriteLine($"Semantic Search - Document Title: {result.Document.Title}");
            Console.WriteLine($"Semantic Search - Document Content: {result.Document.Content}");
        }
    }
    
    private static void CreateIndex(string indexName, SearchIndexClient adminClient)
    {
        IList<SearchField>? searchFields = new FieldBuilder().Build(typeof(PdfDocument));
        SearchIndex definition = new SearchIndex(indexName, searchFields);
        definition.SemanticSearch = new SemanticSearch
        {
            Configurations =
            {
                new SemanticConfiguration(semanticConfigName, new SemanticPrioritizedFields()
                {
                    TitleField = new SemanticField("Title"),
                    ContentFields =
                    {
                        // new SemanticField("Title"),
                        // new SemanticField("WorkspaceId"),
                        new SemanticField("Content"),
                        new SemanticField("ProcessedContent")
                    },
                    // KeywordsFields =
                    // {
                    //     new SemanticField("Tags"),
                    //     new SemanticField("Category")
                    // }
                })
            }
        };
        
        adminClient.CreateOrUpdateIndexAsync(definition);
        Console.WriteLine("{0}", "Index was created ...\n");
    }
    
    private static void DeleteIndexIfExists(string indexName, SearchIndexClient adminClient)
    {
        adminClient.GetIndexNames();
        {
            adminClient.DeleteIndex(indexName);
            Console.WriteLine("Index was deleted.");
        }
    }

    private static void IndexDocument(SearchClient client)
    {
        PdfDocument document1 = new( "1", "Car Specifications", "workspace1",
            "The car is red. It has a sleek design and modern features.",
            // processedContent = TextProcessor.ProcessText("The car is red. It has a sleek design and modern features.")
            "The car is red. It has a sleek design and modern features."
        );

        PdfDocument document2 = new("2", "Car Specifications 2", "workspace2",
            "The car is red. It has a sleek design and modern features.",
            "The car is red. It has a sleek design and modern features."
        );

        PdfDocument document3 = new("3", "Car Specifications 2", "workspace1",
            "The car is blue. It has a sleek design and modern features.",
            "The car is blue. It has a sleek design and modern features."
        );

        PdfDocument document4 = new("4", "Car Specifications 2", "workspace2",
            "The car is blue. It has a sleek design and modern features.",
            "The car is blue. It has a sleek design and modern features."
        );

        PdfDocument document5 = new("5", "Some text", "workspace1",
            "Some text will be here. It has a sleek design and modern features.",
            "Some text will be here. It has a sleek design and modern features."
        );

        PdfDocument document6 = new("6", "Features", "workspace1",
            "Features are enabled.",
            "Features are enabled."
        );

        var batch = IndexDocumentsBatch.Upload([document1, document2, document3, document4, document5, document6]);
        client.IndexDocuments(batch);
        Console.WriteLine("Document indexed successfully.");
    }
    
    class PdfDocument
    {
        public PdfDocument(string id, string title, string workspaceId, string content, string processedContent)
        {
            Id = id;
            Title = title;
            WorkspaceId = workspaceId;
            Content = content;
            ProcessedContent = processedContent;
        }
        
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string Id { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Title { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string WorkspaceId { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true)]
        public string Content { get; set; }

        // LexicalAnalyzerName.Values.FrLucene - use for different languages
        // [SearchableField(IsFilterable = true, IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
        [SearchableField(IsFilterable = true, IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
        public string ProcessedContent { get; set; }
    }
}
