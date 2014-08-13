using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
	public class MDRadialProgressTheme : NSObject
	{
        public float Thickness;
        public UIFont Font;
        public UIColor LabelColor;
        public bool DropLabelShadow;
        public UIColor LabelShadowColor;
        public SizeF LabelShadowOffset;
        public UIColor CompletedColor;
        public UIColor IncompletedColor;
        public UIColor CenterColor;
        public UIColor SliceDividerColor;
        public bool SliceDividerHidden;
        public bool DrawIncompleteArcIfNoProgress;
        public uint SliceDividerThickness;
        
        const int MaxFontSize = 64;
        const string StandardThemeIdentifier = "standard";
        
        public static MDRadialProgressTheme WithTheme(string themeName)
        {
            return new MDRadialProgressTheme();
        }
        
        public static MDRadialProgressTheme StandardTheme()
        {
            return MDRadialProgressTheme.WithTheme(StandardThemeIdentifier);
        }

        public MDRadialProgressTheme()
        {
            CompletedColor = UIColor.Green;
            IncompletedColor = UIColor.FromRGB(0.8f, 0.8f, 0.8f);
            SliceDividerColor = UIColor.White;
            CenterColor = UIColor.Clear;
            Thickness = 15;
            SliceDividerHidden = false;
            SliceDividerThickness = 2;
            DrawIncompleteArcIfNoProgress = false;
            
            LabelColor = UIColor.Black;
            DropLabelShadow = true;
            LabelShadowColor = UIColor.FromRGB(247 / 255, 247 / 255, 247 / 255);
            LabelShadowOffset = new SizeF(1, 1);
            Font = UIFont.SystemFontOfSize(MaxFontSize);
        }
	}
}
