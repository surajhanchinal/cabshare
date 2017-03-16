using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace cabshare
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var replytext = ReplyCreate(activity);
                string y = await ReplyCreate(activity);
                Activity reply = activity.CreateReply(y);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            string x = await ReplyCreate(activity);
            var response = Request.CreateResponse(x);
            return response;

            /*using (travelrecordEntities DB = new travelrecordEntities())
            {
                var z = (from b in DB.Requests select b).ToList();
                
            }*/
                
            
        }

        private static async Task<LUIS> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LUIS Data = new LUIS();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/0d2edcc2-6e71-42cd-9ea5-26953a8f2300?subscription-key=f96f048c665e40c0b30a50e790a4de2c&verbose=true&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LUIS>(JsonDataResponse);
                }
            }
            return Data;
        }
        private static async Task<string> ReplyCreate(Activity activity)
        {
            var x = await GetEntityFromLUIS(activity.Text);
            if (x.topScoringIntent.intent == "Greeting")
            {
                return "hi";
            }
            else if(x.topScoringIntent.intent == "Search")
            {
                string answer = "";
                var a = await GetEntityFromLUIS(activity.Text);
                var y = await DBquery.Clean(a);
                var z = await DBquery.dataquery(y);
                foreach(var b in z)
                {
                    answer += String.Format("name : {0}--origin : {1}--destination : {2}--date : {3}--time : {4}:{5}\n\r",b.name,b.origin.TrimEnd(),b.destination.TrimEnd(),b.date1.Value.ToShortDateString(),b.time1.Value.TotalHours.ToString(),b.time1.Value.TotalMinutes.ToString());
                }
                return answer;

            }
            else
            {
                return "i dont get it";
            }
        }
        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}