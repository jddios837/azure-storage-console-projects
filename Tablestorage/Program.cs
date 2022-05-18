// See https://aka.ms/new-console-template for more 

using System.Net;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Configuration;
using Tablestorage;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .Build();
string getConString = config["ConnectionString"];
string tName = "todos";

Console.WriteLine("Hello, World!");
Console.WriteLine(getConString);
var table = await CreateTableAsync(tName, getConString);
InsertOperationAsync(table).Wait();
//UpdateOperationAsync(table).Wait();
//ReadOperationAsync(table).Wait();
//DeleteOperationAsync(table, "hacer las actividades del hogar","Preparar hogar").Wait();

static async Task<TableClient> CreateTableAsync(string tableName, string connectionString)
{
    // Construct a new TableClient using a connection string.
    var client = new TableClient(connectionString, tableName);

    // Create the table if it doesn't already exist.
    Console.WriteLine(await client.CreateIfNotExistsAsync() != null ? "Table created: {0}" : "Table already exist: {0}", tableName);
    return client;
}

static async Task InsertOperationAsync(TableClient table)
{
    Todo todo = new Todo("Preparar hogar", "hacer las actividades del hogar")
    {
        Done = false
    };

    table.AddEntity(todo);
}

static async Task UpdateOperationAsync(TableClient table)
{
    Todo todo = new Todo("Preparar hogar", "hacer las actividades del hogar")
    {
        Done = true
    };

    await table.UpdateEntityAsync(todo,ETag.All, TableUpdateMode.Replace);
    Console.WriteLine("Registro Actualizado");
}

static async Task ReadOperationAsync(TableClient table)
{
    Pageable<Todo> queryResults = table
        .Query<Todo>(ent => ent.PartitionKey.Equals("hacer las actividades del hogar") && ent.RowKey.Equals("Preparar hogar"));

    Todo result = queryResults.First();
    Console.WriteLine("{0}\t{1}\t{2}", result.RowKey, result.PartitionKey, result.Done);
}

static async Task DeleteOperationAsync(TableClient table, string partitionKey, string rowKey)
{
    table.DeleteEntity(partitionKey, rowKey);
}