// =======================
// PlayerController.cs
// =======================
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    Rigidbody rb;
    float xRot = 0f;
    bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (playerCamera == null) return;

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRot -= my;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        float recoilVert = 0f;
        float recoilHoriz = 0f;
        if (PlayerRecoil.Instance != null)
        {
            recoilVert = PlayerRecoil.Instance.GetVerticalOffset();
            recoilHoriz = PlayerRecoil.Instance.GetHorizontalOffset();
        }

        playerCamera.transform.localRotation = Quaternion.Euler(xRot + recoilVert, recoilHoriz, 0);
        transform.Rotate(Vector3.up * mx);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * h + transform.forward * v;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 vel = dir.normalized * speed;
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.contacts.Length > 0 && c.contacts[0].normal.y > 0.5f) isGrounded = true;
    }
}