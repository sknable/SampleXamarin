using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SODA;
using SODA.CIMSystemService;
namespace iOSAgent
{
	public partial class SecondViewController : UIViewController
	{
        private SODAClient _agent;
		private Boolean _state = false;

		public SecondViewController (SODAClient agent) : base ("SecondViewController", null)
		{
            _agent = agent;

			Title = NSBundle.MainBundle.LocalizedString ("Actions", "Actions");
			TabBarItem.Image = UIImage.FromBundle ("second");


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

		partial void OnState (MonoTouch.Foundation.NSObject sender)
		{

			if(_state)
			{
				_state = false;
				btnState.SetTitle("Go Available",UIControlState.Normal);
				_agent.CIM.changeAvailability(CIMAvailabilityState.UNAVAILABLE,true,"Lunch");
			}
			else
			{
				_state = true;				
				btnState.SetTitle("Go To Lunch",UIControlState.Normal);
				_agent.CIM.changeAvailability(CIMAvailabilityState.AVAILABLE,true,"Ready");
			}



		}


		partial void OnTakeNext (MonoTouch.Foundation.NSObject sender)
		{
			_agent.CIM.takeNextInteraction();
		}


	}
}

