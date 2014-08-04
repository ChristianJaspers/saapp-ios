using System;
using MonoTouch.UIKit;
using SDWebImage;
using System.Diagnostics;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Threading.Tasks;
using XValidator;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    partial class ProfileViewController : BaseUIViewController
	{
		private const string menu_icon = "ic_menu";
		private const string menu_password_change = "ic_change_password";

		private const string PlaceholderImageName = "avatar_placeholder.png";
		private UIImage PlaceholderImage = UIImage.FromBundle(PlaceholderImageName);

		private ImagePickerPresenter imagePickerPresenter;

		public ProfileViewController(IntPtr handle) : base (handle)
		{
			imagePickerPresenter = new ImagePickerPresenter ();
			imagePickerPresenter.FinishedPicking += (didPickAnImage, pickedImage) =>
            {
                Task.Run(async () =>
                    {
                        if (didPickAnImage)
                        {
                            await UploadImage(pickedImage);
                        }
                    });
            };
		}
        
        #region Lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            
            Title = I18n.Profile;
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                FlyoutViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
            
            var buttonPasswordChange = new UIBarButtonItem(UIImage.FromBundle(menu_password_change), UIBarButtonItemStyle.Plain, delegate
            {
                DisplayPasswordChange(string.Empty,true);
            });
            
            NavigationItem.SetRightBarButtonItem(buttonPasswordChange, true);

			ProfileImageEditButton.TouchUpInside += (s, e) => imagePickerPresenter.ShowImagePickerTypeSelection(this);

			LoadUser();
		}
        
        #endregion

		private async Task UploadImage(UIImage image)
		{
			if (!IsNetworkAvailable())
			{
				ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
				return;
			}

			ShowHud(I18n.ServiceAccessProfilePictureUpdatingProfilePicture);
			SetHudDetailsLabel(I18n.ServiceAccessProfilePicturePreparingForUploadMessage);

			var maximumImageWidthAndHeight = 1024;
			var downsizedImage = ImageManipulationHelper.ResizeImageToMaximumSize(image, maximumImageWidthAndHeight, maximumImageWidthAndHeight);

			var imageFilePath = await ImageFilesManagementHelper.SharedInstance.SaveImageToTemporaryFileJpeg(downsizedImage, 0.75f);
			var mimeType = "image/jpeg";

			SetHudDetailsLabel(I18n.ServiceAccessProfilePictureUploadingMessage);

			var uploadResult = await ServiceProviderUser.Instance.UpdateAvatar(imageFilePath, mimeType);
			await ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
			if (!uploadResult.IsSuccess)
			{
				HideHud();
				ShowAlert(uploadResult.Error.LocalizedMessage);
				return;
			}
				
			InvokeOnMainThread(() =>
			{
				ProfileImageView.SetImage(new NSUrl(uploadResult.User.AvatarThumbUrl), 
					downsizedImage, 
					SDWebImageOptions.ProgressiveDownload 
					& SDWebImageOptions.ContinueInBackground,
					(UIImage downloadedImage, NSError error, SDImageCacheType cacheType) =>
					{
						InvokeOnMainThread(() =>
						{
							HideHud();
							var uploadCompletedMessage = I18n.ServiceAccessProfilePictureUpdateSuccessfulMessage;
							ShowAlert(uploadCompletedMessage);
						});
					}
				);
			});
		}

		private void LoadUser()
		{
			var user = UserManager.LoggedInUser();
            
			if (user == null)
			{
				Debug.WriteLine("Can't display profile information - LoggedInUser is null");
				return;
			}

			InvokeOnMainThread(() =>
			{
				labelDisplayName.Text = user.DisplayName;
				labelExperience.Text = user.Experience.ToString();
				labelMyActivity.Text = user.MyActivity.ToString();
				labelMyTeamActivity.Text = user.MyTeamActivity.ToString();
				labelAllTeamsActivity.Text = user.AllTeamsActivity.ToString();
				ProfileImageView.SetImage(new NSUrl(user.AvatarThumbUrl), PlaceholderImage);
			});
		}
        
        #region Password change
        
        XForm<UITextField> validator;

        void DisplayPasswordChange(string defaultPassword = "", bool firstTime = false)
        {   
            UIAlertView alert = new UIAlertView(I18n.ProvideNewPassword, I18n.ProvideNewPasswordRequirement, null, I18n.OK, I18n.Cancel);
            
            alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
            
            var passwordField = alert.GetTextField(0);
            
            if ( !firstTime )
            {
                passwordField.Text = defaultPassword;
                ValidateField(passwordField);
            }
            
            alert.Clicked += (sender, e) =>
            {
                if ( e.ButtonIndex == 0 )
                {       
                    if ( ValidateField(passwordField) )
                    {
                        UpdatePassword(passwordField.Text);
                    }
                    else
                    {
                        ShowAlert( string.Join("\n", validator.Errors),() => DisplayPasswordChange(passwordField.Text) );
                    }
                }
            };
            
            alert.Show();
        }
        
        bool ValidateField(UITextField field)
        {
            validator = new XForm<UITextField> {
                Inputs = new [] {
                    new XUITextField {
                        Name = I18n.FieldPassword,
                        FieldView = field,
                        Validators = new [] {
                            new XValidatorLengthMinimum(5) {
                                Message = I18n.ValidationLengthMinimum   
                            }
                        },
                    }
                }
            };
            
            return validator.Validate();
        }
        
        void UpdatePassword(string newPassword)
        {
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                return;
            }
            
            ShowHud();

            ServiceProviderUser.Instance.PasswordChange(
                newPassword,
                result => 
                {
                    HideHud();
                    ShowAlert(I18n.SuccessMessagePasswordChange);
                },
                errorMessage =>
                {
                    HideHud();
                    ShowAlert(errorMessage);
                }
            );
        }
        
        #endregion
	}
}
