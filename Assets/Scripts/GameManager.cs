using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    public GameObject mokia;
    public StaminaBar staminaBar;
    public Canvas innerUI;
    public GameObject CameraHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    


}
