using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool grounded = false;
    public float speed = 2.5f;

    public float jump_height = 5f;
    private int _jumps;

    public Vector2 mouse_sensitivity;

    private Rigidbody _rb;
    private CapsuleCollider _cc;
    private Camera _cam;
    private Vector3 _euler_rotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CapsuleCollider>();
        mouse_sensitivity = new Vector2(1, 1);
        _cam = transform.GetChild(0).GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Temp
        Vector3 vel;
        var tSp = Input.GetKey(KeyCode.LeftShift) ? speed * 1.5f : speed;

        // Movement
        vel = (Input.GetAxisRaw("Vertical") * transform.forward) + (Input.GetAxisRaw("Horizontal") * transform.right);
        vel = vel.normalized * tSp;
        vel.y = _rb.velocity.y;

        // Camera
        transform.Rotate(0, Input.GetAxisRaw("Mouse X") * mouse_sensitivity.y, 0);
        _euler_rotation.x = Mathf.Clamp(_euler_rotation.x - Input.GetAxisRaw("Mouse Y") * mouse_sensitivity.x, -90.0f, 90.0f);
        _euler_rotation.y = transform.rotation.eulerAngles.y;
        _cam.transform.eulerAngles = _euler_rotation;

        // Jump
        if (Physics.Raycast(transform.position, -transform.up, _cc.height / 2.0f + 0.01f))
        {
            grounded = true;
            _jumps = 1;
        }
        else grounded = false;

        if (Input.GetKeyDown(KeyCode.Space) && _jumps > 0)
        {
            if (!grounded) _jumps--;
            vel.y = jump_height;
        }

        // Final Update
        _rb.velocity = vel;
    }
}
