// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;

var factory = new ConnectionFactory{ HostName = "localhost" };

using var connection = await factory.CreateConnectionAsync();

using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("letterbox", durable: false, exclusive: false, autoDelete: false, arguments: null);

var message = $"Hello, World! + {RandomNumberGenerator.GetInt32(0, 1500)}";

var encodeMessage = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: "", routingKey: "letterbox", body: new ReadOnlyMemory<byte>(encodeMessage), cancellationToken: CancellationToken.None );

Console.WriteLine("Published message: {0}", message);