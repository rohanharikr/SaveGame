using CommunityToolkit.Mvvm.ComponentModel;
using IGDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Stores
{
    public partial class ModalNavigationStore : ObservableObject
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

        public void Close()
        {
            _detail = null;
        }
    }
}
