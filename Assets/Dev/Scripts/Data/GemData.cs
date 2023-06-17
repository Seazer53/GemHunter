using System;
using UnityEngine;

[Serializable]
public class GemData
{
    public GemRarity type;
    public int price;
    public Sprite icon;
    public GameObject prefab;
    
    public enum GemRarity 
    {
        Common,
        Rare,
        Legendary
    }
}
