using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ThreadYoneticisi : MonoBehaviour
{
    private static ThreadYoneticisi ornek;
    private int maksimumEszamanlilik;

    public static ThreadYoneticisi Ornek
    {
        get
        {
            if (ornek == null)
            {
                var yoneticiNesnesi = new GameObject("ThreadYoneticisi");
                ornek = yoneticiNesnesi.AddComponent<ThreadYoneticisi>();
                DontDestroyOnLoad(yoneticiNesnesi);
            }
            return ornek;
        }
    }

    private void Awake()
    {
        maksimumEszamanlilik = Mathf.Max(1, SystemInfo.processorCount - 1);
    }

    public async Task ParalelCalistirAsync(int toplamIs, Action<int> islem)
    {
        if (islem == null || toplamIs <= 0) return;

        // ��leri tutacak dinamik bir kuyruk olu�turuyoruz
        var kuyruk = new ConcurrentQueue<int>(Enumerable.Range(0, toplamIs));
        List<Task> gorevler = new List<Task>();

        for (int i = 0; i < maksimumEszamanlilik; i++)
        {
            // Her i� par�ac��� kuyru�un verilerini i�ler
            gorevler.Add(Task.Run(() =>
            {
                while (kuyruk.TryDequeue(out int indeks))
                {
                    islem(indeks); // �� mant���n� burada �al��t�r�yoruz
                }
            }));
        }

        // T�m i� par�ac�klar� tamamlanana kadar bekle
        await Task.WhenAll(gorevler);
    }

    public void AnaIsParcacigindaCalistir(Action islem)
    {
        // Ana i� par�ac���na i� g�ndermek i�in kullan�l�r
        if (islem == null) return;
        AnaIslemDagiticisi.Ornek.KuyrugaEkle(islem);
    }
}
