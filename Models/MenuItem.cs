namespace blindy.Models;

public class MenuItem : Notifier {
    private bool isChecked;

    public string Content { get; set; } = "";
    public bool IsChecked {
        get => isChecked;
        set => this.RaiseAndSetIfChanged(ref isChecked, value);
    }
}