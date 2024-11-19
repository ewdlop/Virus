using ClassLibrary2;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;

//https://learn.microsoft.com/en-us/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer


namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        protected readonly EventLog _eventLog1;
        protected int eventId = 1;


        /*
         * 
         * BOOL SetServiceStatus(
              [in] SERVICE_STATUS_HANDLE hServiceStatus,
              [in] LPSERVICE_STATUS      lpServiceStatus
            );
            https://learn.microsoft.com/en-us/windows/win32/api/winsvc/nf-winsvc-setservicestatus
        */


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

         /// <summary>
         /// 
         /// </summary>
         /// <param name="args"></param>
        public Service1(string[] args)
        {
            InitializeComponent();

            string eventSourceName = "MySource";
            string logName = "MyNewLog";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }

            if (args.Length > 1)
            {
                logName = args[1];
            }

            _eventLog1 = new EventLog();

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }

            _eventLog1.Source = eventSourceName;
            _eventLog1.Log = logName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _eventLog1.WriteEntry("In OnStart.");

            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            // Set up a timer that triggers every minute.
            Timer timer = new Timer
            {
                Interval = 60000 // 60 seconds
            };
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            _eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void OnContinue()
        {
            _eventLog1.WriteEntry("In OnContinue.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
        {
            _eventLog1.WriteEntry("In OnStop.");

            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(ServiceHandle, ref serviceStatus);

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(ServiceHandle, ref serviceStatus);
        }
    }
}