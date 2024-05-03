using Avalonia.Controls;
using Avalonia.Input;

namespace blindy.Views;

public partial class TyperView : UserControl {
    public TyperView() {
        InitializeComponent();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e) {
        base.OnPointerPressed(e);
        this.Focus();
    }
}