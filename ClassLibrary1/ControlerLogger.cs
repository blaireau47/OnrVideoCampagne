using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONRVideo
{
    enum LogType
    {
        Error,
        Message
    }
    public class ControlerLogger
    {
        public static void LogMessage(string _message)
        {
            writeMessage(_message, LogType.Message);
        }


        public static void LogErrror(string _message)
        {
            writeMessage(_message, LogType.Error);
        }

        public static void LogErrror(Exception  _ex)
        {
            string msg = _ex.Message + _ex.InnerException.Message + _ex.StackTrace;

            LogErrror(msg);
        }


        private static void writeMessage(string _message, LogType _type)
        {


        }

        private static void alert(string _message)
        {

        }
    }
}
