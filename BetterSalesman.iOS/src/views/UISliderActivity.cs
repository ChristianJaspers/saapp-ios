// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	public sealed partial class UISliderActivity : UISlider
	{   
		public UISliderActivity (IntPtr handle) : base (handle)
		{
		}
        
        public override RectangleF TrackRectForBounds(System.Drawing.RectangleF forBounds)
        {
            var bounds = base.TrackRectForBounds(forBounds);
            
            bounds.Height = 14;
            
            return bounds;
        }
	}
}
