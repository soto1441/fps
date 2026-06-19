// =======================
// EnemyHealth.cs
// =======================
using UnityEngine;

public class EnemyHealth : Damageable
{
    public GameObject deathEffectPrefab;
    public int creditsOnDeath = 300;

    protected override void Die()
    {
        // 사망 이펙트 생성
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // 크레딧 추가
        PlayerState.credits += creditsOnDeath;
        Debug.Log($"획득 크레딧: {creditsOnDeath}. 총 크레딧: {PlayerState.credits}");

        base.Die();
    }
}