using System;
using SODA;
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
                    _agent.CIM.changeAvailability(SODA.CIMSystemService.CIMAvailabilityState.UNAVAILABLE,true,"MONO");
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
                    _agent.CIM.changeAvailability(SODA.CIMSystemService.CIMAvailabilityState.AVAILABLE,true,"MONO");
                }
                catch
                {

                }
            }

        }

        #endregion

        #region SODA Handlers

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
          _window = new Interaction(e);
          this.Hidden += new EventHandler(_window.OnParentHidden);
          _window.Show();

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

