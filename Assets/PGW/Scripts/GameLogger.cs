using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class GameLogger : MonoBehaviour
{
    public static GameLogger Instance { get; private set; }
    private string logFilePath; private bool isHeaderWritten; private const string AUDIO_LISTENER_MESSAGE = "There are no audio listeners in the scene. Please ensure there is always one audio listener in the scene";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        string logDir = Path.Combine(Application.persistentDataPath, "log");
        Directory.CreateDirectory(logDir);
        logFilePath = Path.Combine(logDir, $"GameLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");

        Application.logMessageReceived += HandleUnityLog;
        Log("System|Initialized|Status=OK");
    }

    public void Log(string message)
    {
        if (message == AUDIO_LISTENER_MESSAGE)
            return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log($"[{timestamp}] {message}");
        WriteToCsv(timestamp, message);
    }

    private void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        if (type != LogType.Log || logString.StartsWith("[") || logString == AUDIO_LISTENER_MESSAGE)
            return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        WriteToCsv(timestamp, logString);
    }

    private void WriteToCsv(string timestamp, string message)
    {
        try
        {
            var csvColumns = new List<string> { timestamp };
            var headerColumns = new List<string> { "Timestamp" };

            bool isStructured = ParseMessage(message, out var fields);

            if (isStructured)
            {
                csvColumns.Add(EscapeCsvField(fields.GetValueOrDefault("Category", "Unknown")));
                csvColumns.Add(EscapeCsvField(fields.GetValueOrDefault("Action", "Unknown")));
                headerColumns.Add("Category");
                headerColumns.Add("Action");

                foreach (var kvp in fields)
                {
                    if (kvp.Key != "Category" && kvp.Key != "Action")
                    {
                        csvColumns.Add(EscapeCsvField(kvp.Value));
                        headerColumns.Add(kvp.Key);
                    }
                }
            }
            else
            {
                csvColumns.Add(EscapeCsvField(message));
                headerColumns.Add("Message");
            }

            if (!isHeaderWritten)
            {
                File.WriteAllText(logFilePath, string.Join(",", headerColumns) + "\n");
                isHeaderWritten = true;
            }

            File.AppendAllText(logFilePath, string.Join(",", csvColumns) + "\n");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to CSV: {ex.Message}");
        }
    }

    private bool ParseMessage(string message, out Dictionary<string, string> fields)
    {
        fields = new Dictionary<string, string>();
        string[] parts = message.Split('|');
        if (parts.Length < 2)
            return false;

        fields["Category"] = parts[0].Trim();
        fields["Action"] = parts[1].Trim();

        for (int i = 2; i < parts.Length; i++)
        {
            string[] kvp = parts[i].Split('=');
            if (kvp.Length == 2)
                fields[kvp[0].Trim()] = kvp[1].Trim();
        }

        return true;
    }

    private string EscapeCsvField(string field)
    {
        return string.IsNullOrEmpty(field) ? "\"\"" : $"\"{field.Replace("\"", "\"\"")}\"";
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleUnityLog;
    }

}