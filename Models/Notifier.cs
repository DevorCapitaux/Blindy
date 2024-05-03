using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace blindy.Models;

public class Notifier : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void RaiseAndSetIfChanged<T>(ref T property, T value) {
        if (property?.Equals(value) ?? false)
            return;
        property = value;
        NotifyPropertyChanged();
    }
}