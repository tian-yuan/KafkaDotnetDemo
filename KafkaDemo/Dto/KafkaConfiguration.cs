using Config.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo.Dto
{
    class KafkaConfiguration
    {
        public delegate void OnConfigChanged(string kafkaBootstrapServers, string kafkaTopics);
        String KafkaBootStrapServers;
        String KafkaTopics;
        System.Timers.Timer timer;
        public event OnConfigChanged ConfigChanged;

        public KafkaConfiguration()
        {
            InitTimer();
        }

        private void InitTimer()
        {
            //设置定时间隔(毫秒为单位)
            int interval = 1000;
            timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerUp);
        }

        private void TimerUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            String tempKfkBootstrapservers = config.AppSettings.Settings["KafkaBootStrapServers"].Value;
            String tempKfkTopic = config.AppSettings.Settings["KafkaTopics"].Value;
            if (KafkaBootStrapServers != tempKfkBootstrapservers 
                || KafkaTopics != tempKfkTopic)
            {
                Console.WriteLine("Kafka bootstrap servers is changed, old : " + KafkaBootStrapServers + ", new : " + tempKfkBootstrapservers);
                Console.WriteLine("Kafka topics is changed, old : " + KafkaTopics + ", new : " + tempKfkTopic);
                KafkaBootStrapServers = tempKfkBootstrapservers;
                KafkaTopics = tempKfkTopic;
                ConfigChanged.Invoke(KafkaBootStrapServers, KafkaTopics);
            }
        }
    }
}