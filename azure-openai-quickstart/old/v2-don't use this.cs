// using Azure;
// using Azure.AI.OpenAI;
// using Azure.AI.OpenAI.Chat;
// using OpenAI.Chat;
//
// namespace azure_openai_quickstart;
//
// public class v2
// {
//     // TODO ---> we need version 2 of Azure.AI.OpenAI
//     string endpoint = "https://bundledocs-develop.openai.azure.com/";
//     string key = "****";
//     string deploymentName = "gpt-35-turbo";
//
//     AzureOpenAIClient azureClient = new(new Uri(endpoint),  new AzureKeyCredential(key));
//
//     // This must match the custom deployment name you chose for your model
//     ChatClient chatClient = azureClient.GetChatClient(deploymentName);
//
//     // model pricing: https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/
//     // Quickstart: Chat with Azure OpenAI models using your own data - take data from azure studio - https://learn.microsoft.com/en-us/azure/ai-services/openai/use-your-data-quickstart?tabs=command-line%2Cpython-new&pivots=programming-language-csharp
//
//
//
//     // https://learn.microsoft.com/en-us/dotnet/api/azure.ai.openai?view=azure-dotnet-preview&preserve-view=true
//     // https://learn.microsoft.com/en-us/dotnet/api/azure.ai.openai.azurechatcompletionoptionsextensions.getdatasources?view=azure-dotnet-preview
//     // https://medium.com/medialesson/use-azure-openai-to-create-a-copilot-on-your-own-data-in-c-part-2-b7acc1922337
//
//     // https://www.youtube.com/watch?v=fYJuokUnucE
//     async Task Test3_StreamingChat()
//     {
//         string promtHere = "some promt should be here !!!!";
//         
//         ChatCompletionOptions options = new()
//         {
//             // Messages = {new AssistantChatMessage(promtHere)},
//             // Messages = new List<ChatMessage>(),
//             // Messages = { new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information.") },
//             Temperature = (float)0.7,
//             MaxTokens = 800,
//             // NucleusSamplingFactor = (float)0.95,
//             FrequencyPenalty = 0,
//             PresencePenalty = 0,
//         };
//         
//         options.AddDataSource(new AzureChatData);
//
//         // while (true)
//         // {
//         //     Console.Write("Chat Prompt:");
//         //     string line = Console.ReadLine()!;
//         //     if (line.Equals("quit", StringComparison.OrdinalIgnoreCase))
//         //     {
//         //         break;
//         //     }
//         //     options.Messages.Add(new ChatMessage(ChatRole.User, line));
//         //
//         //     Console.WriteLine("Response:");
//         //     Response<StreamingChatCompletions> response =
//         //         await client.GetChatCompletionsStreamingAsync(
//         //             deploymentOrModelName: deployment,
//         //             options);
//         //
//         //     using StreamingChatCompletions streamingChatCompletions = response.Value;
//         //     string fullresponse = "";
//         //     await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
//         //     {
//         //         await foreach (ChatMessage message in choice.GetMessageStreaming())
//         //         {
//         //             fullresponse += message.Content;
//         //             Console.Write(message.Content);
//         //         }
//         //         Console.WriteLine();
//         //     }
//         //     options.Messages.Add(new ChatMessage(ChatRole.Assistant, fullresponse));
//         //
//         // }
//     }
//
//
//     // by tutorial: https://learn.microsoft.com/en-us/azure/ai-services/openai/chatgpt-quickstart?tabs=command-line%2Cpython-new&pivots=programming-language-csharp
//     void Test1()
//     {
//         ChatCompletion completion = chatClient.CompleteChat(
//         [
//             new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
//             new UserChatMessage("Does Azure OpenAI support customer managed keys?"),
//             new AssistantChatMessage("Yes, customer managed keys are supported by Azure OpenAI"),
//             new UserChatMessage("Do other Azure AI services support this too?")
//         ]);
//         
//         Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");
//     }
//
//     async Task Test2()
//     {
//         var chatUpdates = chatClient.CompleteChatStreamingAsync(
//         [
//             new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
//             new UserChatMessage("Does Azure OpenAI support customer managed keys?"),
//             new AssistantChatMessage("Yes, customer managed keys are supported by Azure OpenAI"),
//             new UserChatMessage("Do other Azure AI services support this too?")
//         ]);
//         
//         await foreach(var chatUpdate in chatUpdates)
//         {
//             if (chatUpdate.Role.HasValue)
//             {
//                 Console.Write($"{chatUpdate.Role} : ");
//             }
//             
//             foreach(var contentPart in chatUpdate.ContentUpdate)
//             {
//                 Console.Write(contentPart.Text);
//             }
//         }
//     }
// }
