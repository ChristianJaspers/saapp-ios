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
	[Register ("ProfileViewController")]
	partial class ProfileViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BackButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProfileImageEditButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView ProfileImageView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}
			if (ProfileImageEditButton != null) {
				ProfileImageEditButton.Dispose ();
				ProfileImageEditButton = null;
			}
			if (ProfileImageView != null) {
				ProfileImageView.Dispose ();
				ProfileImageView = null;
			}
		}
	}
}
