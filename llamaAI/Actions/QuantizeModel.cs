using LLama;

namespace llamaAI.Actions;

public class QuantizeModel
{
    public static void Run(string modelPath)
    {
        Console.Write("Please input your original model path: ");
        var inputPath = Console.ReadLine();
        Console.Write("Please input your output model path: ");
        var outputPath = Console.ReadLine();
        Console.Write("Please input the quantize type (one of q4_0, q4_1, q5_0, q5_1, q8_0): ");
        var quantizeType = Console.ReadLine();
        if (LLamaQuantizer.Quantize(inputPath, outputPath, quantizeType))
        {
            Console.WriteLine("Quantization succeed!");
        }
        else
        {
            Console.WriteLine("Quantization failed!");
        }
    }
}