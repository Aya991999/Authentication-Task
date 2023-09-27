using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helpers
{
    public class JWT
    {
        public string key { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public double DurationInDays { get; set; }
    }
}
