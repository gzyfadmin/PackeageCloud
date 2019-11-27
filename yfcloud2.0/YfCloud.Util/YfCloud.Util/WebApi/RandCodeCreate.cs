using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.WebApi
{
    /// <summary>
    /// 产生随机字符串
    /// </summary>
    public static class RandCodeCreate
    {
        private static char[] constant =
     {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };


        private static char[] constantNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 产生纯数字随机数
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomOnlyNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="Length">长度</param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        private static Dictionary<char, string> Dictorystring = new Dictionary<char, string>() { { '0', "M" }, { '1', "F" }, { '2', "z" }, { '3', "q" },
            { '4', "P" }, { '5', "K" }, { '6', "X" }, { '7', "u" }, { '8', "L" }, { '9', "D" } };

        /// <summary>
        /// 根据数字生成字符串
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public static string CreateCodeByID(int ID)
        {
            string result = string.Empty;
            if (ID >= 100000000)
            {
                throw new Exception("ID 不能大于100000000");
            }

            foreach (var s in ID.ToString())
            {
                result += Dictorystring[s];
            }

            return result.PadLeft(8, 'H'); //不足8位 补H
        }

    }
}
