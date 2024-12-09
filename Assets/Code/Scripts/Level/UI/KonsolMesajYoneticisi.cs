using UnityEngine;

public class KonsolMesajYoneticisi : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += FilterLogs;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= FilterLogs;
    }

    void FilterLogs(string logString, string stackTrace, LogType type)
    {
        // Belirli bir hata mesaj�n� engelle
        if (logString.Contains("UnityEditor.IMGUI.Controls.TreeViewController"))
        {
            return; // Bu mesaj konsola yaz�lmayacak
        }

        // Di�er mesajlar i�in varsay�lan i�lem
        Debug.Log(logString);
    }
}
