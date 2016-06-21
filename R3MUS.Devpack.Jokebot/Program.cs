using R3MUS.Devpack.Slack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace R3MUS.Devpack.Jokebot
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = new XmlDocument();
            var byteLength = 0;
            string joketext = string.Empty;

            do
            {
                doc.Load(Properties.Settings.Default.RSSFeedUrl);
                var rssNode = doc.SelectNodes("rss/channel/item").Item(0);

                joketext = rssNode.InnerText.Split(new[] { "http" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if(joketext.IndexOf("...") > -1)
                {
                    joketext = joketext.Split(new[] { "..." }, StringSplitOptions.RemoveEmptyEntries)[1];
                }

                joketext = string.Concat("@bodomatix: ", joketext.Replace("<br />", ""));
                byteLength = ASCIIEncoding.ASCII.GetByteCount(joketext);
            }
            while (byteLength > 400);

            
            var message = new MessagePayload();
            message.Attachments = new List<MessagePayloadAttachment>();
            message.Attachments.Add(new MessagePayloadAttachment() {
                Text = joketext
            });

            Plugin.SendToRoom(message, Properties.Settings.Default.Channel, Properties.Settings.Default.SlackToken, Properties.Settings.Default.BotName);
        }
    }
}
