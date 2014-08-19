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
	[Register ("ArgumentsDetail")]
	partial class ArgumentsDetail
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISegmentedControl chooseRating { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView labelBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelDescriptionBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelDescriptionFeature { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelEarnXPForVote { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView labelFeature { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelHowRelevant { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView rateContainer { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (chooseRating != null) {
				chooseRating.Dispose ();
				chooseRating = null;
			}
			if (labelBenefit != null) {
				labelBenefit.Dispose ();
				labelBenefit = null;
			}
			if (labelDescriptionBenefit != null) {
				labelDescriptionBenefit.Dispose ();
				labelDescriptionBenefit = null;
			}
			if (labelDescriptionFeature != null) {
				labelDescriptionFeature.Dispose ();
				labelDescriptionFeature = null;
			}
			if (labelEarnXPForVote != null) {
				labelEarnXPForVote.Dispose ();
				labelEarnXPForVote = null;
			}
			if (labelFeature != null) {
				labelFeature.Dispose ();
				labelFeature = null;
			}
			if (labelHowRelevant != null) {
				labelHowRelevant.Dispose ();
				labelHowRelevant = null;
			}
			if (rateContainer != null) {
				rateContainer.Dispose ();
				rateContainer = null;
			}
		}
	}
}
