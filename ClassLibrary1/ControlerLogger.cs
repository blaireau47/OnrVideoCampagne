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
        Information,
        Warning
    }
    public class ControlerLogger
    {
        public static void WriteInformation(string _message)
        {
            writeMessage(_message, LogType.Information);
        }


        public static void LogError(string _title, string _message)
        {
            writeMessage(_title, _message, LogType.Error);
        }

        public static void LogError(string _message, Exception _ex)
        {
            string msg;
            if (_ex.InnerException != null && _ex.InnerException.InnerException != null)
            {
                msg = _message + _ex.Message + _ex.InnerException.Message + _ex.InnerException.InnerException.Message  +_ex.StackTrace;
            }else if(_ex.InnerException != null)
                msg = _message + _ex.Message + _ex.InnerException.Message + _ex.StackTrace;
            else
            msg = _message + _ex.Message  + _ex.StackTrace;

            LogError(_ex.Message, msg);
        }

        public static void LogErrror(Exception  _ex)
        {
            string msg = _ex.Message + _ex.InnerException.Message + _ex.StackTrace;

            LogError(_ex.Message, msg);
        }
        private static void writeMessage(string _message, LogType _type)
        {
            writeMessage("", _message, _type);
        }

        private static void writeMessage(string _msgTitle, string _message, LogType _type)
        {
            switch (_type)
            {
                case LogType.Error:
                    System.Diagnostics.Trace.TraceError(_message);
                    ControlerMailer ctlMailer = new ControlerMailer();
                    ctlMailer.SendErrorLogMessage(_msgTitle, _message);
                    break;
                case LogType.Information:
                    System.Diagnostics.Trace.TraceInformation(_message);
                   
                    break;
                case LogType.Warning:
                    break;
                default:
                    break;
            }
            

        }

    }
}
