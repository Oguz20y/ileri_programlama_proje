using UnityEngine;
using TMPro; // TextMeshPro için gerekli kütüphane
using UnityEngine.EventSystems;
public class PhoenixYetenegi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private GameObject phoenixPrefab; // Phoenix prefab'ý
    [SerializeField] private LayerMask haritaMaskesi; // Harita katmaný
    [SerializeField] private TextMeshProUGUI sayaçMetni; // Sayaç metni

    [Header("Özellikler")]
    [SerializeField] private float beklemeSuresi = 20f; // Bekleme süresi

    private bool yetenekAktif = false; // Yetenek aktif mi?
    private bool kullanilabilir = false; // Yetenek kullanýlabilir mi?
    private float kalanSure = 0f; // Bekleme süresi

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
            sayaçMetni.text = Mathf.Ceil(kalanSure).ToString();

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
        sayaçMetni.text = "";
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
