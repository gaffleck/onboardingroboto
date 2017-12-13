using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;
using Microsoft.Bot.Connector;
using dataaccess;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
            
        }

        //public Activity Activity { get; set; }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
           

            await context.PostAsync("What do you want to do?"); //
            context.Wait(MessageReceived);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "MyIntent" with the name of your newly created intent in the following handler
        [LuisIntent("Fetch Tasks")]
        public async Task MyIntent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var db = new BotDbContext();

            var tasks = db.OnboardingTasks.ToList();

            var msg = $"Here is what you need to do: ";

            foreach (var t in tasks)
            {
                msg = msg + string.Format("\n {0}", t.TaskName);
            }
            //IMessageActivity act = await activity;
            //var reply = act.CreateReply();


            //reply.Type = ActivityTypes.Message;
            //reply.TextFormat = TextFormatTypes.Plain;

            //reply.SuggestedActions = new SuggestedActions()
            //{
            //    Actions = new List<CardAction>()
            //   {
            //    new CardAction(){ Title = "Blue", Type=ActionTypes.ImBack, Value="Blue" },
            //    new CardAction(){ Title = "Red", Type=ActionTypes.ImBack, Value="Red" },
            //    new CardAction(){ Title = "Green", Type=ActionTypes.ImBack, Value="Green" }
            //}
            //};

            //await context.PostAsync(reply); //


            await context.PostAsync(msg); //
                        
            context.Wait(MessageReceived);
        }
    }

    
}