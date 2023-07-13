// See https://aka.ms/new-console-template for more information

using llamaAI;

string modelPath = DataConstants.WizardLMPath;

await TaskRunner.Run(modelPath);