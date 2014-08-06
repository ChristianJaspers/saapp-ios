// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace BetterSalesman.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton buttonForgotPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField inputEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField inputPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButtonRounded loginButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (buttonForgotPassword != null) {
				buttonForgotPassword.Dispose ();
				buttonForgotPassword = null;
			}
			if (inputEmail != null) {
				inputEmail.Dispose ();
				inputEmail = null;
			}
			if (inputPassword != null) {
				inputPassword.Dispose ();
				inputPassword = null;
			}
			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}
		}
	}
}
