using Avalonia.Controls.Documents;

namespace blindy.ViewModels;

public static class TyperViewModelExtensions {

    private static void InsertText(InlineCollection inlines, string className, string text, int? index) {
        var run = new Run(text);
        run.Classes.Add(className);
        if (index == null)
            inlines.Add(run);
        else
            inlines.Insert(index ?? 0, run);
    }

    public static void TextDefault(this InlineCollection inlines, string text, int? index = null) {
        InsertText(inlines, "TextDefault", text, index);
    }

    public static void TextToType(this InlineCollection inlines, string text, int? index = null) {
        InsertText(inlines, "TextToType", text, index);
    }

    public static void TypedTextNormal(this InlineCollection inlines, string text, int? index = null) {
        InsertText(inlines, "TypedTextNormal", text, index);
    }

    public static void TypedTextError(this InlineCollection inlines, string text, int? index = null) {
        InsertText(inlines, "TypedTextError", text, index);
    }

    public static void SetCursorDefault(this Run run) {
        run.Classes.Clear();
        run.Classes.Add("CursorDefault");
    }

    public static void SetCursorError(this Run run) {
        run.Classes.Clear();
        run.Classes.Add("CursorError");
    }
}
