using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;


    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }
    private void OnMouseEnter() 
    {
        sr.color = hoverColor;
    }
    private void OnMouseExit()
    {
        sr.color = startColor;
    }
    private void OnMouseDown()
    {
        if (tower != null) 
        {
            return;
        }


        // GameObject towerToBuild = BuildManager.main.GetSelectedTower();
        // tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Bu ��e i�in paran yetmiyor.");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity); 
    }
}
