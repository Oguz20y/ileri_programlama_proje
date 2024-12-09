using System;
using System.Collections.Generic;
using UnityEngine;

public class AnaIslemDagiticisi : MonoBehaviour
{
    private static AnaIslemDagiticisi ornek;
    private static readonly Queue<Action> islemKuyrugu = new Queue<Action>();

    public static AnaIslemDagiticisi Ornek
    {
        get
        {
            if (ornek == null)
            {
                var nesne = new GameObject("AnaIslemDagiticisi");
                ornek = nesne.AddComponent<AnaIslemDagiticisi>();
                DontDestroyOnLoad(nesne);
            }
            return ornek;
        }
    }

    private void Update()
    {
        lock (islemKuyrugu)
        {
            while (islemKuyrugu.Count > 0)
            {
                islemKuyrugu.Dequeue()?.Invoke();
            }
        }
    }

    public void KuyrugaEkle(Action islem)
    {
        if (islem == null) return;

        lock (islemKuyrugu)
        {
            islemKuyrugu.Enqueue(islem);
        }
    }
}