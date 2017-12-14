using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;
using Microsoft.Bot.Connector;
using dataaccess;
using System.Collections.Generic;

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
        public async Task FetchAllTasks(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var db = new BotDbContext();

            var tasks = db.OnboardingTasks.ToList();

            var msg = $"Here is what you need to do: ";

            var act = await activity;


            foreach (var t in tasks)
            {
                msg = msg + string.Format("\n {0}", t.TaskName);
            }

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Plain;
            message.Text = msg;
            message.SuggestedActions = new SuggestedActions() {
                Actions = new List<CardAction>() {
                        new CardAction(){ Title = "Do this now"},
                        new CardAction(){ Title = "Do this later"},
                        new CardAction(){ Title = "I did this already"}
                }
            };

            message.ReplyToId = context.Activity.Id;


            await context.PostAsync(message);          
            context.Wait(MessageReceived);
        }

        [LuisIntent("Fetch Next Task")]
        public async Task FetchNextIntent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var db = new BotDbContext();

            var tasks = db.OnboardingTasks.ToList();
            var nextTask = 0;

            var msg = string.Format("{0}", tasks[nextTask].TaskName);

            var reply = (await activity);

            await context.PostAsync(msg);

            context.Wait(MessageReceived);
        }

        [LuisIntent("Task Help")]
        public async Task HelpIntent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var db = new BotDbContext();

            var tasks = db.OnboardingTasks.ToList();

            var msg = $"What do you need help with?";

            await context.PostAsync(msg);

            context.Wait(MessageReceived);
        }
    }

    
}