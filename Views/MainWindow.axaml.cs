using Avalonia.Controls;
using Avalonia.Input;

namespace blindy.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    protected override void OnTextInput(TextInputEventArgs e) {
        base.OnTextInput(e);

        var vm = this.DataContext as blindy.ViewModels.MainWindowViewModel;
        if (vm != null)
            vm.OnTextInput(e);
    }

    protected override void OnKeyUp(KeyEventArgs e) {
        base.OnKeyUp(e);

        var vm = this.DataContext as blindy.ViewModels.MainWindowViewModel;
        if (vm != null)
            vm.OnKeyUp(e);
    }
}