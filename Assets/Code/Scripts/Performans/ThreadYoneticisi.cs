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

        // Ýþleri tutacak dinamik bir kuyruk oluþturuyoruz
        var kuyruk = new ConcurrentQueue<int>(Enumerable.Range(0, toplamIs));
        List<Task> gorevler = new List<Task>();

        for (int i = 0; i < maksimumEszamanlilik; i++)
        {
            // Her iþ parçacýðý kuyruðun verilerini iþler
            gorevler.Add(Task.Run(() =>
            {
                while (kuyruk.TryDequeue(out int indeks))
                {
                    islem(indeks); // Ýþ mantýðýný burada çalýþtýrýyoruz
                }
            }));
        }

        // Tüm iþ parçacýklarý tamamlanana kadar bekle
        await Task.WhenAll(gorevler);
    }

    public void AnaIsParcacigindaCalistir(Action islem)
    {
        // Ana iþ parçacýðýna iþ göndermek için kullanýlýr
        if (islem == null) return;
        AnaIslemDagiticisi.Ornek.KuyrugaEkle(islem);
    }
}
