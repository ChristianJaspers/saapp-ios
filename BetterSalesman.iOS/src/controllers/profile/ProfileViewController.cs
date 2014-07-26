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

			var uploader = new HttpClientFileUploader(HttpRequest.AuthorizationToken);

			var uploadUrl = HttpConfig.ApiBaseAddress + "profile/avatar";
			var parameterName = "file";
			var mimeType = "image/png";

			ShowHud("Uploading image");
			var result = await uploader.UploadFileAsync(uploadUrl, imageFilePath, parameterName, mimeType, HttpClientFileUploader.HttpMethodPut);
			Debug.WriteLine("Finished uploading image");

			HideHud();

			var uploadCompletedMessage = I18n.ServiceAccessProfilePictureUpdateSuccessful;
			if (!result.IsSuccess)
			{
				uploadCompletedMessage = result.Error.LocalizedMessage;
			}

			ShowAlert(uploadCompletedMessage);

			// TODO - file not found error
			// TODO - reachability error
			// TODO - scale down image
			// TODO - download thumbnail
			// TODO - update user (like during login)

		}
						
		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
