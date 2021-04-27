using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaunaDB.Client;
using FaunaDB.Types;

using static FaunaDB.Query.Language;

namespace fauna_csharp_streaming_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var faunaDomain = "https://" + Environment.GetEnvironmentVariable("FAUNA_DOMAIN");
            var faunaSecret = Environment.GetEnvironmentVariable("FAUNA_ROOT_KEY");

            Run(faunaDomain, faunaSecret).Wait();
        }

        private static async Task Run(string endpoint, string secret)
        {
            var client = new FaunaClient(
                endpoint: endpoint,
                secret: secret
            );

            var docRef = Get(Ref(Collection("Categories"), "287626591762121219"));

            // create a data provider
            var provider = await client.Stream(docRef);

            // we use this object to signalize a completion of
            // asynchronous operation for the current example
            var done = new TaskCompletionSource<object>();

            // a collection for storage of incoming events from the provider
            List<Value> events = new List<Value>();

            // creating a subscriber
            // it takes 3 lambdas that describe the following:
            // - next event processing
            // - error processing
            // - completion processing
            var monitor = new StreamingEventMonitor(
                value =>
                {
                    events.Add(value);
                    Console.WriteLine(value);
                    if (events.Count == 4)
                    {
                        provider.Complete();
                    }
                    else
                    {
                        provider.RequestData();
                    }
                },
                ex => { done.SetException(ex); },
                () => { done.SetResult(null); }
            );

            // subscribe to data provider
            monitor.Subscribe(provider);

            // blocking until we receive all the events
            await done.Task;

            // clear the subscription
            monitor.Unsubscribe();
        }
    }
}
