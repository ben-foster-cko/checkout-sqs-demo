using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Demo.Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();

            Console.Title = configuration["ApplicationName"];


            var profile = configuration["AWSProfile"];
            
            var credentials = new StoredProfileAWSCredentials(profile);
            var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1);

            // Create queue if not exist

            GetQueueUrlResponse queue = sqsClient.GetQueueUrlAsync("BFQueue").Result;

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queue.QueueUrl,
                WaitTimeSeconds = 10,
                VisibilityTimeout = 10
            };

            while (true)
            {
                Console.WriteLine("Polling Queue");
                var messagesResponse = sqsClient.ReceiveMessageAsync(receiveMessageRequest).Result;
                foreach (var message in messagesResponse.Messages)
                {
                    Console.WriteLine($"Message: { message.Body }");

                    var deleteMessageRequest = new DeleteMessageRequest(queue.QueueUrl, message.ReceiptHandle);
                    DeleteMessageResponse result = sqsClient.DeleteMessageAsync(deleteMessageRequest).Result;
                }
            }

        }
    }
}
