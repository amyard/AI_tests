using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DurableFunctionExample
{
    public static class DurableFunction
    {
        public class OrchestratorParameters
        {
            public string PartitionKey { get; set; } = string.Empty;
            public string RowKey { get; set; } = string.Empty;
            public string UserPartitionKey { get; set; } = string.Empty;
            public string UserRowKey { get; set; } = string.Empty;
        }

        // READ THIS !!!
        // https://github.com/amyard/AZ-204/blob/main/FunctionApp/GetProduct.cs
        // https://medium.com/microsoftazure/stateful-serverless-long-running-workflows-with-durable-functions-39ef5c96440b

        [FunctionName("AIStartProcess")]
        public static async Task<IActionResult> AIStartProcess(
            // [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrchestratorParameters parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<OrchestratorParameters>(requestBody);

            string instanceId = await starter.StartNewAsync("OrchestratorFunction", parameters ?? new OrchestratorParameters());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("OrchestratorFunction")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var parameters = context.GetInput<OrchestratorParameters>();

            var isValid = await context.CallActivityAsync<bool>("ValidateFunction", parameters);
            if (!isValid)
            {
                return;
            }

            // Call three functions that return numeric values
            var task1 = context.CallActivityAsync<int>("GetValueFunction1", null);
            var task2 = context.CallActivityAsync<int>("GetValueFunction2", null);
            var task3 = context.CallActivityAsync<int>("GetValueFunction3", null);
            await Task.WhenAll(task1, task2, task3);

            var values = new List<int> { task1.Result, task2.Result, task3.Result };
            // TODO ---> THE SAME !!!
            //var tasks = new List<Task<int>>();
            //for (int i = 1; i <= 3; i++)
            //{
            //    tasks.Add(context.CallActivityAsync<int>($"GetValueFunction{i}", null));
            //}
            //var values = await Task.WhenAll(tasks);

            // Sum the values
            var sum = await context.CallActivityAsync<int>("SumFunction", values);

            // Save the result to a file
            await context.CallActivityAsync("SaveDataFunction", sum);
        }

        [FunctionName("ValidateFunction")]
        public static bool ValidateFunction([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var parameters = context.GetInput<OrchestratorParameters>();

            bool isPartitionKeyValid = Guid.TryParse(parameters.PartitionKey, out _);
            bool isRowKeyValid = Guid.TryParse(parameters.RowKey, out _);
            bool isUserPartitionKeyValid = Guid.TryParse(parameters.UserPartitionKey, out _);
            bool isUserRowKeyValid = Guid.TryParse(parameters.UserRowKey, out _);

            bool areAllKeysValid = isPartitionKeyValid && isRowKeyValid && isUserPartitionKeyValid && isUserRowKeyValid;
            log.LogInformation($"Validation result: {areAllKeysValid}");

            return areAllKeysValid;
        }

        [FunctionName("GetValueFunction1")]
        public static int GetValueFunction1([ActivityTrigger] IDurableActivityContext context, ILogger log) => new Random().Next(1, 101);

        [FunctionName("GetValueFunction2")]
        public static int GetValueFunction2([ActivityTrigger] IDurableActivityContext context, ILogger log) => new Random().Next(1, 101);

        [FunctionName("GetValueFunction3")]
        public static int GetValueFunction3([ActivityTrigger] IDurableActivityContext context, ILogger log) => new Random().Next(1, 101);

        [FunctionName("SumFunction")]
        public static int SumFunction([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            int[] values = context.GetInput<int[]>();
            int sum = values.Length > 0 ? values.Sum() : 0;
            log.LogInformation($"Sum of values: {sum}");
            return sum;
        }

        [FunctionName("SaveData")]
        public static async Task SaveData([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            int sum = context.GetInput<int>();
            string fileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.txt";
            string filePath = Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, fileName);

            await File.WriteAllTextAsync(filePath, sum.ToString());
            log.LogInformation($"Saved data to {filePath}");
        }
    }
}
