using UnityEngine;

public class SyncRotToPos : MonoBehaviour
{
    [SerializeField] private Transform trackTransform;
    [SerializeField] private Vector3 rotationAxes = Vector3.right;

    private float movementRotationRatio = 1.745f; // Don't ask... Magic...
    private Vector3 initialPosition;
    private float lastDistance = 0f;

    private void Start()
    {
        initialPosition = trackTransform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(initialPosition, trackTransform.position);

        transform.Rotate(rotationAxes, Mathf.Rad2Deg * (distance - lastDistance) * movementRotationRatio);

        lastDistance = distance;
    }
}
