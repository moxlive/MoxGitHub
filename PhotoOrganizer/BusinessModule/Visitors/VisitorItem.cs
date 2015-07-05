using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoOrganizer.BusinessModule.MatchRules;
using System.IO;

namespace PhotoOrganizer.BusinessModule.Visitors
{
    public class VisitorItem
    {
        public DirectoryInfo CurrentDir { get; set; }
        public int ActionVisitorLevel { get; set; }

        Dictionary<string, object> info = new Dictionary<string, object>();

        public Dictionary<string, object> Info
        {
            set { info = value; }
            get { return info; }
        }
    }
}
