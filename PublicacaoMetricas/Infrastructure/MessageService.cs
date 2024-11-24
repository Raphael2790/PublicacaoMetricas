using System.Text.Json;
using Amazon.SQS.Model;
using PublicacaoMetricas.Abstractions.Infrastructure;
using PublicacaoMetricas.Common;

namespace PublicacaoMetricas.Infrastructure;

public class MessageService() : IMessageService
{
    public async Task<ServiceResult> SendMessageAsync(string message)
    {
        try
        {
            var request = new SendMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue",
                MessageBody = message
            };
            
            await Task.Delay(2000);
            
            return ServiceResult.CreateSuccess();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error sending message to SQS: {e.Message}");
            return ServiceResult.CreateError("Error sending message to SQS");
        }
    }
}