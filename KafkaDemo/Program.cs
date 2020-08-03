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
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Core.Utils;
using Com.Ctrip.Framework.Apollo.Internals;
using Com.Ctrip.Framework.Apollo.Spi;


namespace KafkaDemo
{
    internal class Program
    {
        static Dictionary<string, MessageConsumer> consumerMap = new Dictionary<string, MessageConsumer>();
        static void Main(string[] args)
        {
            Console.WriteLine("Start kafka consumer.");
            KafkaConfiguration kafkaConfiguration = new KafkaConfiguration();
            kafkaConfiguration.ConfigChanged += OnKafkaConfigChanged;
            //IConfig config = ApolloConfigurationManager.GetAppConfig().ConfigureAwait(false).GetAwaiter().GetResult(); //config instance is singleton for each namespace and is never null
            //config.ConfigChanged += new ConfigChangeEvent(OnChanged);
            var configuration = new ConsumerConfig
            {
                GroupId = "test_consumer", // consumer group id (multiple consumers in a group)
                BootstrapServers = "10.242.202.163:9092",
                /* Consumers will (by default) automatically signal to Kafka that a message
                   has been consumed. This could be a problem if the consumer crashes after
                   it has told Kafka that the message was successfully consumed - the 
                   consumer may miss out on processing a specific message.
                 */
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            const string topic = "one";
            //Thread.Sleep(10 * 60 * 1000);
            var kafkaConsumer = new MessageConsumer();
            kafkaConsumer.Start(ProcessMessage, configuration, topic);
        }

        public static void ProcessMessage(Message<Ignore, string> message)
        {
            Console.WriteLine(message.Value);
        }

        private static void OnChanged(object sender, ConfigChangeEventArgs changeEvent)
        {
            foreach (string key in changeEvent.ChangedKeys)
            {
                ConfigChange change = changeEvent.GetChange(key);
                Console.WriteLine("Change - key: {0}, oldValue: {1}, newValue: {2}, changeType: {3}", change.PropertyName, change.OldValue, change.NewValue, change.ChangeType);
            }
        }

        private static void OnKafkaConfigChanged(string kafkaBootstrapServers, string kafkaTopics)
        {
            Console.WriteLine("On kafka config changed, " + kafkaBootstrapServers + ", " + kafkaTopics);
            var kafkaTopicList = kafkaTopics.Split(',');
            for (int i = 0; i < kafkaTopicList.Length; i++)
            {
                if (!consumerMap.ContainsKey(kafkaTopicList[i]))
                {
                    string topic = kafkaTopicList[i];
                    var configuration = new ConsumerConfig
                    {
                        GroupId = "test_consumer" + topic, // consumer group id (multiple consumers in a group)
                        BootstrapServers = kafkaBootstrapServers,
                        /* Consumers will (by default) automatically signal to Kafka that a message
                        has been consumed. This could be a problem if the consumer crashes after
                        it has told Kafka that the message was successfully consumed - the 
                        consumer may miss out on processing a specific message.
                        */
                        EnableAutoCommit = true,
                        AutoOffsetReset = AutoOffsetReset.Latest
                    };

                    var kafkaConsumer = new MessageConsumer();
                    kafkaConsumer.Start(ProcessMessage, configuration, topic);
                    consumerMap.Add(topic, kafkaConsumer);
                }
            }
        }
    }
}
