namespace rabbitMQ.Core.Parameters
{
    public class Credential
    {
        public string HostName { get; set; }
        public int? Port { get; set; }
        public bool EnableSSL { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}