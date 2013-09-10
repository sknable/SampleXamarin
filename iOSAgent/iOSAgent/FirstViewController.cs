using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SODA;
namespace iOSAgent
{
	public partial class FirstViewController : UIViewController
	{

        private SODAClient _agent;

        public FirstViewController(SODAClient agent) : base("FirstViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Login", "Login");
			TabBarItem.Image = UIImage.FromBundle ("first");

            _agent = agent;

            //_agent.LoginAgent("", "", "", "");

            //_agent.Run();

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
	}
}

