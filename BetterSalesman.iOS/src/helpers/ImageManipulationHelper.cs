using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace BetterSalesman.iOS
{
	/// <summary>
	/// Based on http://forums.xamarin.com/discussion/4170/resize-images-and-save-thumbnails
	/// </summary>
	public static class ImageManipulationHelper
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

        public static UIImage RoundCorners (this UIImage image, float roundnessPercentage)
        {
            float width = image.Size.Width;
            float height = image.Size.Height;
            float radius = ((width+height)/2) * (roundnessPercentage/(100*2));

            UIGraphics.BeginImageContext (new SizeF (width, height));
            CGContext c = UIGraphics.GetCurrentContext();

            c.BeginPath ();
            c.MoveTo(width, height/2);
            //Bottom-right Corner
            c.AddArcToPoint(width, height, height / 2, width, radius);
            //Bottom-left Corner
            c.AddArcToPoint(0, height, 0, 0, radius);
            //Top-left Corner
            c.AddArcToPoint(0, 0, width/2, 0, radius);
            //Top-right Corner
            c.AddArcToPoint(width, 0, width, height/2, radius);
            c.ClosePath();
            c.Clip();

            image.Draw (new PointF (0, 0));
            UIImage converted = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext ();
            return converted;
        }
        
        public static UIImage Circle(this UIImage image)
        {
            return image.CropToMaxSquare().RoundCorners(100);
        }

        public static UIImage CropToMaxSquare(this UIImage image)
        {
            int width = (int)image.Size.Width;
            int height = (int)image.Size.Height;

            int newWidth = width;
            int newHeight = height;

            int crop_x = 0;
            int crop_y = 0;

            if (width > height)
            {
                crop_x = (int)(width - height) / 2;
                newWidth = height;
            } 
            else
            {
                crop_y = (int)(height - width) / 2;
                newHeight = width;
            }

            return image.CropImage(crop_x, crop_y, newWidth, newHeight);
        }

        public static UIImage CropImage(this UIImage sourceImage, int crop_x, int crop_y, int width, int height)
        {
            var imgSize = sourceImage.Size;
            UIGraphics.BeginImageContext(new SizeF(width, height));
            var context = UIGraphics.GetCurrentContext();
            var clippedRect = new RectangleF(0, 0, width, height);
            context.ClipToRect(clippedRect);
            var drawRect = new RectangleF(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
            sourceImage.Draw(drawRect);
            var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return modifiedImage;
        }
	}
}

