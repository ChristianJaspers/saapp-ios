using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public static class CustomStyles
    {
        const string imageBg = "progress_bg";
        const string imageFill = "progress_full";
        
        const string FontLight = "HelveticaNeue-Light";
        
        public static void Init()
        {
            var fillImage = UIImage.FromBundle(imageFill).StretchableImage(3,3);
            var bgImage = UIImage.FromBundle(imageBg).StretchableImage(3,3);

            UISlider.Appearance.SetThumbImage(new UIImage(), UIControlState.Normal);
            UISlider.Appearance.SetMinTrackImage(fillImage, UIControlState.Normal);
            UISlider.Appearance.SetMaxTrackImage(bgImage, UIControlState.Normal);

            UINavigationBar.Appearance.TintColor = AppDelegate.ColorTextGreen;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
                TextColor = AppDelegate.ColorTextGreen,
                TextShadowOffset = new UIOffset(0,0),
                Font = UIFont.FromName(FontLight,20),
            });
        }
    }
}

