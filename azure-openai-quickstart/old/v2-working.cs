// using System.Text;
// using Azure;
// using Azure.AI.OpenAI;
// using Azure.AI.OpenAI.Chat;
// using iText.Kernel.Pdf;
// using iText.Kernel.Pdf.Canvas.Parser;
// using iText.Kernel.Pdf.Canvas.Parser.Listener;
// using OpenAI.Chat;
// using SharpToken;
// using TiktokenSharp;
//
// string endpoint = "https://bundledocs-develop.openai.azure.com/";
// string key = "***";
// string deploymentName = "gpt-35-turbo";
// string encodingName = "cl100k_base";
//
// AzureOpenAIClient azureClient = new(new Uri(endpoint),  new AzureKeyCredential(key));
// ChatClient chatClient = azureClient.GetChatClient(deploymentName);
//
// string pdfText = GetText(@"C:\LITROOT\Projects\dotNet\AI_tests\azure-openai-quickstart\Data\EngineersReport.pdf");
// int sharpTokenEncoding = SharpTokenEncoding(pdfText);
// int tiktokenSharpEncoding = TiktokenSharpEncoding(pdfText);
// Console.WriteLine($"sharpTokenEncoding - {sharpTokenEncoding}. tiktokenSharpEncoding - {tiktokenSharpEncoding}");
//
// // THIS WORKED AS EXPECTED !!!
// // string prompt = @"""Extract and provide only the most relevant dates from the PDF document.Ensure that the dates are unique in the output.Format the extracted dates in the 'YYYY-MM-DD' format.For each extracted date, include a brief context that describes the main information related to that date.The output should be sorted in ascending order by date.Summarize the text in up to 10 sentences. Also StatusType should be equal to 1. Store the results in a valid JSON format as follows:
// //
// // {
// //     ""StatusType"": ""1"",
// //     ""TextSummarization"": ""%TextSummary%"",
// //     ""DateExtraction"": [
// //         {""Date"": %Date%, ""Context"": %Context%},
// //         ...
// //         {""Date"": %Date%, ""Context"": %Context%},
// //     ]
// // }""";
//
// string prompt = @"""
// Extract and provide only the most relevant dates from the PDF document. Ensure that the dates are unique and formatted in 'YYYY-MM-DD'. For each date, include a brief context describing the main information related to it. Sort the output in ascending order by date. Summarize the text in up to 10 sentences. The StatusType should be 1, and the Message should be 'success'. Store the results in a valid JSON format as follows:
//
// {
//     ""StatusType"": ""1"",
//     ""Message"": ""success"",
//     ""TextSummarization"": ""%TextSummary%"",
//     ""DateExtraction"": [
//         {""Date"": %Date%, ""Context"": %Context%},
//         ...
//         {""Date"": %Date%, ""Context"": %Context%},
//     ]
// }""";
//
//
//
// // Extension methods to use data sources with options are subject to SDK surface changes. Suppress the
// // warning to acknowledge and this and use the subject-to-change AddDataSource method.
// #pragma warning disable AOAI001
//
// var chatCompletionsOptions = new ChatCompletionOptions {
//     // MaxTokens = 1000,
//     Temperature = 0,
//     // Temperature = (float)0.7,
//     MaxTokens = 1000,
//     // FrequencyPenalty = 0,
//     // PresencePenalty = 0,
//     // TopP = (float)0.95
//     // TopP = (float)0
// };
//
// ChatCompletion completion = chatClient.CompleteChat(
// [
//     new SystemChatMessage("You are a helpful AI Assistant"),
//     new UserChatMessage("The following information are from PDF document: " + pdfText),
//     new UserChatMessage(prompt),
// ], options: chatCompletionsOptions);
//
// var botResponse = completion.Content.First().ToString();
// Console.WriteLine(botResponse);
//
// var asd = "aaa";
//
// // AzureChatMessageContext onYourDataContext = completion.GetAzureMessageContext();
// //
// // if (onYourDataContext?.Intent is not null)
// // {
// //     Console.WriteLine($"Intent: {onYourDataContext.Intent}");
// // }
// // foreach (AzureChatCitation citation in onYourDataContext?.Citations ?? [])
// // {
// //     Console.WriteLine($"Citation: {citation.Content}");
// // }
//
// // Response<ChatCompletions> response = azureClient.GetChatCompletions(deploymentOrModelName: deploymentName, chatCompletionsOptions);
// // var botResponse = response.Value.Choices.First().Message.Content;
// //
// // Console.WriteLine(botResponse);
//
//
//
// // https://github.com/dmitry-brazhenko/SharpToken
// int SharpTokenEncoding(string text)
// {
//     var encoding = GptEncoding.GetEncoding(encodingName);
//     // var encoded = encoding.Encode(text);
//     int encoded = encoding.CountTokens(text);
//     
//     return encoded;
// }
//
// // https://github.com/aiqinxuancai/TiktokenSharp
// int TiktokenSharpEncoding(string text)
// {
//     var encoding = TikToken.GetEncoding(encodingName);
//     // var encoded = encoding.Encode(text);
//     List<int> encoded = encoding.Encode(text);
//     
//     return encoded.Count();
// }
//
//
// string GetText(string pdfPath)
// {
//     PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfPath));
//     StringBuilder text = new();
//
//     try
//     {
//         for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
//         {
//             PdfPage pdfPage = pdfDocument.GetPage(page);
//             ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
//             string currentText = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
//             text.Append(currentText);
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex);
//     }
//     finally
//     {
//         pdfDocument.Close();
//     }
//     
//     return text.ToString();
// }
