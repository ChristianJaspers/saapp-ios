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
		UIImageView imageViewBorder { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelActivityLevelLastWeek { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelAllTeamsActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelDisplayName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelEarnInTotal { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelExperience { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelMyActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelMyTeamActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProfileImageEditButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView ProfileImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView profilePictureInBackground { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISliderActivity progressAllTeamsActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISliderActivity progressMyActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISliderActivity progressMyTeamActivity { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (imageViewBorder != null) {
				imageViewBorder.Dispose ();
				imageViewBorder = null;
			}
			if (labelActivityLevelLastWeek != null) {
				labelActivityLevelLastWeek.Dispose ();
				labelActivityLevelLastWeek = null;
			}
			if (labelAllTeamsActivity != null) {
				labelAllTeamsActivity.Dispose ();
				labelAllTeamsActivity = null;
			}
			if (labelDisplayName != null) {
				labelDisplayName.Dispose ();
				labelDisplayName = null;
			}
			if (labelEarnInTotal != null) {
				labelEarnInTotal.Dispose ();
				labelEarnInTotal = null;
			}
			if (labelExperience != null) {
				labelExperience.Dispose ();
				labelExperience = null;
			}
			if (labelMyActivity != null) {
				labelMyActivity.Dispose ();
				labelMyActivity = null;
			}
			if (labelMyTeamActivity != null) {
				labelMyTeamActivity.Dispose ();
				labelMyTeamActivity = null;
			}
			if (ProfileImageEditButton != null) {
				ProfileImageEditButton.Dispose ();
				ProfileImageEditButton = null;
			}
			if (ProfileImageView != null) {
				ProfileImageView.Dispose ();
				ProfileImageView = null;
			}
			if (profilePictureInBackground != null) {
				profilePictureInBackground.Dispose ();
				profilePictureInBackground = null;
			}
			if (progressAllTeamsActivity != null) {
				progressAllTeamsActivity.Dispose ();
				progressAllTeamsActivity = null;
			}
			if (progressMyActivity != null) {
				progressMyActivity.Dispose ();
				progressMyActivity = null;
			}
			if (progressMyTeamActivity != null) {
				progressMyTeamActivity.Dispose ();
				progressMyTeamActivity = null;
			}
		}
	}
}
