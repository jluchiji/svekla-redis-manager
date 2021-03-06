﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using ServiceStack.Text;
using Svekla.Utilities; 

namespace Svekla.ServiceStack
{
    public class SveklaRedisClient : RedisClient
    {
        // fields
        private Version _ver = null;

        // constructors
        public SveklaRedisClient(String host)
            : base(host) { }
        public SveklaRedisClient(String host, Int32 port)
            : base(host, port) { }
        public SveklaRedisClient(String host, Int32 port, String password)
            : base(host, port, password) { }
        public SveklaRedisClient(Uri uri)
            : base(uri) { }


        // properties
        public Version ServerVersion
        {
            get
            {
                if (_ver == null)
                    _ver = new Version(Info.GetOrDefault("redis_version", "1.0.0"));
                return _ver;
            }
        }
        
        public RedisClientInfo[] GetClientList()
        {
            List<RedisClientInfo> rcis = new List<RedisClientInfo>();
            String response = SendExpectString(RedisCommandsEx.CLIENT, "LIST".ToUtf8Bytes());
            String[] sresponse = response.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string r in sresponse)
                rcis.Add(RedisClientInfo.Parse(r));
            return rcis.ToArray();
        }

        public void KillClientConnection(RedisClientInfo rci)
        {
            SendExpectSuccess(RedisCommandsEx.CLIENT, "KILL".ToUtf8Bytes(), rci.Address.ToUtf8Bytes());
        }
    }
}
