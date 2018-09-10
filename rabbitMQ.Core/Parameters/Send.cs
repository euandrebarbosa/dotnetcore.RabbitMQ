namespace rabbitMQ.Core.Parameters
{
    public class Send : Credential
    {
         public Send(string hostName, 
            string port, 
            bool ssl, 
            string virtualHost, 
            string userName, 
            string password, 
            string queueName, 
            string message)
        {
            HostName = hostName;
            EnableSSL = ssl;
            VirtualHost = virtualHost;
            UserName = userName;
            Password = password;

            if (!string.IsNullOrWhiteSpace(port))
                Port = int.Parse(port);

            QueueName = queueName;
            Message = message;
        }

        public string QueueName { get; set; }
        public string Message { get; set; }
    }
}