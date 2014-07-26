using System;
using MonoTouch.UIKit;
using System.Diagnostics;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Threading.Tasks;

namespace BetterSalesman.iOS
{
	partial class ProfileViewController : BaseUIViewController
	{
		private ImagePickerPresenter imagePickerPresenter;
		private UIImage cachedPickedImage;
		private User user;

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

			BackButton.TouchUpInside += (sender, e) =>
			{
				DismissViewController (true, null);
			};

			ProfileImageEditButton.TouchUpInside += (sender, @event) => 
			{
				imagePickerPresenter.ShowImagePickerTypeSelection(this);
			};

			LoadUser();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

            // TODO indicator if needed?
//            ShowHud();
			ServiceProviderUser.Instance.Profile(async result =>
				{
					await UserSessionManager.Instance.FetchUser(obj =>
					{
                        LoadUser();
                    });

                    // TODO indicator if needed?
//                    HideHud(); 
				},
				errorCode =>
				{
                    // TODO indicator if needed?
//                    HideHud(); 
					ShowAlert(I18n.ErrorConnectionTimeout);
				});
		}

		private async Task UploadImage(UIImage image)
		{
			cachedPickedImage = image;

			var imageFilePath = await ImageFilesManagementHelper.SharedInstance.SaveImageToTemporaryFilePng(image);
			var mimeType = "image/png";

			ShowHud(I18n.ServiceAccessProfilePictureUpdatingProfilePicture);
			SetHudDetailsLabel(I18n.ServiceAccessProfilePictureUploadingMessage);
			var uploadResult = await ServiceProviderUser.Instance.UpdateAvatar(imageFilePath, mimeType);
			if (!uploadResult.IsSuccess)
			{
				HideHud();
				ShowAlert(uploadResult.Error.LocalizedMessage);
				return;
			}

			SetHudDetailsLabel(I18n.ServiceAccessProfilePictureDownloadingThumbnailMessage);

			var downloadResult = await ImageDownloader.DownloadImage(uploadResult.User.AvatarThumbUrl);
			if (!downloadResult.IsSuccess)
			{
				HideHud();
				ShowAlert(uploadResult.Error.LocalizedMessage);
				return;
			}

			UpdateProfileImageView(downloadResult.Image);

			HideHud();
			var uploadCompletedMessage = I18n.ServiceAccessProfilePictureUpdateSuccessful;
			ShowAlert(uploadCompletedMessage);

			// TODO - reachability error
			// TODO - scale down image
			// TODO - update user (like during login)
			// TODO - add placeholder avatar

		}
						
		private void UpdateProfileImageView(UIImage image)
		{
			InvokeOnMainThread(() =>
			{
				if (image != null)
				{
					ProfileImageView.Image = image;
				}
			});
		}

		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
