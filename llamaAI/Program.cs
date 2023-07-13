// See https://aka.ms/new-console-template for more information

using llamaAI;
using llamaAI.Helpers;

string modelPath = DataConstants.WizardLMPath;

// string modelPath = PathHelper.GetPath("Models", "change_model_name_here.bin");

await TaskRunner.Run(modelPath);