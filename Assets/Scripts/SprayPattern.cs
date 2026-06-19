// =======================
// SprayPattern.cs
// =======================
using UnityEngine;

[CreateAssetMenu(fileName = "SprayPattern", menuName = "FPS/SprayPattern")]
public class SprayPattern : ScriptableObject
{
    // offsets: x = yaw (deg), y = pitch (deg)
    public Vector2[] offsets;
    public float patternScale = 1f;
    public bool loop = false;
}