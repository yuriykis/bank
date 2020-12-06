using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transactions.Service.Messaging.Options;
using Transactions.Service.Models;
using Transactions.Service.Services;

namespace Transactions.Service.Messaging.Receiver
{
    public class TransactionUpdateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly ITransactionUpdateService _transactionUpdateService;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public TransactionUpdateReceiver(ITransactionUpdateService transactionUpdateService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _transactionUpdateService = transactionUpdateService;
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = "TransactionUpdateQueue";
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            InitializeRabbitMqListener();
        }
        
        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var transactionUpdateModel = JsonConvert.DeserializeObject<TransactionUpdateModel>(content);

                HandleMessage(transactionUpdateModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            
            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }
        private void HandleMessage(TransactionUpdateModel transactionUpdateModel)
        {
           
            _transactionUpdateService.UpdateTransactionStatus(transactionUpdateModel);
            
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

    }
}