using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahc.Health.Model
{
    public class User
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public Int32 Age { get; set; }
        public string HealthId { get; set; }
        public string TokenKey { get; set; }
        public string ContactNumber { get; set; }
        public String Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
