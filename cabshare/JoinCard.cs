using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace cabshare
{
    public class JoinCard
    {
        public static async Task<int> show(Activity activity,ConnectorClient connector,string message)
        {
            Activity reply = activity.CreateReply(message);
            await connector.Conversations.ReplyToActivityAsync(reply);
            return 1;
        }
        public static async Task<int> Cards(Activity activity,ConnectorClient connector,List<Request> requests)
        {
            foreach(var b in requests)
            {
                string card = String.Format("Date : {0}\r\nTime : {1}\r\nFrom : {2}\r\nTo : {3}\r\nMembers :", b.date1.Value.ToShortDateString(), b.time1.ToString(), b.origin.TrimEnd(), b.destination.TrimEnd());
                Activity reply = activity.CreateReply("");
                reply.Attachments = new List<Attachment>();
                List<CardImage> cardImages = new List<CardImage>();

                List<CardAction> cardButtons = new List<CardAction>();
                CardAction plButton = new CardAction()
                {
                    Value = JsonConvert.SerializeObject(b),
                    Type = "imBack",
                    Title = "Join Pool"
                };
                cardButtons.Add(plButton);
                HeroCard plCard = new HeroCard()
                {

                    Text = card,
                    Images = cardImages,
                    Buttons = cardButtons
                };
                Attachment plAttachment = plCard.ToAttachment();
                reply.Attachments.Add(plAttachment);
                await connector.Conversations.ReplyToActivityAsync(reply);
                
            }
            return 1;
        }
    }
}