using Core.Models.Topics;
using Core.Models.Users;

namespace Core.UnitTests.Topics
{
    public abstract class ApplicationsTestBase
    {
        protected readonly Topic topic = FakeDataGenerator.GenerateTopic();
        protected readonly Student submitter = FakeDataGenerator.GenerateStudent();
        protected readonly Application application;

        public ApplicationsTestBase()
        {
            application = FakeDataGenerator.GenerateApplication(topic, submitter);
        }
    }
}
