// =======================
// Damageable.cs
// =======================
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHP = 100;
    [SerializeField]
    private int currentHPField;
    public int currentHP
    {
        get { return currentHPField; }
        set { currentHPField = value; }
    }

    public int armor = 0; // 0 none, 50 light, 100 heavy
    [Range(0f, 1f)]
    public float armorDamageReduction = 0.6f;

    void Start()
    {
        currentHP = maxHP;
    }

    public void ApplyDamage(int baseDamage, WeaponData weapon = null)
    {
        if (baseDamage <= 0) return;
        if (currentHP <= 0) return;

        int final = baseDamage;

        // 방어구 계산
        if (armor > 0)
        {
            int absorbed = Mathf.RoundToInt(baseDamage * armorDamageReduction * (armor / 100f));
            absorbed = Mathf.Min(absorbed, armor);
            final = Mathf.Max(0, baseDamage - absorbed);
            armor -= absorbed;
            if (armor < 0) armor = 0;
        }

        currentHP -= final;
        Debug.Log($"{gameObject.name}이(가) {final}의 피해를 입었습니다. 남은 체력: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 죽었습니다.");
        Destroy(gameObject);
    }
}