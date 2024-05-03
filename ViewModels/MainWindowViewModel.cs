using Avalonia.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using blindy.Models;
using System;

namespace blindy.ViewModels;

public class MainWindowViewModel : ViewModelBase {
    private TyperViewModel content;
    private Exercises exercises = new Exercises();


    public MainWindowViewModel() {
        MenuItems = new ObservableCollection<MenuItem>(GetExercises());

        foreach (var menuItem in MenuItems) {
            menuItem.PropertyChanged += MenuButtonPressed;
        }

        content = new TyperViewModel();
    }

    private IEnumerable<MenuItem> GetExercises() {
        List<MenuItem> items = new List<MenuItem>();
        foreach (var ex in exercises.Collection) {
            items.Add(new MenuItem { Content = ex.FileName[..^4] });
        }

        return items;
    }

    // Event Handlers
    void MenuButtonPressed(object? sender, EventArgs e) {
        var item = sender as MenuItem;
        if (item != null && item.IsChecked) {
            string fileName = item.Content + ".txt";
            var ex = exercises.FindExercise(fileName);
            if (ex == null)
                throw new Exception($"Error: exercise '{fileName}' was not found");
            content.InitScene(ex);
        }
    }

    public void OnTextInput(TextInputEventArgs e) {
        Content.OnTextInput(e);
    }

    public void OnKeyUp(KeyEventArgs e) {
        if (e.Key.ToString() == "Escape")
            Content.EscapePressed();
    }

    // Properties
    public ObservableCollection<MenuItem> MenuItems { get; }

    public TyperViewModel Content {
        get => content;
        set => this.RaiseAndSetIfChanged(ref content, value);
    }
}
