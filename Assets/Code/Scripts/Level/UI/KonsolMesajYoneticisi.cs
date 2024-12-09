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
        // Belirli bir hata mesajýný engelle
        if (logString.Contains("UnityEditor.IMGUI.Controls.TreeViewController"))
        {
            return; // Bu mesaj konsola yazýlmayacak
        }

        // Diðer mesajlar için varsayýlan iþlem
        Debug.Log(logString);
    }
}
