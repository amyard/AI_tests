using LLama;
using LLama.Common;

namespace llamaAI.Actions;

public class ChatSessionWithRoleName
{
    public static void Run(string modelPath)
    {
        var prompt = File.ReadAllText("Assets/chat-with-bob.txt").Trim();
        
        InteractiveExecutor ex = new(new LLamaModel(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 5)));
        ChatSession session = new ChatSession(ex); // The only change is to remove the transform for the output text stream.

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The chat session has started. In this example, the prompt is printed for better visual result.");
        Console.ForegroundColor = ConsoleColor.White;

        // show the prompt
        Console.Write(prompt);
        while (true)
        {
            foreach (var text in session.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
            {
                Console.Write(text);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            prompt = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}