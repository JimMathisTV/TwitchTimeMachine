using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TwitchTimeMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter streamer name: ");
            var streamer = Console.ReadLine();

            Console.WriteLine("Enter vod ID from URL: ");
            var vodID = Console.ReadLine();

            Console.WriteLine("Enter timestamp (YYYY-MM-DD HH:MM:SS) UTC: ");
            var timestamp = Console.ReadLine();

            if (!DateTime.TryParse(timestamp, out var dateTime))
            {
                Console.WriteLine("Unable to read timestamp, please use format listed above");
                return;
            }

            var secondsSinceEpoch = (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

            var preHash = streamer + "_" + vodID + "_" + secondsSinceEpoch;

            var hash = Hash(preHash);

            var result = hash.Substring(0, 20) + "_" + preHash;

            Console.WriteLine();
            Console.WriteLine("https://vod-secure.twitch.tv/" + result + "/chunked/index-dvr.m3u8");
        }

        public static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
