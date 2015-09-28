using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoOrganizer.BusinessModule.Common;

namespace PhotoOrganizer.BusinessModule
{
    public interface ISettingManager
    {
        void SaveSetting(string settingName, string value);

        void SaveSettings(Settings setting);

        string ReadSettingString(string settingName);

        void ReadSettings(Settings setting);

    }
}
