using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class Login : Gtk.Window
    {
        private SODAClient _agent;
        private MainWindow _mainWindow;

        public Login(SODAClient agent) : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            _agent = agent;

        }

        private void ButtonPressHandler(object obj, EventArgs args)
        {
            LoginButton.Sensitive = false;

            if (_agent.LoginAgent("",entryName.Text , entryPassword.Text, ""))
            {

                this.Visible = false;
                _agent.Run();
                _mainWindow = new MainWindow(_agent);
                _mainWindow.Hidden += new EventHandler(OnHidden);
                _mainWindow.Show();
                        

            }
            else
            {

                LoginButton.Sensitive = true;

            }

        }

        private void OnHidden(object obj,EventArgs args)
        {
            LoginButton.Sensitive = true;
            this.Show();
        }

    }
}

