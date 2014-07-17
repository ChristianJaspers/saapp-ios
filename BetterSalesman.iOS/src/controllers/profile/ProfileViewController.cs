using System;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;

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
            
            LoadUser();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            ShowIndicator();

            ServiceProviderUser.Instance.Profile(
                result =>
                {
                    UserSessionManager.Instance.FetchUser(obj =>
                        {
                            LoadUser();
                            // TODO refresh UI here?
                        });
                    
                    HideIndicator();
                },
                errorCode =>
                {
                    HideIndicator();  
                    ShowAlert(I18n.ErrorConnectionTimeout);
                }
            );
        }

        void LoadUser()
        {
            user = UserManager.LoggedInUser();
        }
    }
}
