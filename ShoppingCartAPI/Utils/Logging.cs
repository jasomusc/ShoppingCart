using ShoppingCartAPI.DB;
using System;
using System.Diagnostics;

namespace ShoppingCartAPI.Utils
{
    public class Logging
    {
        /// <summary>
        /// Logs messages into the database/file.
        /// </summary>
        /// <param name="token">The token used to authenticate</param>
        /// <param name="exception">Message to log</param>
        public void LogProgress(string token, string ip, Exception exception)
        {
            CallErrorLog(token, ip, exception);
        }
        
        /// <summary>
        /// Will get the Function Name and the Class Name that 
        /// calls the log mehtod. Then this will call the insert
        /// method to write into the database/file.
        /// </summary>
        /// <param name="token">The token used to authenticate</param>
        /// <param name="exception">Message to log</param>
        private void CallErrorLog(string token, string ip, Exception exception)
        {
            StackTrace strace = new StackTrace();
            string processName = strace.GetFrame(2).GetMethod().ReflectedType.FullName + "." + strace.GetFrame(2).GetMethod().Name;
            string process = strace.GetFrame(2).GetMethod().Module.Name;
            
            Error_Logging error = new Error_Logging();
            error.Process = process;
            error.ProcessName = processName;
            error.Message = exception.ToString();
            error.CreatedBy = token + " - " + ip;
            error.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!AddToDatabaseLog(error))
            {
                WriteToEventLog(error);
            }
        }
        
        /// <summary>
        /// This method will write to the Event Log
        /// </summary>
        /// <param name="logObject">Logging Object with the passed information.</param>
        private void WriteToEventLog(Error_Logging error)
        {
            string LogSource = "Shopping Cart API";

            try
            {
                if (!EventLog.SourceExists(LogSource))
                {
                    EventLog.CreateEventSource(LogSource, "Shopping Cart API");
                }
                EventLog.WriteEntry(LogSource, error.ProcessName + " - " + error.Message, EventLogEntryType.Warning);
            }
            catch
            {
                throw new Exception("No permission to write to event log.");
            }
        }
        
        /// <summary>
        /// Inserts the passed information into to database
        /// </summary>
        /// <param name="token">The token used to authenticate</param>
        /// <param name="logObject">Logging Object with the passed information.</param>
        /// <returns>True if sucessfully inserted, else False.</returns>
        private bool AddToDatabaseLog(Error_Logging error)
        {
            bool result = false;

            try
            {
                using (ShoppingCartEntities context = new ShoppingCartEntities())
                {
                    context.Error_Logging.Add(error);
                    context.SaveChanges();

                    result = true;
                }
            }
            catch
            {
                //Failed to log error in database.
            }

            return result;
        }
    }    
}