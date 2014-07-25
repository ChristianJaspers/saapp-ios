// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
	public partial class ArgumentsDetail : UIViewController
	{
        public Argument Argument;
        
		public ArgumentsDetail (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            labelFeature.Text = Argument.Feature;
            labelBenefit.Text = Argument.Benefit;
        }
        
        #endregion
	}
}
