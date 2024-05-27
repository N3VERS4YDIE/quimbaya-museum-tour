using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform camPivot;

    [Header("Settings")]
    public float movementSpeed = 1;
    public float mouseSpeed = 1;

    private Rigidbody rb;
    private Transform cam;
    private float movementX;
    private float movementY;
    private float mouseX;
    private float mouseY;
    private float totalRotationX;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        HandleCam();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        transform.position += movementSpeed * Time.fixedDeltaTime * (movementX * transform.right + movementY * transform.forward);
    }

    private void HandleRotation()
    {
        var rotationY = mouseX * mouseSpeed;
        transform.Rotate(Vector3.up, rotationY);

        var rotationX = mouseY * mouseSpeed;
        totalRotationX += rotationX;
        totalRotationX = Mathf.Clamp(totalRotationX, -90, 90);
        camPivot.localRotation = Quaternion.Euler(-totalRotationX, 0, 0);
    }

    private void HandleCam()
    {
        cam.position = camPivot.position;
        cam.rotation = camPivot.rotation;
    }
}