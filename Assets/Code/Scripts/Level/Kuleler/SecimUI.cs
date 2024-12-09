using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeData
{
    public List<GameObject> upgradePrefabs;
    public List<int> upgradeFiyatlari;
}

[System.Serializable]
public class KuleTurData
{
    public GameObject kulePrefab;
    public int kuleFiyati;
    public UpgradeData upgradeData;
}

public class SecimUI : MonoBehaviour
{
    [Header("Kule Türleri ve Upgrade Verileri")]
    [SerializeField] public List<KuleTurData> kuleTurleri;

    [Header("UI Referanslarý")]
    [SerializeField] private TMP_Text upgradeFiyatText;

    private GameObject ilgiliKule;
    private Plot ilgiliPlot;
    private int kuleTuruIndex;
    private int upgradeSeviyesi;

    public void IlgiliKuleyiAyarla(GameObject kule, Plot plot, int seviye)
    {
        ilgiliKule = kule;
        ilgiliPlot = plot;
        upgradeSeviyesi = seviye;

        for (int i = 0; i < kuleTurleri.Count; i++)
        {
            if (kule.name.Contains(kuleTurleri[i].kulePrefab.name))
            {
                kuleTuruIndex = i;
                GuncelleUpgradeFiyatText();
                return;
            }
        }

        Debug.LogWarning("Kule türü bulunamadý!");
    }

    public void KuleyiYukselt()
    {
        if (ilgiliKule == null) return;

        KuleTurData kuleTurData = kuleTurleri[kuleTuruIndex];

        if (upgradeSeviyesi < kuleTurData.upgradeData.upgradePrefabs.Count)
        {
            int fiyat = kuleTurData.upgradeData.upgradeFiyatlari[upgradeSeviyesi];

            if (LevelYoneticisi.main.para >= fiyat)
            {
                LevelYoneticisi.main.ParaHarca(fiyat);
                Destroy(ilgiliKule);

                GameObject yeniKule = Instantiate(
                    kuleTurData.upgradeData.upgradePrefabs[upgradeSeviyesi],
                    ilgiliKule.transform.position,
                    Quaternion.identity
                );

                upgradeSeviyesi++;
                ilgiliPlot.KuleyiGuncelle(yeniKule, upgradeSeviyesi);
                GuncelleUpgradeFiyatText();
            }
            else
            {
                Debug.LogWarning("Yeterli paranýz yok!");
            }
        }
        else
        {
            Debug.Log("Bu kule maksimum seviyeye ulaþtý!");
        }
    }

    public void KuleyiYik()
    {
        if (ilgiliKule != null)
        {
            Destroy(ilgiliKule);
            if (ilgiliPlot != null)
            {
                ilgiliPlot.KuleyiTemizle();
            }
        }

        PaneliKapat();
    }

    public void PaneliKapat()
    {
        gameObject.SetActive(false);
    }

    private void GuncelleUpgradeFiyatText()
    {
        if (kuleTuruIndex < 0 || kuleTuruIndex >= kuleTurleri.Count)
        {
            upgradeFiyatText.text = "Max Seviye";
            return;
        }

        KuleTurData kuleTurData = kuleTurleri[kuleTuruIndex];

        if (upgradeSeviyesi < kuleTurData.upgradeData.upgradeFiyatlari.Count)
        {
            upgradeFiyatText.text = "Fiyat: " + kuleTurData.upgradeData.upgradeFiyatlari[upgradeSeviyesi].ToString();
        }
        else
        {
            upgradeFiyatText.text = "Max Seviye";
        }
    }
}