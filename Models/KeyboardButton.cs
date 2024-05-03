using Avalonia;
using Avalonia.Controls;

namespace blindy.Models;

public enum Finger : ushort {
    Any = 0, Thumbs,
    IndexR,  MiddleR,
    RingR,   PinkyR,
    IndexL,  MiddleL,
    RingL,   PinkyL
}

public class KeyboardButton : Notifier {

    Finger finger;
    private bool marked;
    private bool[] styles = new bool[10];
    private int[] constraints = new int[4] { 10, 0, 10, 0 };

    public string Key { get; set; } = "";
    public string KeyId { get; set; } = "";
    public string AddKey { get; set; } = "";
    public Thickness Margin { get; private set; } = new Thickness(10, 0);
    public int[] Constraints {
        get => constraints;
        set {
            value.CopyTo(constraints, 0);
            Margin = new Thickness(constraints[0], constraints[1], constraints[2], constraints[3]);
        }
    }
    public bool Marked {
        get => marked;
        set => this.RaiseAndSetIfChanged(ref marked, value);
    }
    public Finger Finger {
        get => finger;
        set {
            finger = value;
            styles[((int)finger)] = true;
        }
    }
    public bool[] Styles { get => styles; }
}
