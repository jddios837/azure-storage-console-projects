// See https://aka.ms/new-console-template for more information

using Azure.Storage.Queues;
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
// Get a reference to a queue and then create it
QueueClient queue = new QueueClient(getConString, queueName);
queue.CreateIfNotExists();

// Send a message to our queue
for (int i = 0; i < 15; i++)
{
    queue.SendMessage($"New message {i}");
}

Console.WriteLine("Send all queues");
