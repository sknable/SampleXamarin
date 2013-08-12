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
            winLogin.Show ();
			Application.Run ();


		}
	}
}
