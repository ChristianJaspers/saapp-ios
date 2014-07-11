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
        User testUser;
        User sessionUser;
        
        [SetUp]
        public void Init()
        {
            testUser = new User {
                Id = 1,
                Username = "John",
                Email = "Test user",
                Token = "abcdxyz"
            };
            
            sessionUser = null;
            
            UserSessionManager.Instance.User = testUser;

            UserSessionManager.Instance.Save();
        }
        
        [Test]
        public async void SavingUserInManager()
        {   
            await UserSessionManager.Instance.FetchUser();

            sessionUser = UserSessionManager.Instance.User;
            
            Assert.True(sessionUser != null);
        }
        
        [Test]
        public async void SavingSameDataInManager()
        {   
            await UserSessionManager.Instance.FetchUser();

            sessionUser = UserSessionManager.Instance.User;
            
            var serializedA = JsonConvert.SerializeObject(testUser);
            var serializedB = JsonConvert.SerializeObject(sessionUser);

            Assert.True(serializedA.Equals(serializedB));   
        }
    }
}
