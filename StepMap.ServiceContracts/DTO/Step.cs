using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.ServiceContracts.DTO
{
    [DataContract]
    public class Step
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Deadline { get; set; }
        [DataMember]
        public DateTime? FinishDate { get; set; }
        [DataMember]
        public int SentReminder { get; set; }
    }
}
