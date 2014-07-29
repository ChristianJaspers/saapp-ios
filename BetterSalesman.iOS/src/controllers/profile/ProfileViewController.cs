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

		private async Task UploadImage(UIImage image)
		{
			if (!Reachability.IsHostReachable(HttpConfig.Host))
			{
				ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
				return;
			}

			ShowHud(I18n.ServiceAccessProfilePictureUpdatingProfilePicture);
			SetHudDetailsLabel(I18n.ServiceAccessProfilePicturePreparingForUploadMessage);

			var maximumImageWithAndHeight = 1024;
			var downsizedImage = ImageManipulationHelper.ResizeImageToMaximumSize(image, maximumImageWithAndHeight, maximumImageWithAndHeight);

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
			DisplayAvatarPlaceholderInProfileImageView();

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
			var uploadCompletedMessage = I18n.ServiceAccessProfilePictureUpdateSuccessfulMessage;
			ShowAlert(uploadCompletedMessage);

			// TODO - add placeholder avatar in offline mode
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

		private void DisplayAvatarPlaceholderInProfileImageView()
		{
			InvokeOnMainThread(() =>
			{
				ProfileImageView.Image = UIImage.FromBundle("avatar_placeholder.png");
			});
		}

		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
