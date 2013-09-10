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
	[Register ("FirstViewController")]
	partial class FirstViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnLogin { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblPassword { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblUser { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField textPassword { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField textUser { get; set; }

		[Action ("OnLogin:")]
		partial void OnLogin (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (textPassword != null) {
				textPassword.Dispose ();
				textPassword = null;
			}

			if (textUser != null) {
				textUser.Dispose ();
				textUser = null;
			}

			if (btnLogin != null) {
				btnLogin.Dispose ();
				btnLogin = null;
			}

			if (lblPassword != null) {
				lblPassword.Dispose ();
				lblPassword = null;
			}

			if (lblUser != null) {
				lblUser.Dispose ();
				lblUser = null;
			}
		}
	}
}
