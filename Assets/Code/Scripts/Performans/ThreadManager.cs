using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    private static ThreadManager ornek;
    private int maksimumEszamanlilik;

    public static ThreadManager Ornek
    {
        get
        {
            if (ornek == null)
            {
                var yonetici = new GameObject("ThreadManager");
                ornek = yonetici.AddComponent<ThreadManager>();
                DontDestroyOnLoad(yonetici);
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
        var queue = new ConcurrentQueue<int>(Enumerable.Range(0, toplamIs));
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < maksimumEszamanlilik; i++)
        {
            // Her iþ parçacýðý kuyruðun verilerini iþler
            tasks.Add(Task.Run(() =>
            {
                while (queue.TryDequeue(out int index))
                {
                    islem(index); // Ýþ mantýðýný burada çalýþtýrýyoruz
                }
            }));
        }

        // Tüm iþ parçacýklarý tamamlanana kadar bekle
        await Task.WhenAll(tasks);
    }

    public void AnaIsParcigindaCalistir(Action islem)
    {
        // Ana iþ parçacýðýna iþ göndermek için kullanýlýr
        if (islem == null) return;
        UnityMainThreadDispatcher.Instance.Enqueue(islem);
    }
}
