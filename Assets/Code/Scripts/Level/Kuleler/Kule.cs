using System;
using UnityEngine;

[Serializable]
public class Kule
{
    public string ad;
    public int bedel;
    public GameObject prefab;

    public Kule(string _ad, int _bedel, GameObject _prefab)
    {
        this.ad = _ad;
        this.bedel = _bedel;
        this.prefab = _prefab;
    }
}
