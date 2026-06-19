// =======================
// UIManager.cs
// =======================
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public Text ammoText;
    public Image[] slotIcons = new Image[3];
    public Text[] slotTexts = new Text[3];
    public Text scoreText;
    public GameObject buyPanel;
    public Text roundText;
    public GameObject reloadIndicator;
    public GameObject hitEffectPrefab;
    public AudioSource emptyClickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 자동으로 UI 요소 찾기
        if (ammoText == null) ammoText = FindObjectOfType<Text>();
        if (buyPanel == null) buyPanel = GameObject.Find("BuyPanel");
        if (reloadIndicator == null) reloadIndicator = GameObject.Find("ReloadIndicator");
        if (roundText == null) roundText = FindObjectOfType<Text>();
        if (scoreText == null) scoreText = FindObjectOfType<Text>();
    }

    public void UpdateWeaponUI(WeaponInstance[] slots)
    {
        if (slots == null || slotIcons == null || slotTexts == null) return;
        
        for (int i = 0; i < slots.Length && i < slotIcons.Length && i < slotTexts.Length; i++)
        {
            if (slots[i] != null && slots[i].data != null)
            {
                if (slotIcons[i] != null) slotIcons[i].sprite = slots[i].data.icon;
                if (slotTexts[i] != null) slotTexts[i].text = slots[i].data.weaponName;
            }
            else
            {
                if (slotIcons[i] != null) slotIcons[i].sprite = null;
                if (slotTexts[i] != null) slotTexts[i].text = "빈 슬롯";
            }
        }
    }

    public void UpdateAmmoUI(WeaponInstance w)
    {
        if (ammoText == null) return;
        if (w == null) ammoText.text = "";
        else ammoText.text = $"{w.currentAmmo} / {w.reserveAmmo}";
    }

    public void ShowReloading(bool show, float time)
    {
        if (reloadIndicator != null) reloadIndicator.SetActive(show);
    }

    public void ShowBuyPhase(bool show, float time)
    {
        if (buyPanel != null) buyPanel.SetActive(show);
    }

    public void ShowRoundStart(int round)
    {
        if (roundText != null) roundText.text = $"Round {round}";
    }

    public void UpdateScore(int a, int b)
    {
        if (scoreText != null) scoreText.text = $"{a} : {b}";
    }

    public void ShowMatchEnd(string winner)
    {
        if (roundText != null) roundText.text = $"{winner} 승리!";
    }

    public void SpawnHitEffect(Vector3 pos, Vector3 normal)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        }
    }

    public void PlayEmptyClick()
    {
        if (emptyClickSound != null)
        {
            emptyClickSound.Play();
        }
    }
}