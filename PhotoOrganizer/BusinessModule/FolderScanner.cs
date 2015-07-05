using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using PhotoOrganizer.BusinessModule.Photos;
using PhotoOrganizer.BusinessModule.MatchRules;
using PhotoOrganizer.BusinessModule.Visitors;

namespace PhotoOrganizer.BusinessModule
{
    /// <summary>
    /// rule scanPath/Raw Scans/date/CustomSeqNum/ScanSeqNum/0xx.jpg
    /// </summary>
    public class FolderScanner
    {
        private string scanPath = @"D:\Programs\Github\PhotoOrganizer\TestFolder\";
        private List<FolderVisitor> visitors;

        private const string visitorKey_RawScan = "RawScan";
        private const string visitorKey_ScanDate = "ScanDate";
        private const string visitorKey_CustomSeqNum = "CustomSeqNum";
        private const string visitorKey_ScanSeqNum = "ScanSeqNum";

        private const string frontPicName = "089.jpg";
        private const string backPicName = "090.jpg";

        public void InitVisitors()
        {
            visitors = new List<FolderVisitor>();
            List<IFolderMatchRule> rules = new List<IFolderMatchRule>();

            //Raw Scans
            FolderNameMatchRule rule = new FolderNameMatchRule();
            rule.RuleName = "Raw Scans";
            rule.RegExpPattern = "RAW Scans";
            rules.Add(rule);
            visitors.Add(new FolderVisitor(rules, visitorKey_RawScan));

            //date folder
            FolderNameMatchRule rule2 = new FolderNameMatchRule();
            rule2.RuleName = "Date";
            //Reg expression for yyyymmdd
            rule2.RegExpPattern = "^(?:(?:(?:(?:(?:[13579][26]|[2468][048])00)|(?:[0-9]{2}(?:(?:[13579][26])|(?:[2468][048]|0[48]))))(?:(?:(?:09|04|06|11)(?:0[1-9]|1[0-9]|2[0-9]|30))|(?:(?:01|03|05|07|08|10|12)(?:0[1-9]|1[0-9]|2[0-9]|3[01]))|(?:02(?:0[1-9]|1[0-9]|2[0-9]))))|(?:[0-9]{4}(?:(?:(?:09|04|06|11)(?:0[1-9]|1[0-9]|2[0-9]|30))|(?:(?:01|03|05|07|08|10|12)(?:0[1-9]|1[0-9]|2[0-9]|3[01]))|(?:02(?:[01][0-9]|2[0-8])))))$";
            rules = new List<IFolderMatchRule>();
            rules.Add(rule2);
            visitors.Add(new FolderVisitor(rules, visitorKey_ScanDate));

            //customer sequence folder
            FolderNameMatchRule rule3 = new FolderNameMatchRule();
            rule3.RuleName = "CustomerSeqNumber";
            rule3.RegExpPattern = @"(\b)?\d+";
            rules = new List<IFolderMatchRule>();
            rules.Add(rule3);
            visitors.Add(new FolderVisitor(rules, visitorKey_CustomSeqNum));

            //scan sequence folder
            FolderNameMatchRule rule4 = new FolderNameMatchRule();
            rule4.RuleName = "ScanSeqNum";
            rule4.RegExpPattern = @"(\b)?\d+";
            rules = new List<IFolderMatchRule>();
            rules.Add(rule4);
            visitors.Add(new FolderVisitor(rules, visitorKey_ScanSeqNum));

        }

        public IList<PhotoGroup> FindNewPhotoGroups()
        {
            IList<PhotoGroup> photoGroups = new List<PhotoGroup>();

            if (visitors.Count < 1)
            {
                return photoGroups;
            }

            Queue<VisitorItem> children = new Queue<VisitorItem>();
            VisitorItem start = new VisitorItem() 
            { 
                CurrentDir = new DirectoryInfo(scanPath),  
                ActionVisitorLevel = 0
            };           

            children.Enqueue(start);
            VisitorItem next;
            do
            {
                VisitorItem currentInfo = children.Dequeue();
                IList<VisitorItem> dirs = visitors[currentInfo.ActionVisitorLevel].CollectValidDirectories(currentInfo);

                foreach (VisitorItem d in dirs)
                {
                    children.Enqueue(d);
                }

                next = children.Peek();
            } while (next != null && next.ActionVisitorLevel < visitors.Count && children.Count > 0);

            //todo move this code out of scanner
            foreach (VisitorItem picFolder in children)
            {
                PhotoGroup pg = CreatePhotoGroup(picFolder);
                if (pg != null)
                {
                    photoGroups.Add(pg);
                }
               
            }

            return photoGroups;            
        }

        private PhotoGroup CreatePhotoGroup(VisitorItem item)
        {
            DirectoryInfo dir = item.CurrentDir;

            if (!File.Exists(dir.FullName + "\\" + frontPicName) || !File.Exists(dir.FullName + "\\" + frontPicName))
            {
                return null;
                //log
            }

            PhotoGroup pg = new PhotoGroup
            {
                Date = DateTime.ParseExact((string)item.Info[visitorKey_ScanDate], "yyyyMMdd", null),
                CustomSeqNum = item.Info[visitorKey_CustomSeqNum] as string,
                ScanSeqNum = item.Info[visitorKey_ScanSeqNum] as string,
                Front = new Photo()
                {
                   Name = frontPicName,
                   FullPath = item.CurrentDir.FullName + "\\" + frontPicName
                },
                Back = new Photo()
                {
                    Name = backPicName,
                    FullPath = item.CurrentDir.FullName + "\\" + backPicName
                }
            };

            return pg;
        }
       
    }

  
}
