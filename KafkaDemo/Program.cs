using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Confluent.Kafka;
using KafkaDemo.KafkaConsumer;
using KafkaDemo.Dto;
using System.IO;
using System.Threading;

using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Model;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Core.Utils;
using Com.Ctrip.Framework.Apollo.Internals;
using Com.Ctrip.Framework.Apollo.Model;
using Com.Ctrip.Framework.Apollo.Spi;


namespace KafkaDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start kafka consumer.");
            KafkaConfiguration kafkaConfiguration = new KafkaConfiguration();
            var configuration = new ConsumerConfig
            {
                GroupId = "test_consumer", // consumer group id (multiple consumers in a group)
                BootstrapServers = "localhost:9092",
                /* Consumers will (by default) automatically signal to Kafka that a message
                   has been consumed. This could be a problem if the consumer crashes after
                   it has told Kafka that the message was successfully consumed - the 
                   consumer may miss out on processing a specific message.
                 */
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            const string topic = "one";

            var kafkaConsumer = new MessageConsumer();
            kafkaConsumer.Listen(ProcessMessage, configuration, topic);
        }

        public static void ProcessMessage(Message<Null, string> message)
        {
            Console.WriteLine(message.Value);
        }

        public static void WatchConfig()
        {
            IConfig config = await ApolloConfigurationManager.GetAppConfig(); //config instance is singleton for each namespace and is never null
            config.ConfigChanged += new ConfigChangeEvent(OnChanged);

        }

        private void OnChanged(object sender, ConfigChangeEventArgs changeEvent)
        {
            Console.WriteLine("Changes for namespace {0}", changeEvent.Namespace);
            foreach (string key in changeEvent.ChangedKeys)
            {
                ConfigChange change = changeEvent.GetChange(key);
                Console.WriteLine("Change - key: {0}, oldValue: {1}, newValue: {2}, changeType: {3}", change.PropertyName, change.OldValue, change.NewValue, change.ChangeType);
            }
        }
    }
}
