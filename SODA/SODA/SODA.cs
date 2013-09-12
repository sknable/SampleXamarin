using System;
using SODA.CIMSystemService;
using System.Threading;
using System.Collections.Generic;
namespace SODA
{
	public class SODAClient
	{
	
        #region Properties
		private CIMSystemService.CIMSystemService _webService;
        private Boolean _isRunning = false;
        private const int MAX_DURATION = 2;
        private Thread _eventThread;
        private Dictionary<String, String> _interactions = new Dictionary<String, String>();
        private int _agentQViewSize = 0;

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
        /// Shutdown this instance.
        /// </summary>
        public void Shutdown() { 
            _isRunning = false;
        }

        /// <summary>
        /// Login this instance.
        /// </summary>
		private bool Login()
		{


            //Sample to show pre-processor defs
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
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool Login(String user, String password)
        {

            String extension = "200";

            String number = user.Substring(user.Length - 1);

            CIMLoginResult loginResult = _webService.loginAgent(user, password, "AGENT", "en_us", CIMPhoneType.STANDARD, true,extension + number, "+00:00", null);

            if (loginResult.code == CIMResultType.SUCCEEDED)
            {

                return true;

            }
            else
            {

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

            if (user.Length > 0)
            {
                if (Login(user,password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
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

        }

        public Boolean  SubscribeToAgentQueueView()
        {
            CIMViewMetaData metaData = new CIMViewMetaData();
            metaData.viewType = CIMViewType.AgentQueueView;
            metaData.viewTypeSpecified = true;

            CIMViewFilter filter = new CIMViewFilter();
            CIMViewFilter filter2 = new CIMViewFilter();
            filter.name = CIMViewFilterOperation.EQUAL.ToString();
            filter.operation = CIMViewFilterOperation.EQUAL;
            filter.operationSpecified = true;
            filter.id = "ITYPE";
            filter.values = new String[4] { "EMAIL", "INBOUND_QUEUED_CALL", "TASK", "WEB_CHAT" };


            metaData.filters = new CIMViewFilter[1] { filter };

            CIMViewSubscriptionResult result = _webService.subscribeToView(-1, metaData);

            if (result.code != CIMResultType.SUCCEEDED)
            {
                return false;
            }
            else
            {
                return true; 
            }



        }
        #endregion


        #region View EventHandlers

        public delegate void AgentViewEventHandler(object sender, AgentViewEventArgs e);

        public event AgentViewEventHandler AgentViewUpdate;

        protected void OnAgentViewUpdate(AgentViewEventArgs e)
        {
            AgentViewEventHandler MyEvent = AgentViewUpdate;

            if (MyEvent != null)
            {
                MyEvent(this, e);
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

            if (MyEvent != null && _isRunning)
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

            if (MyEvent != null && _isRunning)
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

            //Dont send out a event if we are logging out
            if (MyEvent != null && _isRunning)
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

            eventGetterResult = _webService.getEvents(MAX_DURATION);
            _webService.changeAvailability(SODA.CIMSystemService.CIMAvailabilityState.AVAILABLE, true, "Ready");
           
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

                        case CIMEventType.CIMInteractionHandlingService_PARTICIPATION_STOPPED:

                            CIMParticipationStoppedEvent interactionStopEvent = (CIMParticipationStoppedEvent)cimEvent;

                            CIMParticipation participation = new CIMParticipation();

                            participation.id = interactionStopEvent.interactionId;

                            ParticitionEventArgs eStopped = new ParticitionEventArgs(participation);

                            OnParticpationStop(eStopped);


                        break;
                        

                        case CIMEventType.CIMSystemService_VIEW_UPDATE:

                             int agentQViewSize = 0;
                             CIMViewUpdateEvent viewEvent = (CIMViewUpdateEvent)cimEvent;

                             if (viewEvent.rows != null)
                             {
                                 if (viewEvent.configuredMetaData.viewType == CIMViewType.AgentQueueView)
                                 {

                                    _interactions.Clear();
                                    foreach (CIMViewData cimData in viewEvent.rows)
                                    {
                                         if (_interactions.ContainsKey(cimData.values[0]))
                                         {
                                              // Do Nothing      
                                         }
                                         else
                                         {
                                            _interactions[cimData.values[0]] = cimData.values[2];
                                         }

                                    }


                                    if (_agentQViewSize != _interactions.Count)
                                    {
                                        OnAgentViewUpdate(new AgentViewEventArgs(viewEvent.rows));
                                    }

                                    agentQViewSize = _interactions.Count;

                                 }

                             }


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


    /// <summary>
    /// 
    /// </summary>
    public class AgentViewEventArgs : System.EventArgs
    {
        public CIMViewData[] _data;


        public AgentViewEventArgs(CIMViewData[] data)
        {
            _data = data;
        }
    }
}

