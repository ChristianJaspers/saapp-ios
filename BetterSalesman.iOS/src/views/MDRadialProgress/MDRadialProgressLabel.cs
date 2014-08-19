using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    public class MDRadialProgressLabel : UILabel
    {
        private RectangleF originalFrame;
        
        public float PointSizeToWidthFactor;
        
        public bool AdjustFontSizeToFitBounds;

        public static NSString KeyThickness = new NSString("Theme.Thickness");
        
        public MDRadialProgressLabel(RectangleF frame, MDRadialProgressTheme theme) : base(frame)
        {
            var center = new PointF(frame.X + frame.Width / 2, frame.Y + frame.Height / 2);
            
            Center = center;
            
            var offset = theme.Thickness;
            
            var sideDimension = Math.Min(frame.Width, frame.Height) - offset;
            var adjustedFrame = new RectangleF(frame.X + offset, frame.Y + offset, sideDimension, sideDimension);
            Bounds = adjustedFrame;
            
            Font = theme.Font;
            TextAlignment = UITextAlignment.Center;
            TextColor = theme.LabelColor;
            PointSizeToWidthFactor = 0.5f;
            AdjustFontSizeToFitBounds = true;
            
            if (theme.DropLabelShadow)
            {
                ShadowColor = theme.LabelShadowColor;
                ShadowOffset = theme.LabelShadowOffset;
            }
            
            Lines = 0;
            AdjustsFontSizeToFitWidth = true;
            
            BackgroundColor = UIColor.Clear;
        }
        
        public override void Draw(RectangleF rect)
        {
            if (AdjustFontSizeToFitBounds)
            {
                var maxWidth = rect.Width * PointSizeToWidthFactor;
                
                while (Font.PointSize > maxWidth)
                {
                    Font = Font.WithSize(Font.PointSize - 1);
                }
            }
            
            base.Draw(rect);
        }
        
        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            base.ObserveValue(keyPath, ofObject, change, context);
            
            if (keyPath.Equals(KeyThickness))
            {
                MDRadialProgressView view = (MDRadialProgressView)ofObject;
                
                float offset = view.Theme.Thickness;
                RectangleF frame = view.Frame;
                float sideDimension = Math.Min(frame.Width, frame.Height) - offset;
                var adjustedFrame = new RectangleF(frame.X + offset, frame.Y + offset, sideDimension, sideDimension);
                Bounds = adjustedFrame;
                
                SetNeedsLayout();
            }
        }
    }
}

