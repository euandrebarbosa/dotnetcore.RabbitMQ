using System.Collections.Generic;

namespace rabbitMQ.Core.Parameters
{
    public class Result
    {
        public Result()
        {
            Results = new List<string>();
            Exceptions = new List<string>();
        }

        public List<string> Results { get; set; }
        public List<string> Exceptions { get; set; }
    }
}