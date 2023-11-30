using System.Text;
using Azure;
using Azure.AI.OpenAI;
using ChatAzureOpenAI.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChatAzureOpenAI.Controllers;

public class ChatController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetResponse(IOptions<AzureOpenAIOptions> options, string userMessage)
    {
        OpenAIClient client = new OpenAIClient(new Uri(options.Value.Endpoint), new AzureKeyCredential(options.Value.Key));

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatMessage(ChatRole.System, "You are helpful assistant"),
                new ChatMessage(ChatRole.User, "Does Azure OpenAI support GPT-4"),
                new ChatMessage(ChatRole.Assistant, "Yes it does"),
                new ChatMessage(ChatRole.User, userMessage)
            },
            MaxTokens = 400
        };
        
        // to not install version beta.9. It wont be working
        Response<ChatCompletions> response = await client.GetChatCompletionsAsync(deploymentOrModelName: options.Value.Model, chatCompletionsOptions);

        var botResponse = response.Value.Choices.First().Message.Content;

        return Json(new { Response = botResponse });
    }
    
    public async Task<IActionResult> GetResponseFromPdf(IOptions<AzureOpenAIOptions> options, string userMessage)
    {
        OpenAIClient client = new OpenAIClient(new Uri(options.Value.Endpoint), new AzureKeyCredential(options.Value.Key));
        string pdfText = GetText("Data/lorum-8.pdf");
        
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatMessage(ChatRole.System, "You are helpful assistant"),
                new ChatMessage(ChatRole.User, "The following information is from PDF text: " + pdfText),
                new ChatMessage(ChatRole.User, userMessage)
            },
            MaxTokens = 1000,
            // Change from 0 to 1. 0 - make it more factual. 1 - make set more creative
            Temperature = 0
        };
        
        // to not install version beta.9. It wont be working
        Response<ChatCompletions> response = await client.GetChatCompletionsAsync(deploymentOrModelName: options.Value.Model, chatCompletionsOptions);

        var botResponse = response.Value.Choices.First().Message.Content;

        return Json(new { Response = botResponse });
    }


    private string GetText(string pdfFilePath)
    {
        PdfDocument pdfDocument = new (new PdfReader(pdfFilePath));
        StringBuilder sb = new();

        for (int page = 1; page < pdfDocument.GetNumberOfPages(); page++)
        {
            PdfPage pdfPage = pdfDocument.GetPage(page);
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string currentText = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
            sb.Append(currentText);
        }
        
        pdfDocument.Close();

        return sb.ToString();
    }
}
