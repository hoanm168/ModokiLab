using AsyncOAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModokiLab.Models
{
    static class Setting
    {
        static readonly string path = "tokenStore.json";

        public static bool IsExist
        {
            get { return File.Exists(path); }
        }

        public static void SaveAccessToken(AccessToken token)
        {
            File.WriteAllText(path, DynamicJson.Serialize(token));
        }

        public static AccessToken LoadAccessToken()
        {
            var json = DynamicJson.Parse(File.ReadAllText(path));
            return new AccessToken(json.Key, json.Secret);
        }
    }
}
