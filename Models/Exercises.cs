using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System;

namespace blindy.Models;

public class Exercise {
    public Exercise(string fileName, string layout, string collection) {
        FileName = fileName;
        Layout = layout;
        Collection = collection;
    }

    public string FileName { get; }
    public string Layout { get; }
    public string Collection { get; }
}

class Exercises {
    static readonly string exDir = Path.Combine("Data", "Exercises");
    readonly string exConfName = "exercises.json";
    readonly string layoutDefault = "ru";

    List<Exercise> collection;
    
    public Exercises() {
        string exConf = Path.Combine(exDir, exConfName);
        string jsonString = "";
        if (!Path.Exists(exConf))
            File.Create(exConf).Close();
        else
            jsonString = File.ReadAllText(exConf);

        try {
            collection = JsonSerializer.Deserialize<List<Exercise>>(jsonString) ?? new List<Exercise>();
        }
        catch {
            Console.WriteLine("No correct config for exercises. New one was created!");
            collection = new List<Exercise>();
        }

        foreach (string file in Directory.GetFiles(exDir)) {
            string fileName = Path.GetFileName(file);
            if (Path.GetExtension(fileName) == ".txt") {
                if (FindExercise(fileName) == null)
                    collection.Add(new Exercise(fileName, layoutDefault, "New"));
            }
        }

        UpdateConfig();
    }

    public Exercise? FindExercise(string fileName) {
        if (collection == null)
            return null;

        foreach (var ex in collection)
            if (ex.FileName == fileName)
                return ex;
        return null;
    }

    public static string GetFullPath(Exercise ex) {
        return Path.Combine(exDir, ex.FileName);
    }
    
    void UpdateConfig() {
        string exConf = Path.Combine(exDir, exConfName);
        var opts = new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        string jsonString = JsonSerializer.Serialize(collection, opts);
        File.WriteAllText(exConf, jsonString);
    }

    public List<Exercise> Collection { get => collection; }
}
