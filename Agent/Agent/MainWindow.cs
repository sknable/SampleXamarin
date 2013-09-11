using System;
using SODA;
using SODA.CIMSystemService;
using Gtk;

namespace Agent
{
    public partial class MainWindow : Gtk.Window
    {
        private SODAClient _agent;
        private Interaction _window;


        public MainWindow(SODAClient agent) : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            _agent = agent;          
            _agent.ParticpationStart += new SODAClient.ParticipationEventHandler(OnParticpationStart);
            _agent.ParticipationStop += new SODAClient.ParticipationEventHandler(OnParticpationStop);
            _agent.AgentViewUpdate += new SODAClient.AgentViewEventHandler(OnAgentViewUpdate);

      

            TreeViewColumn iid = new TreeViewColumn();
            iid.Title = "\tIID\t";
            CellRendererText iidCell = new CellRendererText();
            iid.PackStart(iidCell,true);

            TreeViewColumn name = new TreeViewColumn();
            name.Title = "\tCustomer Name\t";
            CellRendererText nameCell = new CellRendererText();
            name.PackStart(nameCell, true);

            TreeViewColumn itype = new TreeViewColumn();
            itype.Title = "\tIType\t";
            CellRendererText itypeCell = new CellRendererText();
            itype.PackStart(itypeCell, true);

            iid.AddAttribute(iidCell, "text", 0);
            name.AddAttribute(nameCell, "text", 1);
            itype.AddAttribute(itypeCell, "text", 2);

            viewInteractions.AppendColumn(iid); 
            viewInteractions.AppendColumn(name); 
            viewInteractions.AppendColumn(itype); 

            ListStore interactionList = new ListStore(typeof (string),typeof (string),typeof(string));

            viewInteractions.Model = interactionList;


            //First one is too quick
            if (! _agent.SubscribeToAgentQueueView())
            {
                _agent.SubscribeToAgentQueueView();
            }

        }


        #region ButtonHanlders

        private void LogoutPressHandler(object obj, EventArgs args)
        {

            try
            {
                _agent.CIM.logout();
                _agent = null;
            }
            catch
            {

            }

            this.Destroy();
           

        }

        private void TakeNextPressHandler(object obj, EventArgs args)
        {
            try
            {
                _agent.CIM.takeNextInteraction();
            }
            catch
            {

            }
        }


        private void StateChangePressHandler(object obj, EventArgs args)
        {

            if( changeState.StockId == "gtk-yes")
            {
                changeState.StockId = "gtk-no";
                try
                {
                    _agent.CIM.changeAvailability(CIMAvailabilityState.UNAVAILABLE,true,"MONO");
                }
                catch
                {

                }

            }
            else
            {
                changeState.StockId = "gtk-yes";
                try
                {
                    _agent.CIM.changeAvailability(CIMAvailabilityState.AVAILABLE,true,"MONO");
                }
                catch
                {

                }
            }

        }

        #endregion

        #region SODA Handlers

        private void OnAgentViewUpdate (object sender, AgentViewEventArgs e)
        {
            Gtk.Application.Invoke(delegate{AgentViewUpdate(e);});
        }

        private void OnParticpationStart (object sender, ParticitionEventArgs e)
        {
            Gtk.Application.Invoke(delegate{ParticpationStarted(e);});
        }

        private void OnParticpationStop (object sender, ParticitionEventArgs e)
        {
            Gtk.Application.Invoke(delegate{ParticpationStopped(e);});
        }

        private void ParticpationStarted(ParticitionEventArgs e)
        {
            //Only works for one window
          _window = new Interaction(e,_agent);
          this.Hidden += new EventHandler(_window.OnParentHidden);
          _window.Show();

        }

        private void AgentViewUpdate(AgentViewEventArgs e)
        {
            ListStore interactionsList = (ListStore)viewInteractions.Model;

            interactionsList.Clear();

            foreach (CIMViewData cimData in e._data)
            {
                interactionsList.AppendValues(cimData.values[0], cimData.values[2],cimData.values[1]);             
            }



        }

        private void ParticpationStopped(ParticitionEventArgs e)
        {
            _window.Visible = false; 

            _window.Destroy();
        }

        private void OnHidden(object obj,EventArgs args)
        {

            if (_agent != null)
            {
                try
                {
                    _agent.CIM.logout();
                }
                catch
                {

                }
            }
        }

        #endregion 
    }
}

