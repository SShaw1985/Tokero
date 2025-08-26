using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tokero.Models
{
    public class CoinSelectionItem : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        private Action<CoinSelectionItem, bool>? _onSelectionChanged;

        public string Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if(_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                    _onSelectionChanged?.Invoke(this, value);
                }
            }
        }

        public CoinSelectionItem (string name, bool isSelected = false, Action<CoinSelectionItem, bool>? onSelectionChanged = null)
        {
            Name = name;
            _isSelected = isSelected;
            _onSelectionChanged = onSelectionChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged ([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}