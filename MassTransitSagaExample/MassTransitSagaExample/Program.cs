using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using MassTransitSagaExample.Sagas;
using MassTransit.Saga;
using MassTransitSagaExample.Messages;
using Magnum;
using Magnum.Extensions;

namespace MassTransitSagaExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(bus =>
                {
                    bus.UseMsmq();
                    bus.SetCreateMissingQueues(true);
                    bus.ReceiveFrom("msmq://localhost/MassTransitSagaExample");
                    bus.Subscribe(sub =>
                        {
                            sub.Saga<ExampleSaga>(new InMemorySagaRepository<ExampleSaga>());
                        });
                });

            var correlationId = Guid.NewGuid();

            ThreadUtil.Sleep(3.Seconds());


            Console.WriteLine("Publishing a message");
            Bus.Instance.Publish(new MyMessage1() { CorrelationId = correlationId });

            Console.WriteLine("Press enter to start watching");
            Console.ReadLine();

            Bus.Instance.Publish(new StartWatchingMessage() { CorrelationId = correlationId });

            Console.WriteLine("Publishing a message1");
            Bus.Instance.Publish(new MyMessage1() { CorrelationId = correlationId });

            Console.WriteLine("Publishing a message2");
            Bus.Instance.Publish(new MyMessage2() { CorrelationId = correlationId });

            ThreadUtil.Sleep(3.Seconds());

            Console.WriteLine("I was expecting just one Message1 message to be received by the saga.");

            Console.WriteLine("Press enter to close");
            Console.ReadLine();

        }
    }
}
