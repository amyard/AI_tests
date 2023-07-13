﻿using LLama;
using LLama.Common;

namespace llamaAI.Actions;

public class GetEmbeddings
{
    public static void Run(string modelPath)
    {
        var embedder = new LLamaEmbedder(new ModelParams(modelPath));

        while (true)
        {
            Console.Write("Please input your text: ");
            Console.ForegroundColor = ConsoleColor.Green;
            var text = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(string.Join(", ", embedder.GetEmbeddings(text)));
            Console.WriteLine();
        }
    }
}