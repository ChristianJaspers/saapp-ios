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
	[Register ("ProductGroupPickerButtonViewController")]
	partial class ProductGroupPickerButtonViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProductGroupPickerButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ProductGroupPickerButton != null) {
				ProductGroupPickerButton.Dispose ();
				ProductGroupPickerButton = null;
			}
		}
	}
}
