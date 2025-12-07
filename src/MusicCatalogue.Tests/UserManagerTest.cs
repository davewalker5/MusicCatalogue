using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class UserManagerTest
    {
        private const string UserName = "Some User";
        private const string Password = "password";
        private const string UpdatedPassword = "a new password";

        private IUserManager? _userManager;
        private int _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _userManager = new MusicCatalogueFactory(context).Users;
            _userId = Task.Run(() => _userManager.AddAsync(UserName, Password)).Result.Id;
        }

        [TestMethod]
        public void AddAndGetUserTest()
        {
            var user = Task.Run(() => _userManager!.GetAsync(x => x.Id == _userId)).Result;

            Assert.IsNotNull(user);
            Assert.AreEqual(UserName, user.UserName);
            Assert.AreNotEqual(Password, user.Password);
        }

        [TestMethod]
        public void AddDuplicateUserTest()
        {
            Task.Run(() => _userManager!.AddAsync(UserName, Password)).Wait();
            var users = Task.Run(() => _userManager!.ListAsync(x => true)).Result;

            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(UserName, users.First().UserName);
            Assert.AreNotEqual(Password, users.First().Password);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            Task.Run(() => _userManager!.DeleteAsync(UserName)).Wait();
            var users = Task.Run(() => _userManager!.ListAsync(x => true)).Result;

            Assert.IsFalse(users.Any());
        }

        [TestMethod]
        public void GetMissingUserTest()
        {
            var user = Task.Run(() => _userManager!.GetAsync(x => x.Id == -1)).Result;
            Assert.IsNull(user);
        }

        [TestMethod]
        public void ListAllUsersTest()
        {
            var users = Task.Run(() => _userManager!.ListAsync(x => true)).Result;

            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(UserName, users.First().UserName);
            Assert.AreNotEqual(Password, users.First().Password);
        }

        [TestMethod]
        public void AuthenticateTest()
        {
            bool authenticated = Task.Run(() => _userManager!.AuthenticateAsync(UserName, Password)).Result;
            Assert.IsTrue(authenticated);
        }

        [TestMethod]
        public void FailedAuthenticationTest()
        {
            bool authenticated = Task.Run(() => _userManager!.AuthenticateAsync(UserName, "the wrong password")).Result;
            Assert.IsFalse(authenticated);
        }

        [TestMethod]
        public void SetPassswordTest()
        {
            Task.Run(() => _userManager!.SetPasswordAsync(UserName, UpdatedPassword)).Wait();
            bool authenticated = Task.Run(() => _userManager!.AuthenticateAsync(UserName, UpdatedPassword)).Result;
            Assert.IsTrue(authenticated);
        }
    }
}
