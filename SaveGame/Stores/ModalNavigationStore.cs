using IGDB.Models;

namespace SaveGame.Stores
{
    public class ModalNavigationStore
    {
        private Game? _detail;
        public Game? Detail
        {
            get { return _detail; }
            set
            {
                _detail = value;
                DetailChanged?.Invoke();
            }
        }

        public bool IsOpen => _detail != null;

        public event Action? DetailChanged;

        public void Show(Game game) => Detail = game;

        public void Close() => Detail = null;

    }
}
