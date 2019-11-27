using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.Framework.WebApi
{
    public class TokenConfig
    {
        public string TokenKey { get; set; }

        private TokenConfig()
        {
            TokenKey = "x-token";

        }
        //静态的构造单例模式
        private static TokenConfig instance = new TokenConfig();

        public static TokenConfig Instace
        {
            get
            {
                return instance;
            }
        }
    }
}
