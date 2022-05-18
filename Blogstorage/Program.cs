// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .Build();

string getConString = config["ConnectionString"];
string containerName = "container-platzi";
string blobName = "jdd_linkeding.jpg";
string filePath = "../../../jdd_linkeding.jpg";

Console.WriteLine("Hello World");
Console.WriteLine(getConString);

BlobContainerClient container = new BlobContainerClient(getConString, containerName);
container.CreateIfNotExists();
container.SetAccessPolicy(accessType: PublicAccessType.BlobContainer);

BlobClient blob = container.GetBlobClient(blobName);

using (var fileStream = System.IO.File.OpenRead(filePath))
{
    blob.Upload(fileStream);
}
