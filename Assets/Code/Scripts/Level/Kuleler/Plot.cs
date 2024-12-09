using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hoverRengi;
    [SerializeField] private GameObject secimUIPrefab;
    [SerializeField] private Canvas canvas;

    private GameObject kule;
    private static GameObject aktifSecimUI;
    private Color baslangicRengi;
    private int kuleSeviyesi;

    private void Start()
    {
        baslangicRengi = spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverRengi;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = baslangicRengi;
    }

    private void OnMouseDown()
    {
        if (kule != null)
        {
            if (aktifSecimUI != null)
            {
                Destroy(aktifSecimUI);
            }

            Vector3 uiPozisyonu = Camera.main.WorldToScreenPoint(transform.position);
            uiPozisyonu.y += 120;

            aktifSecimUI = Instantiate(secimUIPrefab, canvas.transform);
            aktifSecimUI.transform.position = uiPozisyonu;

            SecimUI secimUIComponent = aktifSecimUI.GetComponent<SecimUI>();
            if (secimUIComponent != null)
            {
                secimUIComponent.IlgiliKuleyiAyarla(kule, this, kuleSeviyesi);
            }

            Debug.LogWarning("Bu plotta zaten bir kule var! Yeni kule ekleyemezsiniz.");
            return;
        }

        Kule secilenKule = InsaatYoneticisi.main.SeciliKuleyiAl();

        if (secilenKule == null)
        {
            Debug.LogWarning("Seçili bir kule yok!");
            return;
        }

        if (secilenKule.bedel > LevelYoneticisi.main.para)
        {
            Debug.LogWarning("Bu kuleyi inþa etmek için yeterli paranýz yok.");
            return;
        }

        LevelYoneticisi.main.ParaHarca(secilenKule.bedel);
        kule = Instantiate(secilenKule.prefab, transform.position, Quaternion.identity);
        kuleSeviyesi = 0;
    }

    public void KuleyiTemizle()
    {
        if (kule != null)
        {
            Destroy(kule);
            kule = null;
            kuleSeviyesi = 0;
        }
    }

    public void KuleyiGuncelle(GameObject yeniKule, int yeniSeviye)
    {
        kule = yeniKule;
        kuleSeviyesi = yeniSeviye;
    }

    public int GetKuleSeviyesi()
    {
        return kuleSeviyesi;
    }
}