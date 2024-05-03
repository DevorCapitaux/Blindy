using System.Collections.Generic;

namespace blindy.Models;

public class Keyboard : Notifier {
    public Keyboard() {
        Layout = new Layout("ru");
    }

    public void SetLayout(string layout) {
        Layout.LoadLayout(layout);
    }

    private List<KeyboardButton> lastKeysMarked = new List<KeyboardButton>();

    public void UnMarkKeys() {
        if (lastKeysMarked == null)
            return;

        foreach(var key in lastKeysMarked) {
            key.Marked = false;
        }
    }

    public void MarkKey(string keySym) {
        UnMarkKeys();

        string keyId = keySym == " " ? "space" : keySym.ToLower();

        foreach (var row in Layout.KeyCollection) {
            foreach (var key in row) {
                if (keyId != key.KeyId && keyId != key.AddKey)
                    continue;

                key.Marked = true;
                lastKeysMarked.Add(key);

                if (keySym == keySym.ToLower() && keyId != key.AddKey)
                    return;

                KeyboardButton shift;
                if (key.Finger <= Finger.PinkyR)
                    shift = Layout.ShiftL;
                else
                    shift = Layout.ShiftR;
                shift.Marked = true;
                lastKeysMarked.Add(shift);
                return;
            }
        }
    }

    public Layout Layout { get; }
}
