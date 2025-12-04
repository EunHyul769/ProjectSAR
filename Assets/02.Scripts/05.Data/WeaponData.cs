using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public GameObject WeaponPrefab;
}
