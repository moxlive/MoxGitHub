using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.BusinessModule.Log
{
    public interface ILogger
    {
        void LogDebug(string debugMessage);
        void LogInfo(string infoMessage);
        void LogError(string errorMessage);
        void LogException(string prefix, Exception e);
    }
}
