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
	[Register ("ArgumentFormViewController")]
	partial class ArgumentFormViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView fieldBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField fieldFeature { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imageViewBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imageViewFeature { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelFeature { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (fieldBenefit != null) {
				fieldBenefit.Dispose ();
				fieldBenefit = null;
			}
			if (fieldFeature != null) {
				fieldFeature.Dispose ();
				fieldFeature = null;
			}
			if (imageViewBenefit != null) {
				imageViewBenefit.Dispose ();
				imageViewBenefit = null;
			}
			if (imageViewFeature != null) {
				imageViewFeature.Dispose ();
				imageViewFeature = null;
			}
			if (labelBenefit != null) {
				labelBenefit.Dispose ();
				labelBenefit = null;
			}
			if (labelFeature != null) {
				labelFeature.Dispose ();
				labelFeature = null;
			}
		}
	}
}
