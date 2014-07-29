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
		UILabel displayNameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelActivityAllTeams { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelActivityMy { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelActivityMyTeam { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelExperience { get; set; }

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
			if (displayNameLabel != null) {
				displayNameLabel.Dispose ();
				displayNameLabel = null;
			}
			if (labelActivityAllTeams != null) {
				labelActivityAllTeams.Dispose ();
				labelActivityAllTeams = null;
			}
			if (labelActivityMy != null) {
				labelActivityMy.Dispose ();
				labelActivityMy = null;
			}
			if (labelActivityMyTeam != null) {
				labelActivityMyTeam.Dispose ();
				labelActivityMyTeam = null;
			}
			if (labelExperience != null) {
				labelExperience.Dispose ();
				labelExperience = null;
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
