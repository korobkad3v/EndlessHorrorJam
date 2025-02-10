using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTextureUpdater : MonoBehaviour
{
    public int targetFPS = 20;
    public float frameTime;
    private float elapsed;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
        frameTime = 1f / targetFPS;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= frameTime)
        {
            elapsed -= frameTime;
            cam.Render();
        }
    }
}
