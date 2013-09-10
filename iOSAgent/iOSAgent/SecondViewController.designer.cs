// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace iOSAgent
{
	[Register ("SecondViewController")]
	partial class SecondViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnState { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnTakeNext { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblInteraction { get; set; }

		[Action ("OnState:")]
		partial void OnState (MonoTouch.Foundation.NSObject sender);

		[Action ("OnTakeNext:")]
		partial void OnTakeNext (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnState != null) {
				btnState.Dispose ();
				btnState = null;
			}

			if (btnTakeNext != null) {
				btnTakeNext.Dispose ();
				btnTakeNext = null;
			}

			if (lblInteraction != null) {
				lblInteraction.Dispose ();
				lblInteraction = null;
			}
		}
	}
}
