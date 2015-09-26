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
using PhotoOrganizer.BusinessModule.Common;
using PhotoOrganizer.BusinessModule.Log;

namespace PhotoOrganizer.BusinessModule
{
    /// <summary>
    /// rule scanPath/Raw Scans/date/CustomSeqNum/ScanSeqNum/0xx.jpg
    /// </summary>
    public class FolderScanner
    {        

        private const string visitorKey_RawScan = "RawScan";
        private const string visitorKey_ScanDate = "ScanDate";
        private const string visitorKey_CustomSeqNum = "CustomSeqNum";
        private const string visitorKey_ScanSeqNum = "ScanSeqNum";

        private List<FolderVisitor> visitors;
        private Settings settings;
        private string frontPicName = "089.jpg";
        private string backPicName = "090.jpg";
        private string scanPath = @"D:\";
        private ILogger log;

        public FolderScanner()
        {
            log = FileLogger.CreateLogger("FolderScanner");
        }

        public Action<PhotoGroup> NewPhotoGoupHandler;

        public void InitVisitors(Settings settings)
        {
            this.settings = settings;
            LoadSettings();
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

            WatchChange();

        }

        public void FullScan()
        {
            log.LogInfo(string.Format("Start Full Scan, base folder {0}", settings.ScanBasePath));
            IList<PhotoGroup> groups = FindNewPhotoGroups();

            foreach (PhotoGroup group in groups)
            {
                if (NewPhotoGoupHandler != null)
                {
                    NewPhotoGoupHandler.Invoke(group);
                }
            }

        }

        #region private helper
        private IList<PhotoGroup> FindNewPhotoGroups()
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
                log.LogInfo(string.Format("Did not find {0} or {1} in folder {2}.", frontPicName, backPicName, dir.FullName));
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
            
            log.LogInfo(string.Format("Created photo group with {0} and {1} in folder {2}. Date={3}, CustomSeqNum={4}, ScanSeqNum={5}", 
                frontPicName, backPicName, dir.FullName, pg.Date, pg.CustomSeqNum, pg.ScanSeqNum));  
            return pg;
        }

        private void LoadSettings()
        {
            frontPicName = settings.FrontPictureName;
            backPicName = settings.BackPictureName;
            scanPath = settings.ScanBasePath;
        }

        private void WatchChange()
        {
            var changeWatcher = new System.IO.FileSystemWatcher();
            changeWatcher.Path = scanPath;
            changeWatcher.IncludeSubdirectories = true;
            changeWatcher.Filter = "*.jpg";
            changeWatcher.Created += ChangeHandler;
            changeWatcher.EnableRaisingEvents = true;
            log.LogInfo(string.Format("Start watching change in folder {0}", scanPath));
        }

        private void ChangeHandler(object sender, FileSystemEventArgs e)
        {
            string relativePath = e.Name; //RAW Scans\20150705\C0001\006\090.jpg
            string fileName = Path.GetFileName(relativePath);
            string directoryPath = Path.GetDirectoryName(e.FullPath); //D:\Programs\Github\PhotoOrganizer\TestFolder\RAW Scans\20150705\C0001\006

            //check if both front and back picture exist
            if (!((fileName == frontPicName && File.Exists(directoryPath + "\\" + backPicName))
                || (fileName == backPicName && File.Exists(directoryPath + "\\" + frontPicName))))
            {
                return;
            }

            log.LogInfo(string.Format("Locate new picture {0} and {1} in {2}", backPicName, frontPicName, directoryPath));
            //if folder hierarchy level
            string[] folderNames = Path.GetDirectoryName(relativePath).Split(new char[] { '\\', '/' });
            if (folderNames.Length != visitors.Count)
            {
                return;
            }

            //check folder name against rules
            VisitorItem item = new VisitorItem();
            item.CurrentDir = new DirectoryInfo(directoryPath);
            for(int i = 0; i < visitors.Count; i++)
            {
                if (!visitors[i].ValidateFolderName(folderNames[i]))
                {
                    log.LogInfo(string.Format("Picture path {0} did not match rules, skipped", directoryPath));   
                    return;
                }

                item.Info.Add(visitors[i].VisitorKey, folderNames[i]);
            }

            PhotoGroup group = CreatePhotoGroup(item);

            if (NewPhotoGoupHandler != null)
            {
                NewPhotoGoupHandler.Invoke(group);
            }
        }

        #endregion
    }
  
}
