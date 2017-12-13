using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using dataaccess;
using System.Linq;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            var db = new BotDbContext();

            var tasks = db.OnboardingTasks.ToList();

            var msg = $"Here is what you need to do: ";

            foreach (var t in tasks) {
                msg = msg + string.Format("\n {0}",t.TaskName);
            }

            await context.PostAsync(msg); //
            context.Wait(MessageReceived);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "MyIntent" with the name of your newly created intent in the following handler
        [LuisIntent("MyIntent")]
        public async Task MyIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the MyIntent intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }
    }
}