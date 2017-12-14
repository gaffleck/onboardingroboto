using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataaccess
{
    public class OnboardingTask
    {
        public Guid ID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskOwner { get; set; }
        public bool TaskComplete { get; set; }

    }
}
