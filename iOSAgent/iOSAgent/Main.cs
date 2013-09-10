using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SODA;

namespace iOSAgent
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{

            SODAClient _agent = new SODAClient();

            //_agent.LoginAgent("", "", "", "");

            //_agent.Run();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
