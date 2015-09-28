using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.BusinessModule
{
    public delegate void MessageProcessor(string message);
    public interface IMessageDispatcher
    {
        event MessageProcessor MessageHandler;

        void PopulateMessage(string message);
    }

    public class MessageDispatcher : IMessageDispatcher
    {
        public event MessageProcessor MessageHandler;

        public void PopulateMessage(string message)
        {
            if (MessageHandler != null)
            {
                MessageHandler.Invoke(message);
            }
        }
    }
}
