using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.DAL
{
    public class UserConfirmation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConfirmationGuid { get; set; }

        public virtual User User { get; set; }
    }
}
