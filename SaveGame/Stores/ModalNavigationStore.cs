using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Stores
{
    public partial class ModalNavigationStore : ObservableObject
    {
        private bool _currentViewModel;
        public bool CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public bool IsOpen => CurrentViewModel != false;


        public event Action CurrentViewModelChanged;

        public void Close()
        {
            CurrentViewModel = false;
        }
    }
}
