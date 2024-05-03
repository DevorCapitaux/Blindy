using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text.Json;

namespace blindy.Models;

public class KBButtonsRow : BindingList<KeyboardButton>, INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public KBButtonsRow() {
        this.ListChanged += (object? sender, ListChangedEventArgs e) => NotifyPropertyChanged();
    }
}

public class KBButtonsCollection : BindingList<KBButtonsRow>, INotifyCollectionChanged {
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e) {
        CollectionChanged?.Invoke(this, e);
    }

    public KBButtonsCollection() {
        this.ListChanged += (object? sender, ListChangedEventArgs e) =>
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Trigger() =>
        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
}

public class Layout : Notifier {
    readonly string layoutsDir = Path.Combine("Data", "Layouts");
    KBButtonsCollection keyCollection;

    public Layout(string layoutName) {
        string layout_path = Path.Combine(layoutsDir, "kb_layout_" + layoutName + ".json");
        if (!Path.Exists(layout_path))
            throw new System.Exception($"File \"{layout_path}\" not exist!");

        string jsonString = File.ReadAllText(layout_path);
        var collection = JsonSerializer.Deserialize<KBButtonsCollection>(jsonString);
        if (collection == null)
            throw new System.Exception($"Couldn't convert {layout_path} into KBButtonsCollection");

        keyCollection = collection;

        ShiftL = collection[3][0];
        ShiftR = collection[3][^1];
    }

    public void LoadLayout(string layoutName) {
        string layout_path = Path.Combine(layoutsDir, "kb_layout_" + layoutName + ".json");
        if (!Path.Exists(layout_path))
            throw new System.Exception($"File \"{layout_path}\" not exist!");

        string jsonString = File.ReadAllText(layout_path);
        var collection = JsonSerializer.Deserialize<KBButtonsCollection>(jsonString);
        if (collection == null)
            throw new System.Exception($"Couldn't convert {layout_path} into KBButtonsCollection");

        KeyCollection.Clear();
        foreach (var row in collection) {
            KeyCollection.Add(row);
        }
        ShiftL = collection[3][0];
        ShiftR = collection[3][^1];
    }

    public KBButtonsCollection KeyCollection {
        get => keyCollection;
        set => RaiseAndSetIfChanged(ref keyCollection, value);
    }
    public KeyboardButton ShiftL { get; set; }
    public KeyboardButton ShiftR { get; set; }
}
