using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.DAL
{
    //TODO: remove FinishedSteps and NexStep, only Steps needed
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public DateTime StartDate { get; set; }
        public int GoodPoint { get; set; }
        public int BadPoint { get; set; }
        public int NextStepId { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Step> FinishedSteps { get; set; }
        public virtual Step NextStep { get; set; }
        public virtual User User { get; set; }
    }
}
