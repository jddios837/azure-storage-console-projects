// See https://aka.ms/new-console-template for more information

using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .Build();

string getConString = config["ConnectionString"];

Console.WriteLine("Hello World");
Console.WriteLine(getConString);

// Name of the queue we'll send messages to
string queueName = "yuxi-queue";
// Get a reference to a queue and then fill it with messages
QueueClient queue = new QueueClient(getConString, queueName);

string containerName = "container-queue";
BlobContainerClient container = new BlobContainerClient(getConString, containerName);
container.CreateIfNotExists();

// Get the next messages from the queue
foreach (QueueMessage message in queue.ReceiveMessages(maxMessages: 10).Value)
{
    // "Process" the message
    Console.WriteLine($"Message: {message.Body}");

    string filePath = string.Format(@"log{0}.txt", message.MessageId);
    TextWriter tempFile = File.CreateText(filePath);
    tempFile.WriteLine(message.Body);
    tempFile.Close();
    
    
    using (var fileStream = System.IO.File.OpenRead(filePath))
    {
        BlobClient blob = container.GetBlobClient($@"log{message.MessageId}.txt");
        blob.Upload(fileStream);
        Console.WriteLine("Blob Creado");
    }

    // Let the service know we're finished with the message and
    // it can be safely deleted.
    queue.DeleteMessage(message.MessageId, message.PopReceipt);
}