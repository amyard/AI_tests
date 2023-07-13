using llamaAI.Helpers;

namespace llamaAI;

public static class DataConstants
{
    public static string WizardLMPath = PathHelper.GetPath("Models", "wizardLM-7B.ggmlv3.q8_0.bin");
    public static string OpenLlamaPath = PathHelper.GetPath("Models", "open-llama-3b-q8_0.bin");
    public static string GGMLPath = PathHelper.GetPath("Models", "ggml-model-f32-q8_0.bin");
}