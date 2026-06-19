// =======================
// PlayerRecoil.cs
// =======================
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    public static PlayerRecoil Instance { get; private set; }
    float vertOffset = 0f;
    float horizOffset = 0f;
    public float recoverSpeed = 8f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Update()
    {
        vertOffset = Mathf.Lerp(vertOffset, 0, Time.deltaTime * recoverSpeed);
        horizOffset = Mathf.Lerp(horizOffset, 0, Time.deltaTime * recoverSpeed);
    }

    public void AddRecoil(float v, float h)
    {
        vertOffset += v;
        horizOffset += h;
    }

    public float GetVerticalOffset() => vertOffset;
    public float GetHorizontalOffset() => horizOffset;
}
