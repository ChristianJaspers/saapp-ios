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
	[Register ("ForgotPasswordViewController")]
	partial class ForgotPasswordViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton forgotPasswordButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField inputEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton skipButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (forgotPasswordButton != null) {
				forgotPasswordButton.Dispose ();
				forgotPasswordButton = null;
			}
			if (inputEmail != null) {
				inputEmail.Dispose ();
				inputEmail = null;
			}
			if (skipButton != null) {
				skipButton.Dispose ();
				skipButton = null;
			}
		}
	}
}
