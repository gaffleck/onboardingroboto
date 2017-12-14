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
            //var db = new BotDbContext();

            //var tasks = db.OnboardingTasks.ToList();

            var msg = $"What do you need help with?";

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Plain;
            message.Text = msg;
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments.Add(new Attachment()
            {
                ContentUrl = "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png",
                ContentType = "image/png",
                Name = "Bender_Rodriguez.png"
            });

            message.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>() {
                    new CardAction(){ Type="imBack", Title = "See remaining tasks", Value="What do I have left to do?"},
                    new CardAction(){ Title = "Do this later"},
                    new CardAction(){ Title = "I did this already"}
                }
            };
            message.ReplyToId = context.Activity.Id;

            await context.PostAsync(message);

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



//THIS IS A REGULAR MESSAGE WITH AN ATTACHMENT
/*
    var message = context.MakeMessage();
    message.TextFormat = TextFormatTypes.Plain;
    message.Text = msg;
    message.Attachments.Add(new Attachment()
    {
        ContentUrl = "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png",
        ContentType = "image/png",
        Name = "Bender_Rodriguez.png"
    });

    message.SuggestedActions = new SuggestedActions()
    {
        Actions = new List<CardAction>() {
                    new CardAction(){ Title = "Do this now"},
                    new CardAction(){ Title = "Do this later"},
                    new CardAction(){ Title = "I did this already"}
                }
    };
    message.ReplyToId = context.Activity.Id;
*/



//THIS IS A HERO CARD MESSAGE - HAS A CAROUSEL OF CONTENT AND AN ACTION FOR EACH ONE
/*
    var message = context.MakeMessage();
    message.AttachmentLayout = AttachmentLayoutTypes.Carousel; //SET MESSAGE TYPE
    message.Attachments = new List<Attachment>(); //CREATE A LIST OF ATTACHMENTS

    Dictionary<string, string> cardContentList = new Dictionary<string, string>();
    cardContentList.Add("Bender", "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png");
    cardContentList.Add("Adventure Time", "https://upload.wikimedia.org/wikipedia/en/3/37/Adventure_Time_-_Title_card.png?1513234845629");
    cardContentList.Add("Ghostsrhimp", "https://upload.wikimedia.org/wikipedia/en/7/7c/NehruvianDOOM.jpg?1513234947359");

    foreach (KeyValuePair<string, string> cardContent in cardContentList)
    {
        List<CardImage> cardImages = new List<CardImage>();
        cardImages.Add(new CardImage(url: cardContent.Value));

        List<CardAction> cardButtons = new List<CardAction>();

        CardAction plButton = new CardAction()
        {
            Value = $"https://en.wikipedia.org/wiki/{cardContent.Key}",
            Type = "openUrl",
            Title = "WikiPedia Page"
        };

        cardButtons.Add(plButton);

        HeroCard plCard = new HeroCard()
        {
            Title = $"I'm a hero card about {cardContent.Key}",
            Subtitle = $"{cardContent.Key} Wikipedia Page",
            Images = cardImages,
            Buttons = cardButtons
        };

        Attachment plAttachment = plCard.ToAttachment();
        message.Attachments.Add(plAttachment);
    }
*/




//THIS IS A HERO CARD MESSAGE - HAS A CAROUSEL OF CONTENT AND AN ACTION FOR EACH ONE
/*
    var message = context.MakeMessage();
    message.AttachmentLayout = AttachmentLayoutTypes.List; //SET MESSAGE TYPE (THUMBNAIL LIST IN THIS CASE)
    message.Attachments = new List<Attachment>(); //CREATE A LIST OF ATTACHMENTS

    Dictionary<string, string> cardContentList = new Dictionary<string, string>();
    cardContentList.Add("Bender", "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png");
    cardContentList.Add("Adventure Time", "https://upload.wikimedia.org/wikipedia/en/3/37/Adventure_Time_-_Title_card.png?1513234845629");
    cardContentList.Add("Ghostsrhimp", "https://upload.wikimedia.org/wikipedia/en/7/7c/NehruvianDOOM.jpg?1513234947359");

    foreach (KeyValuePair<string, string> cardContent in cardContentList)
    {
        List<CardImage> cardImages = new List<CardImage>();
        cardImages.Add(new CardImage(url: cardContent.Value));

        List<CardAction> cardButtons = new List<CardAction>();

        CardAction plButton = new CardAction()
        {
            Value = $"https://en.wikipedia.org/wiki/{cardContent.Key}",
            Type = "openUrl",
            Title = "WikiPedia Page"
        };

        cardButtons.Add(plButton);

        ThumbnailCard plCard = new ThumbnailCard()
        {
            Title = $"I'm a hero card about {cardContent.Key}",
            Subtitle = $"{cardContent.Key} Wikipedia Page",
            Images = cardImages,
            Buttons = cardButtons
        };

        Attachment plAttachment = plCard.ToAttachment();
        message.Attachments.Add(plAttachment);
    }
*/