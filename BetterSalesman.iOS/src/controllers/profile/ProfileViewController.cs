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
		private const string MenuIcon = "ic_menu";
		private const string MenuIconPasswordChange = "ic_change_password";

		private const string AvatarPlaceholderImageName = "avatar_placeholder.png";
		private UIImage AvatarPlaceholderImage;

		private ImagePickerPresenter imagePickerPresenter;

		private bool firstTimeViewAppears;

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
            
            imageViewBorder.Image = imageViewBorder.Image.Circle();
            
            AvatarPlaceholderImage = UIImage.FromBundle(AvatarPlaceholderImageName).Circle();

            var menuButton = new UIBarButtonItem(UIImage.FromBundle(MenuIcon), UIBarButtonItemStyle.Plain, delegate
            {
                FlyoutViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
            
            var buttonPasswordChange = new UIBarButtonItem(UIImage.FromBundle(MenuIconPasswordChange), UIBarButtonItemStyle.Plain, delegate
            {
                DisplayPasswordChange(string.Empty,true);
            });
            
            NavigationItem.SetRightBarButtonItem(buttonPasswordChange, true);

			ProfileImageEditButton.TouchUpInside += (s, e) => imagePickerPresenter.ShowImagePickerTypeSelection(this);

			LoadUser();

			firstTimeViewAppears = true;
		}
        
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			if (!firstTimeViewAppears)
			{
				LoadUser();
			}
			else
			{
				firstTimeViewAppears = false;
			}
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

			SetHudDetailsLabel(I18n.ServiceAccessProfilePictureDownloadingThumbnailMessage);
			var imageUrl = new NSUrl(uploadResult.User.AvatarThumbUrl);
			var downloadOptions = SDWebImageOptions.ProgressiveDownload 
					| SDWebImageOptions.ContinueInBackground
					| SDWebImageOptions.RetryFailed;
			InvokeOnMainThread(() =>
			{
				ProfileImageView.SetImage(
					imageUrl,
					downsizedImage, 
					downloadOptions,
					ProcessImageDownloadCompleted
				);
			});
		}
        
        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();
            
            LoadUser();
        }

		private void ProcessImageDownloadCompleted(UIImage downloadedImage, NSError error, SDImageCacheType cacheType)
		{
			HideHud();
            
            ProfileImageView.Image = downloadedImage.Circle();
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
                    
                var downloadOptions = SDWebImageOptions.ProgressiveDownload 
                    | SDWebImageOptions.ContinueInBackground
                    | SDWebImageOptions.RetryFailed;
                    
                ProfileImageView.SetImage(new NSUrl(user.AvatarThumbUrl), AvatarPlaceholderImage, downloadOptions, ProcessImageDownloadCompleted);
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
