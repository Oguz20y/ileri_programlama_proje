using UnityEngine;
using TMPro; // TextMeshPro i�in gerekli k�t�phane
using UnityEngine.EventSystems;
public class PhoenixYetenegi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private GameObject phoenixPrefab; // Phoenix prefab'�
    [SerializeField] private LayerMask haritaMaskesi; // Harita katman�
    [SerializeField] private TextMeshProUGUI saya�Metni; // Saya� metni

    [Header("�zellikler")]
    [SerializeField] private float beklemeSuresi = 20f; // Bekleme s�resi

    private bool yetenekAktif = false; // Yetenek aktif mi?
    private bool kullanilabilir = false; // Yetenek kullan�labilir mi?
    private float kalanSure = 0f; // Bekleme s�resi

    private void Start()
    {
        kalanSure = beklemeSuresi;
        kullanilabilir = false;
    }

    private void Update()
    {
        if (!kullanilabilir)
        {
            kalanSure -= Time.deltaTime;
            saya�Metni.text = Mathf.Ceil(kalanSure).ToString();

            if (kalanSure <= 0)
            {
                YetenekHazirla();
            }
        }

        if (Input.GetMouseButtonDown(0) && yetenekAktif && kullanilabilir)
        {
            if (UIUzerindeMi())
                return;

            Vector3 farePozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 hedefPozisyonu = new Vector2(farePozisyonu.x, farePozisyonu.y);

            RaycastHit2D vurulan = Physics2D.Raycast(hedefPozisyonu, Vector2.zero, 0f, haritaMaskesi);
            if (vurulan.collider == null)
            {
                PhoenixOlustur(hedefPozisyonu);
            }
        }
    }

    private void PhoenixOlustur(Vector3 hedef)
    {
        kullanilabilir = false;
        yetenekAktif = false;
        kalanSure = beklemeSuresi;

        GameObject yeniPhoenix = Instantiate(phoenixPrefab, new Vector3(hedef.x, hedef.y, 0f), Quaternion.identity);
    }

    private void YetenekHazirla()
    {
        kullanilabilir = true;
        saya�Metni.text = "";
    }

    public void PhoenixButtonunaTikladi()
    {
        if (!kullanilabilir)
            return;

        yetenekAktif = true;
    }

    private bool UIUzerindeMi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
