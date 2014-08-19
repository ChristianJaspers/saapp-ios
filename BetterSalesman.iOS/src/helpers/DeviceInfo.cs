using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public static class DeviceInfo
    {
        const int IPhoneTallHeight = 1136;
        
        public static bool IsIPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }
        
        public static bool IsIPhoneTall
        {
            get
            {
                return IsIPhone && UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= IPhoneTallHeight;
            }
        }
    }
}

