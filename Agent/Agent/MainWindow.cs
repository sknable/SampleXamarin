using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class MainWindow : Gtk.Window
    {
        private SODAClient _agent;
        private Interaction _window;
        private Login _loginwindow;

        public MainWindow(SODAClient agent,Login loginWindow) : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            _agent = agent;
            _loginwindow = loginWindow;

            _agent.ParticpationStart += new SODAClient.ParticipationEventHandler(OnParticpationStart);
            _agent.ParticipationStop += new SODAClient.ParticipationEventHandler(OnParticpationStop);

        }


        #region ButtonHanlders
        private void LogoutPressHandler(object obj, EventArgs args)
        {

            try
            {
                _agent.CIM.logout();
            }
            catch
            {

            }
            this.Destroy();
            _loginwindow.Show();

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
            Gtk.Application.Invoke(delegate{ParticpationStarted();});
        }

        private void OnParticpationStop (object sender, ParticitionEventArgs e)
        {
            Gtk.Application.Invoke(delegate{ParticpationStopped();});
        }

        private void ParticpationStarted()
        {
            _window = new Interaction();
            _window.Show();
        }

        private void ParticpationStopped()
        {
            _window.Visible = false; 
            _window.Destroy();
        }


        #endregion 
    }
}

