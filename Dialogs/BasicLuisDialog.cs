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

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Plain;
            message.Text = msg;
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                        new CardAction(){ Title = "Show me more", Type = ActionTypes.PostBack, Value = "Show Me More", Text="ShowMore"}                        
                }
            };

            message.ReplyToId = context.Activity.Id;


            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Show Me More")]
        public async Task ShowMoreIntent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result) {
            var db = new BotDbContext();

            var task = db.OnboardingTasks.FirstOrDefault();
            var nextTask = 0;

            //var msg = string.Format("{0}", tasks[nextTask].TaskName);

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Plain;
            message.Text = string.Format("{0} - {1}", task.TaskName, task.TaskDescription);

            message.SuggestedActions = new SuggestedActions() {
                Actions = new List<CardAction> {
                  new CardAction{ Title = "Click here", Type = ActionTypes.OpenUrl, Value = "https://rbauction.sharepoint.com/it/pg/sf/_layouts/15/DocIdRedir.aspx?ID=U7JYW44APTCC-399-36&e=827f8be4cf294b5587983221312d20fb"}
                  }
                };
            //message.


            message.ReplyToId = context.Activity.Id;


            await context.PostAsync(message);
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

       

        [LuisIntent("Count Incomplete Tasks")]
        public async Task Couunt(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

            var msg = $"I'm guessing 3?";

            await context.PostAsync(msg);

            context.Wait(MessageReceived);
        }
    }

    
}