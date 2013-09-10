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
		public SODAClient ()
		{
			_webService = new CIMSystemService.CIMSystemService();
		}

        /// <summary>
        /// Run this instance.
        /// </summary>
		public void Run()
		{

            _eventThread = new Thread(EventGetter);
            _eventThread.IsBackground = true;
            _eventThread.Start();

		}


        #region Operations
        /// <summary>
        /// Login this instance.
        /// </summary>
		private bool Login()
		{

            #if __ANDROID__
                CIMLoginResult loginResult = _webService.loginAgent ("agentDroid", "", "AGENT", "en_us", CIMPhoneType.STANDARD, true, "2001", "+00:00", null);

            #elif __IOS__
                CIMLoginResult loginResult = _webService.loginAgent ("agentIOS", "", "AGENT", "en_us", CIMPhoneType.STANDARD, true, "2002", "+00:00", null);
            #else
                CIMLoginResult loginResult = _webService.loginAgent("agent1", "", "AGENT", "en_us", CIMPhoneType.STANDARD, true, "2003", "+00:00", null);
            #endif


            if (loginResult.code == CIMResultType.SUCCEEDED) 
			{

                return true;

			} else {

                return false;
			}

		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SODA.SODAClient"/> class.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        /// <param name="extension">Extension.</param>
        public Boolean LoginAgent(String url, String user, String password, String extension)
        {

            if (Login())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion


        #region Participation EventHanlders

        public delegate void ParticipationEventHandler(object sender, ParticitionEventArgs e);

        public event ParticipationEventHandler ParticpationStart;
        public event ParticipationEventHandler ParticipationStateChange;
        public event ParticipationEventHandler ParticipationStop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnParticpationStart(ParticitionEventArgs e)
        {
            ParticipationEventHandler MyEvent = ParticpationStart;

            if (MyEvent != null)
            {
                MyEvent(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnParticipationStateChange(ParticitionEventArgs e)
        {
            ParticipationEventHandler MyEvent = ParticipationStateChange;

            if (MyEvent != null)
            {
                MyEvent(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnParticpationStop(ParticitionEventArgs e)
        {
            ParticipationEventHandler MyEvent = ParticipationStop;

            if (MyEvent != null)
            {
                MyEvent(this, e);
            }
        }

        #endregion

        #region SODA EventHanlder

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

                    switch (cimEvent.eventType)
                    {

                        case CIMEventType.CIMInteractionHandlingService_PARTICIPATION_STARTED:

                            CIMParticipationEvent interactionEvent = (CIMParticipationEvent)cimEvent;

                            ParticitionEventArgs e = new ParticitionEventArgs(interactionEvent.participation);

                            OnParticpationStart(e);

                        break;



                    }

                }

               
            }
        }

        #endregion
	}

    /// <summary>
    /// 
    /// </summary>
    public class ParticitionEventArgs : System.EventArgs
    {
        private CIMParticipation _Interaction;

        public CIMParticipation Interaction { get { return _Interaction; } }

        public ParticitionEventArgs(CIMParticipation Interaction)
        {
            _Interaction = Interaction;
        }
    }
}

