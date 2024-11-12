// using System.Text;
// using Azure;
// using Azure.AI.OpenAI;
// using iText.Kernel.Pdf;
// using iText.Kernel.Pdf.Canvas.Parser;
// using iText.Kernel.Pdf.Canvas.Parser.Listener;
// using SharpToken;
// using TiktokenSharp;
//
// string endpoint = "https://bundledocs-develop.openai.azure.com/";
// string key = "****";
// string deploymentName = "gpt-35-turbo";
// string encodingName = "cl100k_base";
//
// OpenAIClient azureClient = new(new Uri(endpoint),  new AzureKeyCredential(key));
// string pdfText = GetText(@"C:\LITROOT\Projects\dotNet\AI_tests\azure-openai-quickstart\Data\EngineersReport.pdf");
// int sharpTokenEncoding = SharpTokenEncoding(pdfText);
// int tiktokenSharpEncoding = TiktokenSharpEncoding(pdfText);
// Console.WriteLine($"sharpTokenEncoding - {sharpTokenEncoding}. tiktokenSharpEncoding - {tiktokenSharpEncoding}");
//
// // return;
//
// // string prompt = @"""
// //     Provide the most relevant dates from PDF document. Ensure the output is limited to JSON format only. Format the answer in Context | Date only. Format the dates found in ISO8601. Limit the context to one sentence.
// // """;
//
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD.";
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. The output result should be valid json format.  [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]";
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. Sort result by date. Return only 10 unique dates. The output result should be valid json format.  [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]";
//
//
//
// // string prompt = @"""
// //     Provide the most relevant dates from PDF document. 
// //     First, I want to extract dates and context to each date. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. Sort result by date. The output result should be valid json format.  [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]
// // """;
//
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. The output result should be valid json format and sorted by extracted date ascending . Also provide text summary. TExt summary should be 10 sentences maximum. the result should be stored in valid json format. {'TextSummarization': %TextSummary%, 'DateExtraction': [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]} ";
//
//
// // CORRECT
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. Sort result by date. The output result should be valid json format.  [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]";
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. Sort result by date. The output result should be valid json format.  {'DateExtraction': [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]}";
// // string prompt = @"Provide the most relevant dates from PDF document. Format the answer in Context | Date. Format the dates found in YYYYY-MM-DD. The output result should be valid json format and sorted by extracted date ascending.  {'DateExtraction': [{%Date%: %Context%},{%Date%: %Context%},{%Date%: %Context%}]}";
//
// // CORRECT - LAST !!!
// // string prompt = @"Extract and provide the most relevant dates from the PDF document. Format the answer in 'Context' | 'Date'. Format the dates found in 'YYYY-MM-DD'. The output result should be in valid JSON format, with the dates sorted in ascending order. Additionally, provide a text summary of up to 10 sentences. The result should be stored in a valid JSON format like this:
// //
// // {
// //     'TextSummarization': '%TextSummary%',
// //     'DateExtraction': [
// //         {%Date%: %Context%},
// //         ...
// //         {%Date%: %Context%}
// //     ]
// // }";
//
//
// string prompt = @"Extract and provide the most relevant dates from the PDF document. Format the answer in 'Context' | 'Date'. Format the dates found in 'YYYY-MM-DD'. The output result should be in valid JSON format, with the dates sorted in ascending order. Additionally, provide a text summary of up to 10 sentences. The result should be stored in a valid JSON format like this:
//
// {
//     'TextSummarization': '%TextSummary%',
//     'DateExtraction': [
//         {'Date': %Date%,  'Context': %Context%},
//         ...
//         {'Date': %Date%,  'Context': %Context%},
//     ]
// }";
//
//
// // WRONG !!!!
// // string prompt = "Extract and provide the most relevant dates from the PDF document. " + 
// //                 "Format the answer in 'Context' | 'Date'. Format the dates found in 'YYYY-MM-DD'. The output result should be in valid JSON format, with the dates sorted in ascending order." +
// //                 "Additionally, provide a text summary of up to 10 sentences. The result should be stored in a valid JSON format like this: " +
// //                 "\n" +
// //                 """
// //                 {
// //                     'TextSummarization': '%TextSummary%',
// //                     'DateExtraction': [
// //                         {%Date%: %Context%},
// //                         ...
// //                         {%Date%: %Context%}
// //                     ]
// //                 }
// //                 """;
//
//
// // string prompt = "Extract and provide the most relevant dates from the PDF document. " + 
// //                 "Format the answer in 'Date' | 'Context'. Format the dates found in 'YYYY-MM-DD'. The output result should be in valid JSON format, with the dates sorted in ascending order." +
// //                 "Additionally, provide a text summary of up to 10 sentences. The result should be stored in a valid JSON format like this: " +
// //                 "\n" +
// //                 """
// //                 {
// //                     'TextSummarization': '%TextSummary%',
// //                     'DateExtraction': [
// //                         {'Date': %Date%,  'Context': %Context%},
// //                         ...
// //                         {'Date': %Date%,  'Context': %Context%},
// //                     ]
// //                 }
// //                 """;
//
// var chatCompletionsOptions = new ChatCompletionsOptions() {
//     Messages = {
//         new ChatMessage(ChatRole.System, "You are a helpful AI Assistant"),
//         new ChatMessage(ChatRole.User, "The following information are from PDF document: " + pdfText),
//         new ChatMessage(ChatRole.User, prompt)
//     },
//     MaxTokens = 1000,
//     Temperature = 0
// };
//
// Response<ChatCompletions> response = azureClient.GetChatCompletions(deploymentOrModelName: deploymentName, chatCompletionsOptions);
// var botResponse = response.Value.Choices.First().Message.Content;
//
// Console.WriteLine(botResponse);
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
