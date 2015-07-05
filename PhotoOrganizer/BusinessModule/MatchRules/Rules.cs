using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PhotoOrganizer.BusinessModule.MatchRules
{
    public interface IFolderMatchRule
    {
        string RuleName {get; set;}

        bool CheckMatch(object attribute);
    }


    public class FolderNameMatchRule : IFolderMatchRule
    {
        public string RuleName { get; set; }

        public string RegExpPattern { get; set; }

        public bool CheckMatch(object attribute)
        {
            string Name = attribute as string;

            if (Name == null)
            {
                //error
                return false;
            }

            return Regex.IsMatch(Name, RegExpPattern);
        }
    }
}
