using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models.Out;
using ContractSystem.Repositories;
using ContractSystem.Service;
using Mapster;
using Moq;

namespace ContractSystemTests
{
    public class UserServiceTests
    {
        Mock<IUserRepository> _mockUserRepisitory;
        UserService _userService;

        [SetUp]
        public void Setup()
        {
            //TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
            _mockUserRepisitory = new Mock<IUserRepository>();
            _userService = new UserService((IUserRepository)_mockUserRepisitory.Object);
        }

        [Test]
        public void Test1()
        {
            _mockUserRepisitory
                .Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(new UserDTO() { Id = 1});
            
            var expected = new UserOut() { Id = 1 };

            var actual = _userService.getById(1);
            
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
        }
    }
}