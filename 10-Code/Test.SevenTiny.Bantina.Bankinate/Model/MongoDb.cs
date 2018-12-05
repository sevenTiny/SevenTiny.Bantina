﻿using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Bantina.Configuration;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.SevenTiny.Bantina.Bankinate.Model
{
    [DataBase("local")]
    public class MongoDb : MongoDbContext<MongoDb>
    {
        public MongoDb() : base(ConnectionStrings.Get("mongodb39911"))
        {

        }
    }

    public class ConnectionStrings: ConfigBase<ConnectionStrings> {
        public string Key { get; set; }
        public string Value { get; set; }

        private static Dictionary<string, string> dictionary;

        private static void Initial()
        {
            dictionary = new Dictionary<string, string>();
            foreach (var item in Configs)
            {
                dictionary.AddOrUpdate(item.Key, item.Value);
            }
        }

        public static string Get(string key)
        {
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            Initial();
            return dictionary.SafeGet(key);
        }
    }
}