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

//			var fileUploadRequest = new FileUploadRequest();
//
//			fileUploadRequest.ProgressUpdated += progressPercentage =>
//            {
//                Debug.WriteLine("Upload progress: " + progressPercentage);
//
//                SetHudProgress(progressPercentage / 100.0f);
//            };
//
//			fileUploadRequest.Success += async remoteFileUrl =>
//            {
//                ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
//
//                InvokeOnMainThread(() =>
//                    {
//                        Debug.WriteLine("Upload complete - file URL: " + remoteFileUrl);
//                        ProfileImageView.Image = cachedPickedImage;
//                        cachedPickedImage = null;
//                    });
//
//                HideHud();
//            };
//
//			fileUploadRequest.Failure += (errorCode, errorMessage) =>
//            {
//                ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
//                cachedPickedImage = null;
//
//                InvokeOnMainThread(() => ShowAlert(errorMessage));
//
//                HideHud();
//            };
//
//			ShowHud("Updating profile picture");
//			fileUploadRequest.Perform(imageFilePath);
		}
						
		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
