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
		private UserSession stubSession;

		public UserSessionManagerTests()
		{
			stubSession = new UserSession
			{
				UserId = 11,
				Token = "test_token"
			};
		}

		[SetUp]
		public void Setup()
		{
			UserSessionManager.Instance.SaveSession(stubSession.UserId, stubSession.Token);
		}

		[TearDown]
		public void TearDown()
		{
			UserSessionManager.Instance.Discard();
		}

        [Test]
        public async void TestCurrentSessionIsNotNull()
        {
			var currentSession = await UserSessionManager.Instance.LoadSessionAsync();
            
			Assert.IsNotNull(currentSession);
        }
        
        [Test]
        public async void TestCurrentSessionIsEqualToSavedSession()
        {
			var storedSession = await UserSessionManager.Instance.LoadSessionAsync();

			var serializedStubSession = JsonConvert.SerializeObject(stubSession);
			var serializedStoredSession = JsonConvert.SerializeObject(storedSession);

			Assert.AreEqual(serializedStubSession, serializedStoredSession);
        }
    }
}
