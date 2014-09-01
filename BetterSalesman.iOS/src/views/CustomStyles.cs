using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public static class CustomStyles
    {
        const string imageBg = "progress_bg";
        const string imageFull = "progress_full";
        
        public static void Init()
        {
            var minImage = UIImage.FromBundle(imageFull).StretchableImage(3,3);
            var maxImage = UIImage.FromBundle(imageBg).StretchableImage(3,3);

            UISlider.Appearance.SetThumbImage(new UIImage(), UIControlState.Normal);
            UISlider.Appearance.SetMinTrackImage(minImage, UIControlState.Normal);
            UISlider.Appearance.SetMaxTrackImage(maxImage, UIControlState.Normal);

            UINavigationBar.Appearance.TintColor = AppDelegate.ColorTextGreen;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
                TextColor = AppDelegate.ColorTextGreen,
                TextShadowOffset = new UIOffset(0,0),
                Font = UIFont.FromName("HelveticaNeue-Light",20),
            });
        }
    }
}

