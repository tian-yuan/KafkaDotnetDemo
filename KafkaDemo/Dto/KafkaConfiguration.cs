using Config.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo.Dto
{
    class KafkaConfiguration
    {
        IKafkaSettings kafkaSettiongs;
        System.Timers.Timer timer;

        public KafkaConfiguration()
        {
            kafkaSettiongs = new ConfigurationBuilder<IKafkaSettings>().Build();
            kafkaSettiongs.PropertyChanged += (sender, e) =>
            {
                Console.WriteLine("Current property name : " + e.PropertyName);
                Console.WriteLine("Current kafka bootstrap servers config : " + kafkaSettiongs.KafkaBootStrapServers);
                Console.WriteLine("Current kafka topics config : " + kafkaSettiongs.KafkaTopics);
            };
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
            Console.WriteLine("Begin reload configs.");
            IKafkaSettings temp = new ConfigurationBuilder<IKafkaSettings>().UseAppConfig().Build();
            Console.WriteLine("Temp kafka bootstrap servers config : " + temp.KafkaBootStrapServers);
            Console.WriteLine("Temp kafka topics config : " + temp.KafkaTopics);
            kafkaSettiongs.KafkaBootStrapServers = temp.KafkaBootStrapServers;
            kafkaSettiongs.KafkaTopics = temp.KafkaTopics;
        }
    }
}

