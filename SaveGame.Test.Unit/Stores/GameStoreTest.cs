using SaveGame.Models;
using SaveGame.Stores;

namespace SaveGame.Test.Unit.Stores
{
    public class GameStoreTest
    {
        private GameStore _gameStore;
        private readonly Game _mario;

        public GameStoreTest()
        {
            _mario = Fixture.Mario();
        }

        [SetUp]
        public void Setup()
        {
            //We want to start every test on a clean state
            _gameStore = new GameStore();
        }

        [Test]
        public void AddToPlay() {
            _gameStore.AddToPlay(_mario);
            var playGames = _gameStore.PlayGames;
            Assert.That(playGames, Has.Count.EqualTo(1));
            Assert.That(playGames.First(), Is.SameAs(_mario));
        }

        [Test]
        public void AddToPlaying()
        {
            _gameStore.AddToPlaying(_mario);
            var playingGames = _gameStore.PlayingGames;
            Assert.That(playingGames, Has.Count.EqualTo(1));
            Assert.That(playingGames.First(), Is.SameAs(_mario));
        }

        [Test]
        public void AddToPlayed()
        {
            _gameStore.AddToPlayed(_mario);
            var playedGames = _gameStore.PlayedGames;
            Assert.That(playedGames, Has.Count.EqualTo(1));
            Assert.That(playedGames.First(), Is.SameAs(_mario));
        }

        [Test]
        public void RemoveGame()
        {
            _gameStore.AddToPlay(_mario);
            _gameStore.Remove(_mario);
            var playGames = _gameStore.PlayGames;
            Assert.That(playGames, Is.Empty);
        }

        [Test]
        public void PlayToPlayedStatus()
        {
            _gameStore.AddToPlay(_mario);
            _gameStore.AddToPlayed(_mario);
            var playGames = _gameStore.PlayGames;
            var playedGames = _gameStore.PlayedGames;
            Assert.Multiple(() =>
            {
                Assert.That(playGames.Count, Is.Empty);
                Assert.That(playedGames, Has.Count.EqualTo(1));
            });
            Assert.That(playedGames.First(), Is.SameAs(_mario));
        }

        [Test]
        public void CannotAddDuplicate()
        {
            _gameStore.AddToPlay(_mario);
            _gameStore.AddToPlay(_mario);
            var playGames = _gameStore.PlayGames;
            Assert.That(playGames, Has.Count.EqualTo(1));
        }
    }
}
