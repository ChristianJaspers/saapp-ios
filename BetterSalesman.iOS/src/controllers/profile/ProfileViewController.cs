using System;
using MonoTouch.UIKit;
using SDWebImage;
using System.Diagnostics;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Threading.Tasks;
using XValidator;

namespace BetterSalesman.iOS
{
    partial class ProfileViewController : BaseUIViewController
	{
		private ImagePickerPresenter imagePickerPresenter;
		private string currentProfilePictureUrl = null;

		const string PlaceholderImage = "avatar_placeholder.png";

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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			BackButton.Clicked += (s, e) => DismissViewController(true, null);
            
            buttonPasswordChange.Clicked += (s, e) => DisplayPasswordChange(string.Empty,true);

			ProfileImageEditButton.TouchUpInside += (s, e) => imagePickerPresenter.ShowImagePickerTypeSelection(this);

			LoadUser();
		}

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
			if (!uploadResult.IsSuccess)
			{
				await ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
				HideHud();
				ShowAlert(uploadResult.Error.LocalizedMessage);
				return;
			}

			await ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);

			SetHudDetailsLabel(I18n.ServiceAccessProfilePictureDownloadingThumbnailMessage);

			var downloadResult = await ImageDownloader.DownloadImage(uploadResult.User.AvatarThumbUrl);
			if (!downloadResult.IsSuccess)
			{
				DisplayAvatarPlaceholderInProfileImageView();
				HideHud();
				ShowAlert(uploadResult.Error.LocalizedMessage);
				return;
			}

			UpdateProfileImageView(downloadResult.Image);

			HideHud();
			var uploadCompletedMessage = I18n.ServiceAccessProfilePictureUpdateSuccessfulMessage;
			ShowAlert(uploadCompletedMessage);
		}
						
		private void UpdateProfileImageView(UIImage image)
		{
			InvokeOnMainThread(() =>
			{
				if (image != null)
				{
					// TODO use async image loading
					ProfileImageView.Image = image;
				}
			});
		}

		private void DisplayAvatarPlaceholderInProfileImageView()
		{
			InvokeOnMainThread(() =>
			{
				ProfileImageView.Image = UIImage.FromBundle(PlaceholderImage);
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
			});

			UpdateProfilePicture(user);
		}

		private void UpdateProfilePicture(User user)
		{
			if (!ShouldDownloadProfilePicture(user))
			{
				return;
			}

			if (IsNetworkAvailable())
			{
				Task.Run(async () =>
				{
					var downloadResult = await ImageDownloader.DownloadImage(user.AvatarThumbUrl);
					if (downloadResult.IsSuccess)
					{
						UpdateProfileImageView(downloadResult.Image);
						currentProfilePictureUrl = user.AvatarThumbUrl;
					}
					else
					{
						DisplayAvatarPlaceholderInProfileImageView();
					}
				});
			}
			else
			{
				DisplayAvatarPlaceholderInProfileImageView();
			}
		}

		private bool ShouldDownloadProfilePicture(User user)
		{
			return currentProfilePictureUrl == null || !currentProfilePictureUrl.Equals(user.AvatarThumbUrl);
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
