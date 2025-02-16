using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; 
    public LayerMask collisionLayers; 
    public float minDistance = 0.1f; 
    public float maxDistance = 0.5f; 
    public float smoothSpeed = 10f; 

    private Vector3 defaultLocalPosition;

    void Start()
    {
        defaultLocalPosition = transform.localPosition; 
    }

    void LateUpdate()
    {
        Vector3 startPos = cameraPosition.position;
        Vector3 direction = (transform.position - startPos).normalized; 
        float distance = defaultLocalPosition.z; 

        if (Physics.Raycast(startPos, direction, out RaycastHit hit, maxDistance, collisionLayers))
        {
            distance = Mathf.Clamp(hit.distance - 0.05f, minDistance, maxDistance); 
        }

        Vector3 newPosition = startPos - direction * distance;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed); 
    }
}