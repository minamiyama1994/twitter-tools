using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CoreTweet;
using CoreTweet.Streaming;

namespace tweet
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new XmlSerializer(typeof(Tokens));
            var file = args [ 0 ] + ".xml";
            Tokens tokens;
            if (File.Exists(file))
            {
                using (var y = File.OpenRead(file))
                {
                    tokens = x.Deserialize(y) as Tokens;
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
                tokens = se.GetTokens(Console.ReadLine());
                using (var y = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    x.Serialize(y, tokens);
                }
            }
            args
            tokens.Statuses.Update(status => args[1] + args[2]);
        }
    }
}
