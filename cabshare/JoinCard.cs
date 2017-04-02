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
                string card = String.Format("DATE : {0}\r\nTIME : {1}\r\nFROM : {2}\r\nTO : {3}\r\nVACANCY : {4}", b.date1.Value.ToShortDateString(), b.time1.ToString(), b.origin.TrimEnd(), b.destination.TrimEnd(),b.MAXNO);
                Activity reply = activity.CreateReply("");
                reply.Attachments = new List<Attachment>();
                List<CardImage> cardImages = new List<CardImage>();

                List<CardAction> cardButtons = new List<CardAction>();
                CardAction namebutton = new CardAction()
                {
                    Value = "https://www.facebook.com/"+b.fbid,
                    Type = "openUrl",
                    Title = b.name
                };
                cardButtons.Add(namebutton);
                CardAction plButton = new CardAction()
                {
                    Value = JsonConvert.SerializeObject(b),
                    Type = "postBack",
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