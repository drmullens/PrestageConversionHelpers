using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestageCommonMethodCleanerHelpers
{
    public class Logger
    {
        private static volatile Logger _instance;
        private static object syncRoot = new Object();

        public static Logger Instance
        {
            get {
                if (null == _instance) {
                    lock (syncRoot)
                    {
                        if (null == _instance) {
                            _instance = new Logger();
                            
                        }
                    }
                }
                return _instance; 
            }
        }

        private double _progress;

        public double Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }


        private string _currentStatus;

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }


        private Logger() { }


    }
}
