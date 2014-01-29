using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CoreTweet;
using CoreTweet.Streaming;

namespace sync_follow
{
    class Program
    {
        private static XmlSerializer x_ = null;
        private static XmlSerializer x
        {
            get
            {
                if (x_ == null)
                {
                    x_ = new XmlSerializer(typeof(Tokens));
                }
                return x_;
            }
        }
        private static Dictionary<string, Tokens> tokens_ = null;
        private static Tokens users(string sn)
        {
            if (!tokens_.ContainsKey(sn))
            {
                var file = sn + ".xml";
                if (File.Exists(file))
                {
                    using (var y = File.OpenRead(file))
                    {
                        tokens_.Add(sn, x.Deserialize(y) as Tokens);
                    }
                }
                else
                {
                    Console.Write("CK> ");
                    var CK = Console.ReadLine();
                    Console.Write("CS> ");
                    var CS = Console.ReadLine();
                    var se = OAuth.Authorize(CK, CS);
                    Console.WriteLine(se.AuthorizeUri);
                    Console.Write("pin> ");
                    tokens_.Add(sn, se.GetTokens(Console.ReadLine()));
                    using (var y = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        x.Serialize(y, tokens_[sn]);
                    }
                }
            }
            return tokens_[sn];
        }
        static void Main(string[] args)
        {
            var PG_nonen = users("PG_nonen");
            var masakazu_sub0 = users("masakazu_sub0");
            var main_follow = PG_nonen.Friends.List().ToList();
            var sub_follow = masakazu_sub0.Friends.List().ToList();
            var list = PG_nonen.Lists.Show(slug => "masakazu-following", owner_screen_name => "PG_nonen");
            foreach (var follow in main_follow)
            {
                if ( !sub_follow.Contains(follow))
                {
                    masakazu_sub0.Friendships.Update();
                }
            }
        }
    }
}
