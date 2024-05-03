using blindy.Models;

namespace blindy.ViewModels;

public class KeyboardViewModel : ViewModelBase {
    Keyboard keyboard = new Keyboard();

    public void UnMarkKeys() => keyboard.UnMarkKeys();
    public void MarkKeys(string keySym) => keyboard.MarkKey(keySym);

    public void SetLayout(string layout) => keyboard.SetLayout(layout);

    public Keyboard Keyboard {
        get => keyboard;
    }
}

