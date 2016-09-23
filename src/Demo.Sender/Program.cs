using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;

namespace Demo.Sender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var credentials = new StoredProfileAWSCredentials("default");
            var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1);

            // Create queue

            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = "BFQueue"
            };

            CreateQueueResponse queue = sqsClient.CreateQueueAsync(createQueueRequest).Result;

            Console.WriteLine("Send a message");

            string message = null;
            while((message = Console.ReadLine()) != "q")
            {
                var sendMessageRequest = new SendMessageRequest(queue.QueueUrl, message)
                {
                     
                };

                sqsClient.SendMessageAsync(sendMessageRequest);
            }           
        }
    }
}
