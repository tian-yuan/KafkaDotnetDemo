using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Confluent.Kafka;

namespace KafkaDemo.KafkaConsumer
{
    public interface IMessageConsumer
    {
        /// <summary>
        /// Listen for new messages on a topic (defined in <see cref="topic"/>) and process them
        /// using the action provided as <see cref="processMessage"/>. The consumer's properties
        /// are set via <see cref="configuration"/>.
        /// </summary>
        /// <param name="processMessage">An action to execute when a message is received.</param>
        /// <param name="configuration">The configuration to use when creating an instance of the consumer.</param>
        /// <param name="topic">The topic to listen to.</param>
        void Listen();
    }
}
