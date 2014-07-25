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
		UILabel labelBenefit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelFeature { get; set; }

		void ReleaseDesignerOutlets ()
		{
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
