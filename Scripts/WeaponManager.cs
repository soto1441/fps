// =======================
// WeaponManager.cs
// =======================
using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public WeaponInstance[] slots = new WeaponInstance[3]; // 0: 주,1:보조,2:근접
    public Transform firePoint;
    public Camera playerCamera;
    public UIManager ui;
    public LayerMask hitMask;

    bool isReloading = false;

    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].data != null) slots[i].Init();
            else slots[i] = null;
        }

        if (ui != null) ui.UpdateWeaponUI(slots);
    }

    void Update()
    {
        if (playerCamera == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipSlot(2);

        var current = GetCurrentWeapon();
        if (current == null || current.data == null) return;

        bool isADS = Input.GetMouseButton(1);
        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f;
        float spread = current.data.baseSpread * (isADS ? 0.45f : 1f) * (isMoving ? 1.6f : 1f);

        if (isReloading == false && (current.data.isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1")))
        {
            TryShoot(current, spread, isADS);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload(current));
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (current != null) current.shotCount = 0;
        }

        if (ui != null) ui.UpdateAmmoUI(current);
    }

    WeaponInstance GetCurrentWeapon()
    {
        int idx = PlayerState.currentSlot;
        if (idx < 0 || idx >= slots.Length) return null;
        return slots[idx];
    }

    void EquipSlot(int idx)
    {
        if (idx < 0 || idx >= slots.Length) return;
        PlayerState.currentSlot = idx;
        if (ui != null) ui.UpdateWeaponUI(slots);
    }

    void TryShoot(WeaponInstance w, float spread, bool isADS)
    {
        if (w == null || w.data == null) return;
        if (Time.time - w.lastFireTime < 1f / Mathf.Max(0.0001f, w.data.fireRate)) return;
        if (w.currentAmmo <= 0) { if (ui != null) ui.PlayEmptyClick(); return; }

        w.lastFireTime = Time.time;
        w.currentAmmo--;
        w.shotCount++;
        if (ui != null) ui.UpdateAmmoUI(w);

        Vector3 dir = playerCamera.transform.forward;
        Vector3 finalDir = dir;

        if (w.data.sprayPattern != null && w.data.sprayPattern.offsets != null && w.data.sprayPattern.offsets.Length > 0)
        {
            Vector2 patternOffset = GetPatternOffset(w.data.sprayPattern, w.shotCount);
            float adsScale = isADS ? 0.45f : 1f;
            float moveScale = (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f) ? 1.6f : 1f;
            float finalScale = w.data.sprayPattern.patternScale * adsScale * moveScale;
            finalDir = Quaternion.Euler(patternOffset.y * finalScale, patternOffset.x * finalScale, 0) * dir;
        }
        else
        {
            finalDir = ApplyRandomSpread(dir, spread * 0.5f);
        }

        finalDir = ApplyRandomSpread(finalDir, w.data.baseSpread * 0.05f);

        if (w.data.usesHitscan) FireHitscan(w, finalDir);
        else FireProjectile(w, finalDir);

        ApplyRecoil(w);
    }

    Vector2 GetPatternOffset(SprayPattern pattern, int shotIndex)
    {
        if (pattern == null || pattern.offsets == null || pattern.offsets.Length == 0) return Vector2.zero;
        int idx = Mathf.Max(0, shotIndex - 1);
        if (idx >= pattern.offsets.Length)
        {
            if (pattern.loop) idx = idx % pattern.offsets.Length;
            else idx = pattern.offsets.Length - 1;
        }
        return pattern.offsets[idx];
    }

    Vector3 ApplyRandomSpread(Vector3 dir, float smallSpread)
    {
        float yaw = Random.Range(-smallSpread, smallSpread);
        float pitch = Random.Range(-smallSpread, smallSpread);
        return Quaternion.Euler(pitch, yaw, 0) * dir;
    }

    void FireHitscan(WeaponInstance w, Vector3 dir)
    {
        if (firePoint == null) return;
        if (Physics.Raycast(firePoint.position, dir, out RaycastHit hit, 2000f, hitMask))
        {
            var dmg = hit.collider.GetComponent<Damageable>();
            if (dmg != null)
            {
                int applied = w.data.damageBody;
                if (hit.collider.CompareTag("Head")) applied = w.data.damageHead;
                else if (hit.collider.CompareTag("Leg")) applied = w.data.damageLeg;
                dmg.ApplyDamage(applied, w.data);
            }
            if (ui != null) ui.SpawnHitEffect(hit.point, hit.normal);
        }
    }

    void FireProjectile(WeaponInstance w, Vector3 dir)
    {
        if (firePoint == null || w.data.bulletPrefab == null) return;
        GameObject b = Instantiate(w.data.bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        var rb = b.GetComponent<Rigidbody>();
        if (rb) rb.velocity = dir * w.data.bulletSpeed;
        var bullet = b.GetComponent<Bullet>();
        if (bullet != null) bullet.Init(w.data.damageHead, w.data.damageBody, w.data.damageLeg, w.data);
    }

    IEnumerator Reload(WeaponInstance w)
    {
        if (w == null || w.data == null) yield break;
        if (w.currentAmmo >= w.data.maxAmmo || w.reserveAmmo <= 0) yield break;

        isReloading = true;
        if (ui != null) ui.ShowReloading(true, w.data.reloadTime);
        yield return new WaitForSeconds(w.data.reloadTime);

        int need = w.data.maxAmmo - w.currentAmmo;
        int take = Mathf.Min(need, w.reserveAmmo);
        w.currentAmmo += take;
        w.reserveAmmo -= take;
        w.shotCount = 0;

        if (ui != null) ui.ShowReloading(false, 0);
        if (ui != null) ui.UpdateAmmoUI(w);
        isReloading = false;
    }

    void ApplyRecoil(WeaponInstance w)
    {
        if (w == null || w.data == null) return;
        float v = w.data.recoilVertical * Random.Range(0.95f, 1.05f);
        float h = w.data.recoilHorizontal * Random.Range(-1f, 1f);
        if (PlayerRecoil.Instance != null) PlayerRecoil.Instance.AddRecoil(v, h);
    }
}