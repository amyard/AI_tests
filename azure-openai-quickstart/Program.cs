﻿using System.Text;
using Azure;
using Azure.AI.OpenAI;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using SharpToken;
using TiktokenSharp;

string endpoint = "https://bundledocs-develop.openai.azure.com/";
string key = "41f0c5f50f9348aa8a18f9faab9d3be2";
string deploymentName = "gpt-35-turbo";
string encodingName = "cl100k_base";

OpenAIClient azureClient = new(new Uri(endpoint),  new AzureKeyCredential(key));
string pdfText = GetText(@"C:\LITROOT\Projects\dotNet\AI_tests\azure-openai-quickstart\Data\EngineersReport.pdf");
int sharpTokenEncoding = SharpTokenEncoding(pdfText);
int tiktokenSharpEncoding = TiktokenSharpEncoding(pdfText);
Console.WriteLine($"sharpTokenEncoding - {sharpTokenEncoding}. tiktokenSharpEncoding - {tiktokenSharpEncoding}");

// THIS WORKED AS EXPECTED !!!
string prompt = @"Extract and provide the most relevant dates from the PDF document. Format the answer in 'Context' | 'Date'. Format the dates found in 'YYYY-MM-DD'. The output result should be in valid JSON format, with the dates sorted in ascending order. Additionally, provide a text summary of up to 10 sentences. The result should be stored in a valid JSON format like this:

{
    'TextSummarization': '%TextSummary%',
    'DateExtraction': [
        {'Date': %Date%,  'Context': %Context%},
        ...
        {'Date': %Date%,  'Context': %Context%},
    ]
}";


var chatCompletionsOptions = new ChatCompletionsOptions() {
    Messages = {
        new ChatMessage(ChatRole.System, "You are a helpful AI Assistant"),
        new ChatMessage(ChatRole.User, "The following information are from PDF document: " + pdfText),
        new ChatMessage(ChatRole.User, prompt)
    },
    MaxTokens = 1000,
    Temperature = 0
};

Response<ChatCompletions> response = azureClient.GetChatCompletions(deploymentOrModelName: deploymentName, chatCompletionsOptions);
var botResponse = response.Value.Choices.First().Message.Content;

Console.WriteLine(botResponse);



// https://github.com/dmitry-brazhenko/SharpToken
int SharpTokenEncoding(string text)
{
    var encoding = GptEncoding.GetEncoding(encodingName);
    // var encoded = encoding.Encode(text);
    int encoded = encoding.CountTokens(text);
    
    return encoded;
}

// https://github.com/aiqinxuancai/TiktokenSharp
int TiktokenSharpEncoding(string text)
{
    var encoding = TikToken.GetEncoding(encodingName);
    // var encoded = encoding.Encode(text);
    List<int> encoded = encoding.Encode(text);
    
    return encoded.Count();
}


string GetText(string pdfPath)
{
    PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfPath));
    StringBuilder text = new();

    try
    {
        for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
        {
            PdfPage pdfPage = pdfDocument.GetPage(page);
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string currentText = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
            text.Append(currentText);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    finally
    {
        pdfDocument.Close();
    }
    
    return text.ToString();
}
