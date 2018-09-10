using rabbitMQ.Core.Parameters;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Net.Security;
using System.Text;

namespace rabbitMQ.Core
{
    public static class Manager
    {
        public static Result Send(Send param)
        {
            var queueResult = new Result();

            var connectionFactory = new ConnectionFactory()
            {
                HostName = param.HostName,
                VirtualHost = param.VirtualHost,
                UserName = param.UserName,
                Password = param.Password
            };

            if (param.EnableSSL)
            {
                var sslOption = new SslOption
                {
                    Enabled = true,
                    ServerName = param.HostName,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors
                };

                connectionFactory.Ssl = sslOption;
            }

            if (param.Port.HasValue)
                connectionFactory.Port = param.Port.Value;
            
            try
            {
                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: param.QueueName,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var body = Encoding.UTF8.GetBytes(param.Message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: param.QueueName,
                                             basicProperties: null,
                                             body: body);
                    }
                }
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                queueResult.Exceptions.Add("Unreachable Exception: " + ex.Message);
            }
            catch (RabbitMQ.Client.Exceptions.ConnectFailureException connEx)
            {
                queueResult.Exceptions.Add("Connect Failure Exception: " + connEx.Message);
            }
            catch (RabbitMQ.Client.Exceptions.AuthenticationFailureException authEx)
            {
                queueResult.Exceptions.Add("Authentication Failure Exception: " + authEx.Message);
            }

            return queueResult;
        }

        public static Result Receive(Receive param)
        {
            var queueResult = new Result();

            var connectionFactory = new ConnectionFactory()
            {
                HostName = param.HostName,
                VirtualHost = param.VirtualHost,
                UserName = param.UserName,
                Password = param.Password
            };

            if (param.EnableSSL)
            {
                var sslOption = new SslOption
                {
                    Enabled = true,
                    ServerName = param.HostName,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors
                };

                connectionFactory.Ssl = sslOption;
            }

            if (param.Port.HasValue)
                connectionFactory.Port = param.Port.Value;

            try
            {
                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var declare = channel.QueueDeclare(queue: param.QueueName,
                                                             durable: true,
                                                             exclusive: false,
                                                             autoDelete: false,
                                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                                                
                        int downloadSize = param.DownloadSize.HasValue 
                            ? (param.DownloadSize.Value > (int)declare.MessageCount ? (int)declare.MessageCount : param.DownloadSize.Value)
                            : (int)declare.MessageCount;

                        while (downloadSize > 0)
                        {
                            BasicGetResult result = channel.BasicGet(param.QueueName, true);

                            if (result != null)
                            {
                                string data = Encoding.UTF8.GetString(result.Body);
                                queueResult.Results.Add(data);
                            }

                            downloadSize--;
                        }
                    }
                }
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                queueResult.Exceptions.Add("Unreachable Exception: " + ex.Message);
            }
            catch (RabbitMQ.Client.Exceptions.ConnectFailureException connEx)
            {
                queueResult.Exceptions.Add("Connect Failure Exception: " + connEx.Message);
            }
            catch (RabbitMQ.Client.Exceptions.AuthenticationFailureException authEx)
            {
                queueResult.Exceptions.Add("Authentication Failure Exception: " + authEx.Message);
            }

            return queueResult;
        }
    }
}