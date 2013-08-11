using System;
using SODA.CIMSystemService;
using System.Threading;

namespace SODA
{
	public class SODAClient
	{
	
        #region Properties
		private CIMSystemService.CIMSystemService _webService;
        private Boolean _isRunning = false;
        private const int MAX_DURATION = 2;
        private Thread _eventThread;
        #endregion


        #region Getters

        public CIMSystemService.CIMSystemService CIM {
            get { return _webService;}
        }

        public Boolean isRunning {
            get { return _isRunning;}
        }

        #endregion 

        /// <summary>
        /// Initializes a new instance of the <see cref="SODA.SODAClient"/> class.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        /// <param name="extension">Extension.</param>
		public SODAClient (String url,String user,String password,String extension)
		{
			_webService = new CIMSystemService.CIMSystemService();
		}

        /// <summary>
        /// Run this instance.
        /// </summary>
		public void Run()
		{

            if (Login())
            {
                _eventThread = new Thread(EventGetter);
                _eventThread.IsBackground = true;
                _eventThread.Start();

            }

		}


        #region Operations
        /// <summary>
        /// Login this instance.
        /// </summary>
		private bool Login()
		{
			CIMLoginResult loginResult = _webService.loginAgent ("agent1", "", "AGENT", "en_us", CIMPhoneType.STANDARD, true, "2001", "+00:00", null);

			if (loginResult.code == CIMResultType.SUCCEEDED) 
			{          
                return true;

			} else {

                return false;
			}

		}
        #endregion



        #region EventHanlders

        /// <summary>
        /// Events the getter.
        /// </summary>
        private void EventGetter()
        {
            CIMEventListResult eventGetterResult = null;
            _isRunning = true;

            while (_isRunning)
            {
                try
                {
                    eventGetterResult = _webService.getEvents(MAX_DURATION);
                }
                catch
                {
                    //Eat this error for now...this would happen if our session timed out or ended
                }

                if (eventGetterResult != null && eventGetterResult.code == CIMResultType.SUCCEEDED)
                {

                    HandleEvents(eventGetterResult.eventList);

                }
                else
                {

                    //Log error here, kill our loop
                    _isRunning = false;
                }       
            }
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cimEvents">Cim events.</param>
        private void HandleEvents(CIMEvent[] cimEvents)
        {

            if (cimEvents != null)
            {
                foreach(CIMEvent cimEvent in cimEvents)
                {


                }

               
            }
        }

        #endregion
	}
}

