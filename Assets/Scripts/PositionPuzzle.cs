using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PositionPuzzle : MonoBehaviour
{
    [Header("References")]
    public Transform positionA;
    public Transform positionB;

    [Header("Settings")]
    public float movementSpeed = 1;
    public float positionThreshold = 1;

    private Vector3 initialPosition;
    private bool isInSolveRange;
    private bool isSolved;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        isSolved = Input.GetButtonDown("Jump") && isInSolveRange;

        if (isSolved)
        {
            Debug.Log("Puzzle solved!");
            transform.position = Vector3.LerpUnclamped(transform.position, initialPosition, Time.deltaTime * 10);
            StartCoroutine(AutoDestroyCoroutine());
            return;
        }
    }

    private void FixedUpdate()
    {
        Translate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In solve range");
        isInSolveRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out of solve range");
        isInSolveRange = false;
    }

    private void Translate()
    {
        transform.position = Vector3.Lerp(positionA.position, positionB.position, Mathf.PingPong(Time.time * movementSpeed, 1));
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        transform.position = initialPosition;
        Destroy(this);
    }
}
