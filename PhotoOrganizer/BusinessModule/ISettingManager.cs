using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.BusinessModule
{
    public interface ISettingManager
    {
        void SaveSetting(string settingName, string value);

        string ReadSettingString(string settingName);
      
    }
}
