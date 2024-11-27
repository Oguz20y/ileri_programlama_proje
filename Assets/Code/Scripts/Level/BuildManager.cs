using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    // [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0;

    private void Awake() 
    {
        main = this;
    }

    public Tower GetSelectedTower() 
    {
        return towers[selectedTower];
    }
    /*
    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTower];
    }
    */

    public void SetSelectedTower(int _selectedTower) 
    {
        this.selectedTower = _selectedTower;
    }
}
