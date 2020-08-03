using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

// requires Confluent.Kafka nuget package
using Confluent.Kafka;


namespace KafkaDemo.KafkaConsumer
{
    public class MessageConsumer : IMessageConsumer
    {
        Thread thread = new Thread(new ParameterizedThreadStart(Run));
        Action<Message<Ignore, string>> processMessage;
        ConsumerConfig configuration;
        string topic;
        static void Run(object obj)
        {
            MessageConsumer messageConsumer = obj as MessageConsumer;
            messageConsumer.Listen();
        }
        public void Start(Action<Message<Ignore, string>> processMessage, ConsumerConfig configuration, string topic)
        {
            this.processMessage = processMessage;
            this.configuration = configuration;
            this.topic = topic;
            thread.Start(this);
        }

        public void Listen()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(configuration).Build())
            {
                consumer.Subscribe(topic);
                try
                {
                    while(true)
                    {
                        try
                        {
                            var cr = consumer.Consume();
                            Console.WriteLine($"Consumed message '{cr.Value}' at : '{cr.TopicPartitionOffset}'.");
                            processMessage(cr.Message);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}

