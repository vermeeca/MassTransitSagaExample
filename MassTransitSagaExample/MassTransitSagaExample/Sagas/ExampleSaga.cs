using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.Saga;
using Magnum.StateMachine;
using MassTransitSagaExample.Messages;
using MassTransit;

namespace MassTransitSagaExample.Sagas
{
    public class ExampleSaga
        : SagaStateMachine<ExampleSaga>,
        ISaga
    {
        static ExampleSaga()
        {
            Define(() =>
                {
                    Initially(
                        When(WatchSignalReceived)
                            .Then((saga, message) => saga.StartWatchingForMessages(message))
                            .TransitionTo(Watching)
                        );

                    During(Watching,
                        When(Message1Received)
                            .Then((saga, message) => saga.Write(message)),
                        When(Message2Received)
                            .Then((saga, message) => saga.Write(message))
                            );

                });
        }

        public ExampleSaga(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public IServiceBus Bus { get; set; }


        private void Write(object message)
        {
            Console.WriteLine("{0} received", message);
        }

        private void StartWatchingForMessages(StartWatchingMessage message)
        {
            Console.WriteLine("Received Watch Signal");
        }

        public Guid CorrelationId { get; set; }

        public static State Initial { get; set; }
        public static State Watching { get; set; }
        public static State Completed { get; set; }


        public static Event<StartWatchingMessage> WatchSignalReceived { get; set; }
        public static Event<MyMessage1> Message1Received { get; set; }
        public static Event<MyMessage2> Message2Received { get; set; }

        
    }
}
