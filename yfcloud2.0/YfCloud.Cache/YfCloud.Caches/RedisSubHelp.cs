using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Redis;

namespace YfCloud.Caches
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisSubHelp
    {
        private RedisSubscription _sub;


        public RedisSubHelp(Action<string, string> OnMessage)
        {
            var config = new RedisConfigs();
             var  _redisClient = new RedisClient(config.Host, config.Port);
            _sub = new RedisSubscription(_redisClient);
            _sub.OnMessage += OnMessage;
        }

        public RedisSubHelp(RedisCache client,Action<string, string> OnMessage)
        {
            _sub = new RedisSubscription(client.Client);
            _sub.OnMessage += OnMessage;
        }

        public void SubscribeToChannels(params string[] channels)
        {
            _sub.SubscribeToChannels(channels);
            //_sub.SubscribeToChannels("__keyevent@0__:expired");
        }


    }
}
