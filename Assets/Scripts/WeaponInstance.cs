// =======================
// WeaponInstance.cs
// =======================
[System.Serializable]
public class WeaponInstance
{
    public WeaponData data;
    public int currentAmmo;
    public int reserveAmmo;
    public float lastFireTime;
    public int shotCount;

    public void Init()
    {
        if (data == null) return;
        currentAmmo = data.maxAmmo;
        reserveAmmo = data.reserveAmmo;
        lastFireTime = -999f;
        shotCount = 0;
    }
}