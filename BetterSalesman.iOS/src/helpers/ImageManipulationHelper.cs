using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace BetterSalesman.iOS
{
	/// <summary>
	/// Based on http://forums.xamarin.com/discussion/4170/resize-images-and-save-thumbnails
	/// </summary>
	public class ImageManipulationHelper
	{
		public static UIImage ResizeImageToMaximumSize(UIImage sourceImage, float maxWidth, float maxHeight)
		{
		    var sourceSize = sourceImage.Size;
		    var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
		    
			if (maxResizeFactor > 1) 
			{
				return sourceImage;
			}
		    
			var width = maxResizeFactor * sourceSize.Width;
		    var height = maxResizeFactor * sourceSize.Height;
		    
			UIGraphics.BeginImageContext(new SizeF(width, height));
		    	sourceImage.Draw(new RectangleF(0, 0, width, height));
		    	var resultImage = UIGraphics.GetImageFromCurrentImageContext();
		    UIGraphics.EndImageContext();
			    
			return resultImage;
		}

	}
}

