using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class MainWindow : Gtk.Window
    {
        private SODAClient _agent;

        public MainWindow(SODAClient agent) : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            _agent = agent;

            _agent += new SODAClient.ParticipationEventHandler(OnParticpationStart);
           

        }


        #region ButtonHanlders
        private void LogoutPressHandler(object obj, EventArgs args)
        {



        }


        private void TakeNextPressHandler(object obj, EventArgs args)
        {



        }


        private void StateChangePressHandler(object obj, EventArgs args)
        {

            if( changeState.StockId == "gtk-yes")
            {
                changeState.StockId = "gtk-no";

            }
            else
            {
                changeState.StockId = "gtk-yes";
            }

        }

        #endregion

        #region SODA Handlers

        private void OnParticpationStart (object sender, ParticitionEventArgs e)
        {

        }


        #endregion 
    }
}

