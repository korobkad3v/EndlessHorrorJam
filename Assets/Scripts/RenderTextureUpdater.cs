using UnityEngine;

public class RenderTextureUpdater : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera renderCamera;
    public int targetFrameRate = 30; 
    private float timeBetweenUpdates; 
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        timeBetweenUpdates = 1f / targetFrameRate;
    }

    void Update()
    {
        
        timeSinceLastUpdate += Time.deltaTime;

        
        if (timeSinceLastUpdate >= timeBetweenUpdates)
        {
            timeSinceLastUpdate = 0f; // Сбросить таймер
            renderCamera.targetTexture = renderTexture;
            renderCamera.Render(); // Обновляем RenderTexture
        }
    }
}
