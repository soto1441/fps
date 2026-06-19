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
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        // 자동으로 카메라 찾기
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        // PlayerRecoil 생성 (없으면 자동 생성)
        if (PlayerRecoil.Instance == null)
        {
            GameObject recoilObj = new GameObject("PlayerRecoil");
            recoilObj.AddComponent<PlayerRecoil>();
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (playerCamera == null) return;

        // 마우스 입력
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRot -= my;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        // 반동 적용
        float recoilVert = 0f;
        float recoilHoriz = 0f;
        if (PlayerRecoil.Instance != null)
        {
            recoilVert = PlayerRecoil.Instance.GetVerticalOffset();
            recoilHoriz = PlayerRecoil.Instance.GetHorizontalOffset();
        }

        playerCamera.transform.localRotation = Quaternion.Euler(xRot + recoilVert, recoilHoriz, 0);
        transform.Rotate(Vector3.up * mx);

        // 이동
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * h + transform.forward * v;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 vel = dir.normalized * speed;
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }

        // 커서 잠금 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.contacts.Length > 0 && c.contacts[0].normal.y > 0.5f) isGrounded = true;
    }
}