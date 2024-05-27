using System.Collections;
using UnityEngine;

public class RotationPuzzle : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 1;
    public float angleRotationThreshold = 20;

    private Quaternion initialRotation;
    private Vector3 rotation;
    private bool isSolved;

    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = isSolved ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponentInChildren<MeshRenderer>().bounds.size);
        }
    }

    private void Start()
    {
        initialRotation = transform.rotation;
        transform.rotation = GetRandomRotation();
    }

    private void Update()
    {
        if (isSolved)
        {
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, initialRotation, Time.deltaTime * 10);
            StartCoroutine(AutoDestroyCoroutine());
            return;
        }

        HandleRotation();
        isSolved = IsSolved();
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotation);
    }

    private void HandleRotation()
    {
        var rotationX = Input.GetAxis("Vertical") * -rotationSpeed;
        var rotationZ = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation = new Vector3(rotationX, 0, rotationZ);
    }

    private bool IsSolved()
    {
        var rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        var initialRotation = Quaternion.Euler(this.initialRotation.eulerAngles.x, 0, this.initialRotation.eulerAngles.z);
        return Quaternion.Angle(rotation, initialRotation) < angleRotationThreshold;
    }

    private Quaternion GetRandomRotation()
    {
        var angleX = Random.Range(initialRotation.eulerAngles.x + angleRotationThreshold, 360);
        var angleY = Random.Range(initialRotation.eulerAngles.y + angleRotationThreshold, 360);
        var angleZ = Random.Range(initialRotation.eulerAngles.z + angleRotationThreshold, 360);
        return Quaternion.Euler(angleX, angleY, angleZ);
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        transform.rotation = initialRotation;
        Destroy(this);
    }
}
