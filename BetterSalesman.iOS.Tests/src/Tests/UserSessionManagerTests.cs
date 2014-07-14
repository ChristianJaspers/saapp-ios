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
        UserSession testUser;
        UserSession sessionUser;
        
        [SetUp]
        public void Init()
        {
            testUser = new UserSession {
                Token = "abcdxyz"
            };
            
            sessionUser = null;
            
            UserSessionManager.Instance.User = testUser;

            UserSessionManager.Instance.Save();
        }
        
        [Test]
        public async void TestSavingUserInManager()
        {   
            await UserSessionManager.Instance.FetchUser();

            sessionUser = UserSessionManager.Instance.User;
            
            Assert.IsNotNull(sessionUser);
        }
        
        [Test]
        public async void TestSavingSameDataInManager()
        {   
            await UserSessionManager.Instance.FetchUser();

            sessionUser = UserSessionManager.Instance.User;
            
            var serializedA = JsonConvert.SerializeObject(testUser);
            var serializedB = JsonConvert.SerializeObject(sessionUser);

            Assert.AreEqual(serializedA,serializedB);   
        }
    }
}
