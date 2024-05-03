using Avalonia.Input;
using Avalonia.Data;
using Avalonia.Controls.Documents;
using ReactiveUI;
using blindy.Models;
using System.Diagnostics;
using System;

namespace blindy.ViewModels;

public class TyperViewModel : ViewModelBase {
    Backend backend = new Backend();
    InlineCollection inlines = new InlineCollection();
    Run cursor;
    KeyboardViewModel keyboard = new KeyboardViewModel();
    Stopwatch stopwatch = new Stopwatch();

    string text = "";
    string cursorLetter = "";

    int progressBarMaximum = 1;
    int progressBarValue;

    const int staticPos = 24;

    bool errorFlag;
    bool running;

    public TyperViewModel() {
        cursor = new Run {
            [!Run.TextProperty] = new Binding("CursorLetter"),
        };
        Inlines.TypedTextNormal("Выберите упражнение!");
    }

    public bool InitScene(Exercise exercise) {
        Inlines.Clear();
        stopwatch.Reset();
        running = errorFlag = false;

        if (!backend.LoadFile(Exercises.GetFullPath(exercise))) {
            Inlines.TextDefault($"Не удалось найти файл: {exercise.FileName}");
            return false;
        }

        Inlines.TextDefault("Пробел - Начать; Escape - Остановить");

        keyboard.SetLayout(exercise.Layout);

        Text = backend.Text;
        Text = Text[1..];
        ProgressBarMaximum = backend.Text.Length;
        ProgressBarValue = backend.Index;

        CursorLetter = backend.Text[0].ToString();
        cursor.SetCursorDefault();

        return true;
    }

    private int GetSpeed() {
        long totalMlsec = stopwatch.ElapsedMilliseconds;
        double min = totalMlsec / 60000f;
        int wordPerMin = Convert.ToInt32(backend.Index / min);
        return wordPerMin;
    }

    private void ShowResult() {
        long mins = stopwatch.ElapsedMilliseconds / 60000;
        long secs = stopwatch.ElapsedMilliseconds / 1000 - mins * 60;

        keyboard.UnMarkKeys();
        Inlines.Clear();
        Inlines.TextDefault(
            $"Скорость: {GetSpeed()} сим/мин; " +
            $"Ошибок: {backend.ErrorCount} " +
            $"({backend.ErrorCount * 1000 / (backend.Index != 0 ? backend.Index : 1) / 10f}%); " +
            $"Введено: {backend.Index} сим.; " +
            $"Время: {mins}:{secs:00}"
        );
        stopwatch.Reset();
    }

    public void OnTextInput(TextInputEventArgs e) {
        if (backend.Finished)
            return;

        if (!running) {
            if (e.Text != " ")
                return;

            var textToType = new Run { [!Run.TextProperty] = new Binding("Text") };
            Inlines.Clear();
            Inlines.Add(cursor);
            Inlines.Add(textToType);
            keyboard.MarkKeys(CursorLetter);

            running = true;
            stopwatch.Start();
            return;
        }

        char chr = e.Text?[0] ?? '\0';

        if (backend.TypeKey(chr)) {
            if (Inlines.Count >= staticPos)
                Inlines.RemoveAt(0);

            if (errorFlag) {
                errorFlag = false;
                cursor.SetCursorDefault();
                Inlines.TypedTextError(chr.ToString(), Inlines.Count - 2);
            } else
                Inlines.TypedTextNormal(chr.ToString(), Inlines.Count - 2);

            CursorLetter = Text != "" ? Text[0].ToString() : " ";
            keyboard.MarkKeys(CursorLetter);
            Text = Text != "" ? Text[1..] : Text;

            ProgressBarValue = backend.Index;

            if (backend.Finished) {
                stopwatch.Stop();
                running = false;
                ShowResult();
            }

        } else {
            if (errorFlag)
                return;

            errorFlag = true;
            cursor.SetCursorError();
        }
    }

    public void EscapePressed() {
        if (!running)
            return;
        stopwatch.Stop();
        backend.Finished = true;
        running = false;
        ShowResult();
    }

    // Properties
    public InlineCollection Inlines {
        get => inlines;
        set => this.RaiseAndSetIfChanged(ref inlines, value);
    }
    private string Text {
        get => text;
        set => this.RaiseAndSetIfChanged(ref text, value);
    }
    private string CursorLetter {
        get => cursorLetter;
        set => this.RaiseAndSetIfChanged(ref cursorLetter, value);
    }
    public KeyboardViewModel Keyboard {
        get => keyboard;
        set => this.RaiseAndSetIfChanged(ref keyboard, value);
    }
    private int ProgressBarMaximum {
        get => progressBarMaximum;
        set => this.RaiseAndSetIfChanged(ref progressBarMaximum, value);
    }
    private int ProgressBarValue {
        get => progressBarValue;
        set => this.RaiseAndSetIfChanged(ref progressBarValue, value);
    }
}
