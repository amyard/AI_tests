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
// string key = "41f0c5f50f9348aa8a18f9faab9d3be2";
// string deploymentName = "gpt-35-turbo";
// string encodingName = "cl100k_base";
//
// AzureOpenAIClient azureClient = new(new Uri(endpoint),  new AzureKeyCredential(key));
// ChatClient chatClient = azureClient.GetChatClient(deploymentName);
//
// StringBuilder sb = new();
//
// string t1 = @"""
// {
//   ""Status"": {
//     ""StatusType"": 1,
//     ""Message"": ""success""
//   },
//   ""TextSummarization"": ""The document is a letter from Michael Jenkins, a consulting engineer, to Moriarty \u0026 Partners, solicitors, regarding a road traffic accident that occurred on March 25, 2000, at Kilmore Road, Tagoat, Co. Wexford. The letter includes a site location map, drawings, and photographs of the incident. The accident involved Michael James, who was driving towards his work, and a transit van travelling towards him. Both vehicles slowed but not sufficiently such that an impact occurred more or less on the centre of the road. The letter provides details of the road width, grass margin, and sight distance, and suggests that a safe speed for travelling this road would have been in the region of 25mph. The letter also mentions that the incident occurred at approximately 6:00 a.m. and sunrise would have occurred on that particular date at approximately 6:20 a.m. which suggests that daylight would have begun to occur from approximately 5:50 a.m."",
//   ""DateExtraction"": [
//     {""Date"": ""2000-03-25"", ""Context"": ""Road traffic accident at Kilmore Road, Tagoat, Co. Wexford at approximately 6 a.m.""},
//     {""Date"": ""2000-08-24"", ""Context"": ""Instructions from Moriarty \u0026 Partners, solicitors to Michael Jenkins""},
//     {""Date"": ""2000-10-25"", ""Context"": ""Letter from Michael Jenkins to Moriarty \u0026 Partners, solicitors regarding the road traffic accident""}
//   ]
// }""";
//
// string t2 = @"""
// {
// 	""Status"": {
// 		""StatusType"": 1,
// 		""Message"": ""success""
// 	},
//     ""TextSummarization"": ""The document is a letter from Michael Jenkins, a consulting engineer, to Moriarty & Partners, solicitors, regarding a road traffic accident that occurred on March 25, 2000, at Kilmore Road, Tagoat, Co. Wexford. The letter includes a site location map, drawings, and photographs of the incident. The accident involved Michael James, who was driving towards his work, and a transit van travelling towards him. Both vehicles slowed but not sufficiently such that an impact occurred more or less on the centre of the road. The letter provides details of the road width, grass margin, and sight distance, and suggests that a safe speed for travelling this road would have been in the region of 25mph. The letter concludes that both vehicles were probably travelling at a speed somewhat greater than that which would be regarded as prudent."",
//     ""DateExtraction"": [
//         {""Date"": ""2000-03-25"", ""Context"": ""Road traffic accident at Kilmore Road, Tagoat, Co. Wexford at approximately 6 a.m.""},
//         {""Date"": ""2000-08-24"", ""Context"": ""Instructions from Moriarty & Partners, solicitors""},
//         {""Date"": ""2000-10-25"", ""Context"": ""Letter from Michael Jenkins to Moriarty & Partners, solicitors regarding the road traffic accident""},
//         {""Date"": ""2000-09-05"", ""Context"": ""Michael Jenkins attended the locus with Mr. James to take measurements and photographs and discuss the matter in some detail""},
//         {""Date"": ""2000-03-25"", ""Context"": ""Sunrise occurred at approximately 6:20 a.m. on the date of the accident""},
//         {""Date"": ""2000-03-25"", ""Context"": ""Daylight would have begun to occur from approximately 5:50 a.m. on the date of the accident""}
//     ]
// }""";
//
// string t3 = @"""
// {
//   ""Status"": {
//     ""StatusType"": 1,
//     ""Message"": ""success""
//   },
//   ""TextSummarization"": ""The document is a letter from Michael Jenkins, a consulting engineer, to Moriarty \u0026 Partners, solicitors, regarding a road traffic accident that occurred on March 25, 2000, at Kilmore Road, Tagoat, Co. Wexford. The letter includes a site location map, drawings, and photographs of the accident. The accident occurred at approximately 6 a.m. on a fine, dry, and perhaps frosty or damp road with a general speed limit zone and no lighting. Both vehicles involved in the accident were probably travelling at a speed somewhat greater than that which would be regarded as prudent. Sunrise on the day of the accident occurred at approximately 6:20 a.m., suggesting that there was some small measure of daylight available when the incident occurred."",
//   ""DateExtraction"": [
//     {""Date"": ""2000-03-25"", ""Context"": ""Road traffic accident at Kilmore Road, Tagoat, Co. Wexford at approximately 6 a.m.""},
//     {""Date"": ""2000-08-24"", ""Context"": ""Date of instructions from Moriarty \u0026 Partners, solicitors""},
//     {""Date"": ""2000-10-25"", ""Context"": ""Date of the letter from Michael Jenkins to Moriarty \u0026 Partners, solicitors""},
//     {""Date"": ""2000-09-05"", ""Context"": ""Date when Michael Jenkins attended the locus with Mr. James to take measurements and photographs and discuss the matter in some detail""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date of the road traffic accident""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when Michael James was driving towards his work and encountered a transit van travelling towards him""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when both vehicles slowed but not sufficiently such that an impact occurred more or less on the centre of the road""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when the incident occurred in the centre of the section outlined in red on the site location map""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when the incident occurred at the centre of the sketch where the road width is 3.05 metres (10 feet 2 inches) together with a grass margin on the southern side of approximately 1 foot 8 inches and on the northern side of 2 feet 3 inches suggesting a total width of approximately 4.2 metres (14 feet)""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when both vehicles overlapped by perhaps 600 millimetres (2 feet)""},
//     {""Date"": ""2000-03-25"", ""Context"": ""Date when Michael James\u0027 vehicle skidded when he applied his brakes severely""}
//   ]
// }""";
//
// sb.AppendLine(t1);
// sb.AppendLine(t2);
// sb.AppendLine(t3);
//
// string pdfText = sb.ToString();
//
// string prompt = @"""
// Your task is to combine all the summaries and extract only the main dates that are significant to the overall context of the document. Please ensure that the final summary is concise and highlights the key dates. Store the results in a valid JSON format as follows:
//
// {
//     ""Status"": {
//         ""StatusType"": %StatusType%,
//         ""Message"": %Message%,
//
//     },
//     ""TextSummarization"": ""%TextSummary%"",
//     ""DateExtraction"": [
//         {""Date"": %Date%, ""Context"": %Context%},
//         ...
//         {""Date"": %Date%, ""Context"": %Context%},
//     ]
// }""";
//
//
// #pragma warning disable AOAI001
//
// var chatCompletionsOptions = new ChatCompletionOptions {
//     Temperature = 0,
//     MaxTokens = 1000,
// };
//
// ChatCompletion completion = chatClient.CompleteChat(
// [
//     new SystemChatMessage("You are a helpful AI Assistant"),
//     new UserChatMessage("I have a document that has been split into chunks, and each chunk has been summarized with context to specific dates. I will provide you with an extracted object containing these dates and their corresponding summaries. " + pdfText),
//     new UserChatMessage(prompt),
// ], options: chatCompletionsOptions);
//
// var botResponse = completion.Content.First().ToString();
// Console.WriteLine(botResponse);
//
// var asd = "aaa";
//
