using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// requires Confluent.Kafka nuget package
using Confluent.Kafka;


namespace KafkaDemo.KafkaConsumer
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Listen(Action<Message<Null, string>> processMessage, ConsumerConfig configuration, string topic)
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

