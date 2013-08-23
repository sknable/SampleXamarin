using System;
using Gtk;
using SODA;


namespace Agent
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();

           SODAClient agent = new SODAClient();

            Login winLogin = new Login (agent);
            winLogin.DeleteEvent += new DeleteEventHandler(OnDelete);
            winLogin.Show ();
			Application.Run ();


		}

        static void OnDelete(object obj,DeleteEventArgs args)
        {
            Application.Quit();
        }
	}
}
