using System;
using SODA;
using SODA.CIMSystemService;
using Gtk;
using System.Collections.Generic;

namespace Agent
{
    public partial class MainWindow : Gtk.Window
    {
        private SODAClient _agent;
        private Dictionary<String, Interaction> _interactions = new Dictionary<String, Interaction>();


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
        /// <summary>
        /// Logouts the press handler.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="args">Arguments.</param>
        private void LogoutPressHandler(object obj, EventArgs args)
        {

            try
            {
                _agent.Shutdown();
                _agent.CIM.logout();              
                _agent = null;
            }
            catch
            {

            }

            this.Destroy();
           

        }

        /// <summary>
        /// Takes the next press handler.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="args">Arguments.</param>
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

        /// <summary>
        /// States the change press handler.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="args">Arguments.</param>
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

        /// <summary>
        /// Raises the hidden event.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="args">Arguments.</param>
        private void OnHidden(object obj,EventArgs args)
        {

            if (_agent != null)
            {
                try
                {
                    _agent.Shutdown();
                    _agent.CIM.logout();              
                    _agent = null;
                }
                catch
                {

                }
            }
        }

        #endregion

        #region SODA Handlers
        /// <summary>
        /// Raises the agent view update event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnAgentViewUpdate (object sender, AgentViewEventArgs e)
        {
            Gtk.Application.Invoke(delegate{AgentViewUpdate(e);});
        }
        /// <summary>
        /// Raises the particpation start event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnParticpationStart (object sender, ParticitionEventArgs e)
        {
            Gtk.Application.Invoke(delegate{ParticpationStarted(e);});
        }
        /// <summary>
        /// Raises the particpation stop event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnParticpationStop (object sender, ParticitionEventArgs e)
        {
            Gtk.Application.Invoke(delegate{ParticpationStopped(e);});
        }

        private void ParticpationStarted(ParticitionEventArgs e)
        {
            if (! _interactions.ContainsKey(e.Interaction.id))
            {
                Interaction window = new Interaction(e,_agent);
                this.Hidden += new EventHandler(window.OnParentHidden);
                window.Show();
                _interactions[e.Interaction.id] = window;
            }
    
 

        }
        /// <summary>
        /// Agents the view update.
        /// </summary>
        /// <param name="e">E.</param>
        private void AgentViewUpdate(AgentViewEventArgs e)
        {


            //Sopme wierd scenario is going on here causing null....most likely the thread is ending before this event happens
            if (e != null && e._data != null && _agent != null)
            {

                ListStore interactionsList = (ListStore)viewInteractions.Model;

                interactionsList.Clear();

                foreach (CIMViewData cimData in e._data)
                {
                    interactionsList.AppendValues(cimData.values[0], cimData.values[2],cimData.values[1]);             
                }
            }

 



        }
        /// <summary>
        /// Particpations the stopped.
        /// </summary>
        /// <param name="e">E.</param>
        private void ParticpationStopped(ParticitionEventArgs e)
        {

            if(_interactions.ContainsKey(e.Interaction.id))
            {
                Interaction window = _interactions[e.Interaction.id];
                window.Visible = false; 
                window.Destroy();               
                _interactions.Remove(e.Interaction.id);
            }

        }


        #endregion 
    }
}

