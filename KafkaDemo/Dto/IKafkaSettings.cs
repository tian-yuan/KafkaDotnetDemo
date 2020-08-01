using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Config.Net;

namespace KafkaDemo.Dto
{
    public interface IKafkaSettings : INotifyPropertyChanged
    {
        string KafkaBootStrapServers { get; set; }
        string KafkaTopics { get; set; }
    }
}
