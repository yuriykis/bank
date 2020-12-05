using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Service.Messaging.Options;
using Accounts.Service.Models;
using Accounts.Service.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Accounts.Service.Messaging.Receiver
{
    public class AccountsAmountUpdateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IAccountUpdateService _accountUpdateService;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public AccountsAmountUpdateReceiver(IAccountUpdateService accountUpdateService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _accountUpdateService = accountUpdateService;
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
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
                var accountUpdateModel = JsonConvert.DeserializeObject<AccountUpdateModel>(content);

                HandleMessage(accountUpdateModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            
            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }
        private void HandleMessage(AccountUpdateModel accountUpdateModel)
        {
            if (accountUpdateModel.Message == "ExecuteTransaction")
            {
                _accountUpdateService.UpdateAccountsAmount(accountUpdateModel);
            }
            else if(accountUpdateModel.Message == "DeleteAccount")
            {
                _accountUpdateService.DeleteAccount(accountUpdateModel);
            }
            else if(accountUpdateModel.Message == "CreateAccount")
            {
                _accountUpdateService.CreateAccount(accountUpdateModel);
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

    }
}