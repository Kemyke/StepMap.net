using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastLogin { get; set; }
        [Column("UserStateId", TypeName = "int")]
        public UserState UserState { get; set; }
        [Column("UserRoleId", TypeName = "int")]
        public UserRole UserRole { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
