using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataaccess
{
    public class BotDbContext: DbContext
    {
        public BotDbContext(): base("ConversationDataContextConnectionString")
        {

        }


        public virtual DbSet<OnboardingTask> OnboardingTasks  { get; set; }

        
    }
}
