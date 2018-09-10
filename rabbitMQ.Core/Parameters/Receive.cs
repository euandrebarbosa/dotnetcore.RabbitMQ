namespace rabbitMQ.Core.Parameters
{
    public class Receive : Credential
    {
        public Receive(string hostName,
            string port,
            bool ssl,
            string virtualHost,
            string userName,
            string password,
            string queueName,
            int? downloadSize = null) : base()
        {
            HostName = hostName;
            EnableSSL = ssl;
            VirtualHost = virtualHost;
            UserName = userName;
            Password = password;

            if (!string.IsNullOrWhiteSpace(port))
                Port = int.Parse(port);

            QueueName = queueName;
            DownloadSize = downloadSize;
        }

        public string QueueName { get; set; }
        public int? DownloadSize { get; set; }
    }
}