using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using System;

namespace BetterSalesman.iOS
{    
    public class MDRadialProgressView : UIView
    {        
        
        uint internalPadding;
        
        private MDRadialProgressTheme theme;
        public MDRadialProgressTheme Theme
        {
            set {
                theme = value;
                SetNeedsDisplay();
            }
            get { 
                return theme; 
            }
        }
        
        private uint progressTotal;
        public uint ProgressTotal { 
            get {
                return progressTotal;
            }
            set {
                progressTotal = value;
                notifyProgressChange();
                InvokeOnMainThread(SetNeedsDisplay);
            }
        }
        
        private uint progressCounter;
        public uint ProgressCounter { 
            get {
                return progressCounter;
            }
            set {
                progressCounter = value;
                notifyProgressChange();
                InvokeOnMainThread(SetNeedsDisplay);
            }
        }
        
        const float M_PI = (float)Math.PI;
        const float M_PI_2 = (float)(Math.PI / 2);
        
        public bool Clockwise;
        
        public uint StartingSlice;
        
        public MDRadialProgressLabel Label;
        
        public MDRadialProgressView(RectangleF frame) : base(frame)
        {
            InternalInitWithTheme(MDRadialProgressTheme.StandardTheme());
        }
        
        public MDRadialProgressView(RectangleF frame, MDRadialProgressTheme theme) : base(frame)
        {
            InternalInitWithTheme(theme);
        }
        
        public MDRadialProgressView(NSCoder decoder) : base(decoder)
        {
            InternalInitWithTheme(MDRadialProgressTheme.StandardTheme());
        }
        
        void InternalInitWithTheme(MDRadialProgressTheme theme)
        {
            ProgressTotal = 1;
            ProgressCounter = 0;
            StartingSlice = 1;
            Clockwise = true;
            
            Theme = theme;
            
            Label = new MDRadialProgressLabel(Bounds, Theme);
            Add(Label);
            
            internalPadding = 2;
            IsAccessibilityElement = true;
            
            // TODO extract to i18n
            AccessibilityLabel = "Progress".t();
            
            BackgroundColor = UIColor.Clear;
            
//            AddObserver(Label, MDRadialProgressLabel.KeyThickness, NSKeyValueObservingOptions.New, IntPtr.Zero);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
//            RemoveObserver(Label, MDRadialProgressLabel.KeyThickness);
        }
        
        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);
            
            CGContext contextRef = UIGraphics.GetCurrentContext();
            SizeF viewSize = Bounds.Size;
            PointF center = new PointF(viewSize.Width / 2, viewSize.Height / 2);

            double radius = Math.Min(viewSize.Width, viewSize.Height) / 2 - internalPadding;
            
            drawSlicesWithRadius((float)radius, center, contextRef);
            
            if (!Theme.SliceDividerHidden)
            {
                drawSlicesSeparators(contextRef, viewSize, center);
            }

            drawCenter(contextRef, viewSize, center);
        }


        void drawSlicesWithRadius(float radius, PointF center, CGContext contextRef)
        {
            bool cgClockwise = !Clockwise;
            
            uint startingSlice = StartingSlice - 1;
            
            if (ProgressCounter == 0 && Theme.DrawIncompleteArcIfNoProgress)
            {
                DrawArcInContext(contextRef, center, radius, 0, M_PI * 2, Theme.IncompletedColor.CGColor, cgClockwise);
                return;
            }
            
            if (!Theme.SliceDividerHidden && Theme.SliceDividerThickness > 0)
            {
                float sliceAngle = (2 * M_PI) / ProgressTotal;
                
                for (int i =0; i < ProgressTotal; i++) {
                    float startValue = (sliceAngle * i) + sliceAngle * startingSlice;
                    float startAngle, endAngle;
                    if (Clockwise)
                    {
                        startAngle = -M_PI_2 + startValue;
                        endAngle = startAngle + sliceAngle;
                    } 
                    else
                    {
                        startAngle = -M_PI_2 - startValue;
                        endAngle = startAngle - sliceAngle;
                    }
                    
                    contextRef.BeginPath();
                    contextRef.MoveTo(center.X, center.Y);
                    contextRef.AddArc(center.X, center.Y, radius, startAngle, endAngle, cgClockwise);

                    CGColor color = Theme.IncompletedColor.CGColor;

                    if (i < ProgressCounter)
                    {
                        color = Theme.CompletedColor.CGColor;
                    }
                    
                    contextRef.SetFillColor(color);
                    contextRef.FillPath();
                }
            } 
            else 
            {
                double originAngle, endAngle;
                double sliceAngle = (2 * M_PI) / ProgressTotal;
                double startingAngle = sliceAngle * startingSlice;
                double progressAngle = sliceAngle * ProgressCounter;

                if (Clockwise) {
                    originAngle = - M_PI_2 + startingAngle;
                    endAngle = originAngle + progressAngle;
                } else {
                    originAngle = - M_PI_2 - startingAngle;
                    endAngle = originAngle - progressAngle;
                }
                
                DrawArcInContext(contextRef, center, radius, (float)originAngle, (float)endAngle, Theme.CompletedColor.CGColor, cgClockwise);
                
                if (ProgressCounter < ProgressTotal)
                {
                    DrawArcInContext(contextRef, center, radius, (float)endAngle, (float)originAngle, Theme.IncompletedColor.CGColor, cgClockwise);
                }
            }
        }

        void drawSlicesSeparators(CGContext contextRef, SizeF viewSize, PointF center)
        {
            int outerDiameter = (int)Math.Min(viewSize.Width, viewSize.Height);
            
            double outerRadius = outerDiameter / 2.0 - internalPadding;
            int innerDiameter = (int)(outerDiameter - Theme.Thickness);
            double innerRadius = innerDiameter / 2.0;
            int sliceCount = (int)ProgressTotal;
            double sliceAngle = (2 * M_PI) / sliceCount;
            
            contextRef.SetLineWidth(Theme.SliceDividerThickness);
            contextRef.SetStrokeColor(Theme.SliceDividerColor.CGColor);
            contextRef.MoveTo(center.X, center.Y);
            
            for (int i = 0; i < sliceCount; i++) {
                contextRef.BeginPath();
                
                double startAngle = sliceAngle * i - M_PI_2;
                double endAngle = sliceAngle * (i + 1) - M_PI_2;
                
                contextRef.AddArc(center.X, center.Y, (float)outerRadius, (float)startAngle, (float)endAngle, false);
                contextRef.AddArc(center.X, center.Y, (float)innerRadius, (float)endAngle, (float)startAngle, true);
                contextRef.StrokePath();
            }
        }

        
        void drawCenter(CGContext contextRef, SizeF viewSize, PointF center)
        {
            int innerDiameter = (int)(Math.Min(viewSize.Width, viewSize.Height) - Theme.Thickness);
            double innerRadius = innerDiameter / 2.0;
            
            contextRef.SetLineWidth(Theme.Thickness);
            
            RectangleF innerCircle = new RectangleF((float)(center.X - innerRadius), (float)(center.Y - innerRadius), (float)innerDiameter, (float)innerDiameter);
            
            contextRef.AddEllipseInRect(innerCircle);
            contextRef.Clip();
            contextRef.ClearRect(innerCircle);
            contextRef.SetFillColor(Theme.CenterColor.CGColor);
            contextRef.FillRect(innerCircle);
        }
        
        void DrawArcInContext(CGContext context, PointF center, float radius, float startAngle, float endAngle, CGColor color, bool cgClockwise)
        {
            context.BeginPath();
            context.MoveTo(center.X, center.Y);
            context.AddArc(center.X, center.Y, radius, startAngle, endAngle, cgClockwise);
            context.SetFillColor(color);
            context.FillPath();
        }
        
        public override UIAccessibilityTrait AccessibilityTraits
            {
            get
            {
                return base.AccessibilityTraits | UIAccessibilityTrait.AllowsDirectInteraction;
            }
            set
            {
                base.AccessibilityTraits = value;
            }
        }
        
        // TODO labelTextBlock
        //        public virtual Action<string> LabelTextBlock(MDRadialProgressView progressView);
        void notifyProgressChange()
        {
            string text;
            
//            if (LabelTextBlock)
//            {
//                text = LabelTextBlock(this);
//            } 
//            else
//            {
                float percentageCompleted = (100.0f / ProgressTotal) * ProgressCounter;
                text = string.Format("{0}", percentageCompleted);
//            }

            AccessibilityValue = text;
//            Label.Text = text;

            // TODO this notofication
//            NSString *notificationText = [NSString stringWithFormat:@"%@ %@",
//                NSLocalizedString(@"Progress changed to:", nil),
//                self.accessibilityValue];
//            UIAccessibilityPostNotification(UIAccessibilityAnnouncementNotification, notificationText);
        }
    }

}

