using NUnit.Framework;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BetterSalesman.iOS.Tests
{
    [TestFixture]
    public class UserSessionManagerTests
    {
        User stubUser()
        {
            return new User {
                Id = 1,
                Username = "John",
                Email = "Test user",
                Token = "abcdxyz"
            };
        }
        
        [Test]
        public async void SavingUserInManager()
        {
            var userToSave = stubUser();
            
            UserSessionManager.Instance.User = userToSave;
            
            UserSessionManager.Instance.Save();
            
            await UserSessionManager.Instance.FetchUser();
            
            User sessionUser = UserSessionManager.Instance.User;
            
            var serializedA = JsonConvert.SerializeObject(userToSave);
            var serializedB = JsonConvert.SerializeObject(sessionUser);
            
//            Debug.WriteLine("from session: " + serializedA);
//            Debug.WriteLine("from fixture: " + serializedB);
            
            Assert.True(serializedA.Equals(serializedB));
        }
    }
}
