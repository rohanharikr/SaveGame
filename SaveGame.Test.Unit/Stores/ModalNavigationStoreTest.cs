using SaveGame.Models;
using SaveGame.Stores;

namespace SaveGame.Test.Unit.Stores
{
    public class ModalNavigationStoreTest : BaseTest
    {
        private ModalNavigationStore _modalNavigationStore;
        private readonly Game _mario;

        public ModalNavigationStoreTest()
        {
            _mario = Fixture.Mario();
        }

        [SetUp]
        public void Setup()
        {
            //We want to start every test on a clean state
            _modalNavigationStore = new ModalNavigationStore();
        }

        [Test]
        public void Shows()
        {
            _modalNavigationStore.Show(_mario);
            Assert.Multiple(() =>
            {
                Assert.That(_modalNavigationStore.Detail, Is.Not.Null);
                Assert.That(_modalNavigationStore.IsOpen);
            });
        }

        [Test]
        public void Closes()
        {
            _modalNavigationStore.Close();
            Assert.Multiple(() =>
            {
                Assert.That(_modalNavigationStore.Detail, Is.Null);
                Assert.That(_modalNavigationStore.IsOpen, Is.False);
            });
        }
    }
}
