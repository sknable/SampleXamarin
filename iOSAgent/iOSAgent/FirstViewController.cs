using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SODA;
using SODA.CIMSystemService;
namespace iOSAgent
{
	public partial class FirstViewController : UIViewController
	{

        private SODAClient _agent;

        public FirstViewController(SODAClient agent) : base("FirstViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Agent", "Agent");
			TabBarItem.Image = UIImage.FromBundle ("first");

            _agent = agent;
				
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		/// <summary>
		/// Buttons the login.
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void OnLogin (MonoTouch.Foundation.NSObject sender)
		{
			if(_agent.isRunning)
			{
				btnLogin.SetTitle("Login",UIControlState.Disabled);
				_agent.CIM.logout();
				btnLogin.SetTitle("Login",UIControlState.Normal);
				textPassword.Hidden = false;
				textUser.Hidden = false;
				lblUser.Hidden = false;
				lblPassword.Hidden = false;
			}
			else
			{
				btnLogin.SetTitle("Login",UIControlState.Disabled);
				if(_agent.LoginAgent("","","",""))
				{
					_agent.Run();
					btnLogin.SetTitle("Logout",UIControlState.Normal);
					textPassword.Hidden = true;
					textUser.Hidden = true;
					lblUser.Hidden = true;
					lblPassword.Hidden = true;
				}
				else
				{
					btnLogin.SetTitle("Login",UIControlState.Normal);
				}
			}

		}

	}
}

