using LLama;
using LLama.Common;
using llamaAI.Helpers;

namespace llamaAI.Actions;

public class SaveAndLoadSession
{
    public static void Run(string modelPath)
    {
        var dataPath = PathHelper.GetPath("Assets", "chat-with-bob.txt");
        var prompt = File.ReadAllText(dataPath).Trim();
        
        InteractiveExecutor ex = new(new LLamaModel(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 5)));
        ChatSession session = new ChatSession(ex); // The only change is to remove the transform for the output text stream.

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The chat session has started. In this example, the prompt is printed for better visual result. Input \"save\" to save and reload the session.");
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
            if (prompt == "save")
            {
                Console.Write("Preparing to save the state, please input the path you want to save it: ");
                Console.ForegroundColor = ConsoleColor.Green;
                var statePath = Console.ReadLine();
                session.SaveSession(statePath);
                Console.ForegroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Saved session!");
                Console.ForegroundColor = ConsoleColor.White;

                ex.Model.Dispose();
                ex = new(new LLamaModel(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 5)));
                session = new ChatSession(ex);
                session.LoadSession(statePath);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Loaded session!");
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("Now you can continue your session: ");
                Console.ForegroundColor = ConsoleColor.Green;
                prompt = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}