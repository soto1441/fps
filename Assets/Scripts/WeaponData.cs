// =======================
// WeaponData.cs
// =======================
using UnityEngine;

public enum WeaponType { Assault, Sniper, Shotgun, Pistol, Melee }

[CreateAssetMenu(fileName = "WeaponData", menuName = "FPS/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public int maxAmmo = 30;
    public int reserveAmmo = 90;
    public float fireRate = 10f;
    public float reloadTime = 2.2f;
    public float bulletSpeed = 200f;
    public int damageHead = 90;
    public int damageBody = 30;
    public int damageLeg = 20;
    public float recoilVertical = 1.5f;
    public float recoilHorizontal = 0.6f;
    public float baseSpread = 0.5f;
    public bool isAutomatic = true;
    public bool usesHitscan = true;
    public GameObject bulletPrefab;
    public Sprite icon;
    public int price = 3000;
    public SprayPattern sprayPattern;
}