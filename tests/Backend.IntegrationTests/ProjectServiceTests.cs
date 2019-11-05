using Jpp.Common.Backend.Auth;
using Jpp.Common.Backend.Projects;
using Jpp.Common.Backend.Projects.Model;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.IntegrationTests
{
    [TestFixture]
    class ProjectServiceTests
    {
        IAuthentication _auth;
        IMessageProvider _message;
        IPhysicalFactory _physicalFactory;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _message = new MessageProviderFake();
            _physicalFactory = new PhysicalFactoryFake();
            _auth = new UnattendedAuthentication(_message, TestContext.Parameters["ClientId"], TestContext.Parameters["ClientSecret"], TestContext.Parameters["Username"], TestContext.Parameters["Password"]);
            await _auth.Authenticate();
        }

        [Test]
        public async Task GetAllProjectsTest()
        {
            ProjectService projectService = new ProjectService(_auth, _message, _physicalFactory);            
            var result = await projectService.GetAllProjects();
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
