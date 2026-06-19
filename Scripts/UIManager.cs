// =======================
// UIManager.cs
// =======================
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Image[] slotIcons;
    public Text[] slotTexts;
    public Text scoreText;
    public GameObject buyPanel;
    public Text roundText;
    public GameObject reloadIndicator;
    public GameObject hitEffectPrefab;

    public void UpdateWeaponUI(WeaponInstance[] slots)
    {
        if (slotIcons == null || slotTexts == null) return;
        for (int i = 0; i < slots.Length && i < slotIcons.Length && i < slotTexts.Length; i++)
        {
            if (slots[i] != null && slots[i].data != null)
            {
                slotIcons[i].sprite = slots[i].data.icon;
                slotTexts[i].text = slots[i].data.weaponName;
            }
            else
            {
                slotIcons[i].sprite = null;
                slotTexts[i].text = "빈 슬롯";
            }
        }
    }

    public void UpdateAmmoUI(WeaponInstance w)
    {
        if (ammoText == null) return;
        if (w == null) ammoText.text = "";
        else ammoText.text = $"{w.currentAmmo} / {w.reserveAmmo}";
    }

    public void ShowReloading(bool show, float time) { if (reloadIndicator != null) reloadIndicator.SetActive(show); }
    public void ShowBuyPhase(bool show, float time) { if (buyPanel != null) buyPanel.SetActive(show); }
    public void ShowRoundStart(int round) { if (roundText != null) roundText.text = $"Round {round}"; }
    public void UpdateScore(int a, int b) { if (scoreText != null) scoreText.text = $"{a} : {b}"; }
    public void ShowMatchEnd(string winner) { if (roundText != null) roundText.text = $"{winner} 승리!"; }

    public void SpawnHitEffect(Vector3 pos, Vector3 normal)
    {
        if (hitEffectPrefab != null) Instantiate(hitEffectPrefab, pos, Quaternion.LookRotation(normal));
    }

    public void PlayEmptyClick()
    {
        // 빈탄 사운드 재생 구현
    }
}