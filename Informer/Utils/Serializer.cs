using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Informer.Utils
{
    public static class Serializer
    {

        public static String Serialize(object data) 
        {
            if (data == null) 
            {
                return null;
            }

            /*
            String result = "";


            foreach (KeyValuePair<String, String> kvp in data) 
            {
                result += kvp.Key + "=" + kvp.Value + "|";
            }


            return result;
            */

            return JsonConvert.SerializeObject(data);
        }

        public static T Deserialize<T>(String data)
        {
            /*
            Dictionary<String, String> result = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(data)) 
            {
                foreach (String kvp in data.Split('|')) 
                {
                    if (!String.IsNullOrEmpty(kvp) && kvp.Contains("=")) 
                    {
                        String[] d = kvp.Split('=');
                        result.Add(d[0], d[1]);    
                    }
                }
            }

            return result;
            */

            return JsonConvert.DeserializeObject<T>(data);
        }

    }
}
