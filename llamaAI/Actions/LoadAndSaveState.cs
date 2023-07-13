﻿using LLama;
using LLama.Common;
using llamaAI.Helpers;

namespace llamaAI.Actions;

public class LoadAndSaveState
{
    public static void Run(string modelPath)
    {
        var dataPath = PathHelper.GetPath("Assets", "chat-with-bob.txt");
        var prompt = File.ReadAllText(dataPath).Trim();

        InteractiveExecutor ex = new(new LLamaModel(new ModelParams(modelPath, contextSize: 256)));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The executor has been enabled. In this example, the prompt is printed, the maximum tokens is set to 64 and the context size is 256. (an example for small scale usage)");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(prompt);

        var inferenceParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } };

        while (true)
        {
            foreach (var text in ex.Infer(prompt, inferenceParams))
            {
                Console.Write(text);
            }

            prompt = Console.ReadLine();
            if (prompt == "save")
            {
                Console.Write("Your path to save model state: ");
                string modelStatePath = Console.ReadLine();
                ex.Model.SaveState(modelStatePath);

                Console.Write("Your path to save executor state: ");
                string executorStatePath = Console.ReadLine();
                ex.SaveState(executorStatePath);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("All states saved!");
                Console.ForegroundColor = ConsoleColor.White;

                var model = ex.Model;
                model.LoadState(modelStatePath);
                ex = new InteractiveExecutor(model);
                ex.LoadState(executorStatePath);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Loaded state!");
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("Now you can continue your session: ");
                Console.ForegroundColor = ConsoleColor.Green;
                prompt = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}