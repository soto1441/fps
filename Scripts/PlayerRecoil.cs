// =======================
// PlayerRecoil.cs
// =======================
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    public static PlayerRecoil Instance;
    float vertOffset = 0f;
    float horizOffset = 0f;
    public float recoverSpeed = 8f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
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