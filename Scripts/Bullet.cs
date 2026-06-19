// =======================
// Bullet.cs
// =======================
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    int dmgHead, dmgBody, dmgLeg;
    WeaponData sourceWeapon;

    public void Init(int head, int body, int leg, WeaponData data)
    {
        dmgHead = head; dmgBody = body; dmgLeg = leg; sourceWeapon = data;
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision col)
    {
        var hit = col.collider;
        int applied = dmgBody;
        if (hit.CompareTag("Head")) applied = dmgHead;
        else if (hit.CompareTag("Leg")) applied = dmgLeg;

        var dmg = col.gameObject.GetComponent<Damageable>();
        if (dmg != null) dmg.ApplyDamage(applied, sourceWeapon);

        Destroy(gameObject);
    }
}