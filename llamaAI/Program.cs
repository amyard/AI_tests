// See https://aka.ms/new-console-template for more information

using llamaAI;

Console.WriteLine("Hello, World!");

string modelPath = DataConstants.WizardLMPath;

await TaskRunner.Run(modelPath);