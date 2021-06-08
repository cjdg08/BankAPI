using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ApiCredentialDTO
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
