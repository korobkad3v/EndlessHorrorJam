using Unity.VisualScripting;
using UnityEngine;

public class DPad : MonoBehaviour
{
    public Vector2 clampValues = new Vector2(3, 3);
    public float smoothSpeed = 10f;
    

    public void RotateToZero() {
        transform.localRotation = 
            Quaternion.Slerp(transform.localRotation, 
                Quaternion.identity, 
                smoothSpeed * Time.deltaTime);
    }
}
