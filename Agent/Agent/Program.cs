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

            SODAClient agent = new SODAClient("", "", "", "");
            agent.Run();

			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();


		}
	}
}
