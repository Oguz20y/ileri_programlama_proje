using UnityEngine;

public class EnemyMovement_P : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float hareketHizi = 2f;

    private Transform[] yolNoktalari; // Koridora ait yol noktalarý
    private int yolIndeksi = 0;

    private void Start()
    {
        int rastgeleKoridor = Random.Range(0, LevelManager_P.anaYonetic.koridorlar.Length);
        yolNoktalari = LevelManager_P.anaYonetic.koridorlar[rastgeleKoridor].yolNoktalari;

        transform.position = yolNoktalari[0].position;
    }

    private void Update()
    {
        HedefeDogruHareketEt();
    }

    private void HedefeDogruHareketEt()
    {
        if (yolIndeksi >= yolNoktalari.Length) return;

        Vector3 hedef = yolNoktalari[yolIndeksi].position;
        transform.position = Vector3.MoveTowards(transform.position, hedef, hareketHizi * Time.deltaTime);

        if (Vector3.Distance(transform.position, hedef) < 0.1f)
        {
            yolIndeksi++;

            if (yolIndeksi >= yolNoktalari.Length)
            {
                LevelManager_P.anaYonetic.mevcutCan -= LevelManager_P.anaYonetic.dusmanHasar;
                Destroy(gameObject);
            }
        }
    }

    public void HareketiDurdur()
    {
        hareketHizi = 0f;
    }
}
