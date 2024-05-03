using System.IO;

namespace blindy.Models;

public class Backend : Notifier {
    int index, errorCount;
    bool errorFlag;
    string? filePath, text;

    public bool LoadFile(string filePath) {
        if (!File.Exists(filePath))
            return false;
        FilePath = filePath;
        text = File.ReadAllText(FilePath).Trim().ReplaceLineEndings(" ");
        Finished = false;
        errorFlag = false;
        Index = 0;
        ErrorCount = 0;
        return true;
    }

    public bool TypeKey(char? key) {
        if (Finished || key == null)
            return false;

        if (key != Text[Index]) {
            if (errorFlag)
                return false;
            errorFlag = true;
            ++ErrorCount;
            return false;
        }

        errorFlag = false;
        ++Index;
        if (Index == Text.Length)
            Finished = true;
        return true;
    }

    public bool Finished { get; set; } = true;
    public string FilePath {
        get => filePath ?? "";
        private set => filePath = value;
    }
    public string Text {
        get => text ?? "";
        private set => text = value;
    }
    public int ErrorCount {
        get => errorCount;
        private set => errorCount = value;
    }
    public int Index {
        get => index;
        private set => index = value;
    }
}
