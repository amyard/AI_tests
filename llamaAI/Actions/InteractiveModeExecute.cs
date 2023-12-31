﻿using LLama;
using LLama.Common;
using llamaAI.Helpers;

namespace llamaAI.Actions;

public class InteractiveModeExecute
{
    public async static Task Run(string modelPath)
    {
        var dataPath = PathHelper.GetPath("Assets", "chat-with-bob.txt");
        var prompt = File.ReadAllText(dataPath).Trim();

        InteractiveExecutor ex = new(new LLamaModel(new ModelParams(modelPath, contextSize: 256)));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The executor has been enabled. In this example, the prompt is printed, the maximum tokens is set to 128 and the context size is 256. (an example for small scale usage)");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(prompt);

        var inferenceParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" }, MaxTokens = 128 };

        while (true)
        {
            await foreach (var text in ex.InferAsync(prompt, inferenceParams))
            {
                Console.Write(text);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            prompt = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}