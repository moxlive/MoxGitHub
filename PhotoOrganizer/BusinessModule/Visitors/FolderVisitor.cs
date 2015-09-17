using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PhotoOrganizer.BusinessModule.MatchRules;

namespace PhotoOrganizer.BusinessModule.Visitors
{
    //find all fold matching rules in current dir
    public class FolderVisitor
    {
        public IList<IFolderMatchRule> MatchRules { get; set; }
        public string VisitorKey { get; set; }

        public FolderVisitor(IList<IFolderMatchRule> rules, string key)
        {
            VisitorKey = key;
            MatchRules = rules;
        }

        public IList<VisitorItem> CollectValidDirectories(VisitorItem currentItem)
        {
            IList<VisitorItem> validFolders = new List<VisitorItem>();
            foreach (DirectoryInfo d in currentItem.CurrentDir.GetDirectories())
            {
                if (ValidateFolderName(d.Name))
                {
                    //log
                    VisitorItem newItem = new VisitorItem
                    {
                        CurrentDir = d,
                        ActionVisitorLevel = currentItem.ActionVisitorLevel + 1,
                        Info = new Dictionary<string, object>(currentItem.Info)
                    };

                    newItem.Info.Add(VisitorKey, d.Name);
                    validFolders.Add(newItem);
                }
            }

            return validFolders;
        }

        public bool ValidateFolderName(string input)
        {
            foreach (IFolderMatchRule r in MatchRules)
            {
                if (!r.CheckMatch(input))
                {
                    //log
                    return false;
                }
            }

            return true;
        }
    }
}
