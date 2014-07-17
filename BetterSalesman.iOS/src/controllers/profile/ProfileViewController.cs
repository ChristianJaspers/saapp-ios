using System;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
    partial class ProfileViewController : BaseUIViewController
    {
        User user;
        
        public ProfileViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
            
            // TODO load user from DB here
            // user = 
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            ShowIndicator();

            ServiceProviderUser.Instance.Profile(
                result =>
                {
                    // TODO load user
                    HideIndicator();
                },
                errorCode =>
                {
                    HideIndicator();
                    // TODO there was a problem   
                    ShowAlert(I18n.ErrorConnectionTimeout);
                }
            );
        }
    }
}
