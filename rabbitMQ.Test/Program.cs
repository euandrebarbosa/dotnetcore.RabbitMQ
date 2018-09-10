using core = rabbitMQ.Core;
using coreParameters = rabbitMQ.Core.Parameters;

using System;

namespace rabbitMQ.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region SEND

            Console.WriteLine("Hello! Lets send an item to queue!");

            var resultsSend = core.Manager.Send(
                    new coreParameters.Send(
                        "", // hostname
                        "", // port
                        false, // SSL
                        "", // virtualhost
                        "", // username
                        "", // password
                        "", // queue name    
                        "My test message!" // message
                    ));

            foreach(var r in resultsSend.Results)
                Console.WriteLine($"Send Result: {r}");

            foreach(var e in resultsSend.Exceptions)
                Console.WriteLine($"Send Exception: {e}");

            #endregion 

            #region RECEIVE
            
            Console.WriteLine("Lets recieve 1 item!");

            var resultsReceive = core.Manager.Receive(
                    new coreParameters.Receive(
                        "", // hostname
                        "", // port
                        false, // SSL
                        "", // virtualhost
                        "", // username
                        "", // password
                        "", // queue name   
                        1 // number of items to download
                    ));

             foreach(var r in resultsReceive.Results)
                Console.WriteLine($"Receive Result: {r}");

            foreach(var e in resultsReceive.Exceptions)
                Console.WriteLine($"Receive Exception: {e}");

            #endregion

            Console.ReadKey();
        }
    }
}