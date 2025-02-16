using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI clockText;
    public float timeMultiplier = 10f;  
    public float resetTime = 300f; 
    
    private float currentTime;

    public void Awake()
    {
        clockText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        currentTime = 0;  
    }

    private void Update()
    {
        currentTime += Time.deltaTime * timeMultiplier;

        if (currentTime  >= resetTime)
        {
            currentTime  = 0;
        }
        int hours = Mathf.FloorToInt(currentTime / 60f);
        int minutes = Mathf.FloorToInt(currentTime % 60f);

        clockText.text = $"{hours:D2}:{minutes:D2}";
    }
}
