// =======================
// Damageable.cs
// =======================
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int armor = 0; // 0 none, 50 light, 100 heavy
    [Range(0f,1f)]
    public float armorDamageReduction = 0.6f;

    void Start() { currentHP = maxHP; }

    public void ApplyDamage(int baseDamage, WeaponData weapon)
    {
        if (baseDamage <= 0) return;

        int final = baseDamage;

        if (armor > 0)
        {
            int absorbed = Mathf.RoundToInt(baseDamage * armorDamageReduction * (armor / 100f));
            absorbed = Mathf.Min(absorbed, armor);
            final = Mathf.Max(0, baseDamage - absorbed);
            armor -= absorbed;
            if (armor < 0) armor = 0;
        }

        currentHP -= final;
        if (currentHP <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}