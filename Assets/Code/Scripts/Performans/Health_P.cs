using System.Collections;
using UnityEngine;

public class Health_P : MonoBehaviour
{
    [SerializeField] private int maksimumCan = 5;
    [SerializeField] private int mevcutCan;
    [SerializeField] private int paraDegeri = 50;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator dusmanAnimator;

    private bool yokEdildi = false;
    private EnemyMovement_P dusmanHareketi;
    private Color orijinalRenk;

    private void Start()
    {
        mevcutCan = maksimumCan;
        dusmanHareketi = GetComponent<EnemyMovement_P>();

        if (spriteRenderer != null)
        {
            orijinalRenk = spriteRenderer.color;
        }
    }

    public void HasarAl(int miktar)
    {
        mevcutCan -= miktar;

        if (mevcutCan <= 0 && !yokEdildi)
        {
            Oldur();
        }
        else
        {
            StartCoroutine(KisaKirmiziEfekt());
        }
    }

    private void Oldur()
    {
        yokEdildi = true;

        dusmanHareketi?.HareketiDurdur();
        dusmanAnimator?.SetBool("Alive", false);

        LevelManager_P.anaYonetic.ParaArttir(paraDegeri);

        EnemySpawner_P.dusmanYokEdildi.Invoke();
        Destroy(gameObject, 0.5f);
    }

    private IEnumerator KisaKirmiziEfekt()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = orijinalRenk;
        }
    }
}

