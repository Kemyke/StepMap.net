using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.ServiceContracts.DTO
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public int GoodPoint { get; set; }
        [DataMember]
        public int BadPoint { get; set; }
        [DataMember]
        public IList<Step> FinishedSteps { get; set; }
        [DataMember]
        public Step NextStep { get; set; }

    }
}
