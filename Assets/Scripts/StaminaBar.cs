using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    public List<Sprite> states;
    public PlayerController playerController;
    private float changeFrameEveryStaminaUnits;

    private Dictionary<Sprite, float[]> map;

    void Awake()
    {
        changeFrameEveryStaminaUnits = 100f / states.Count;
        map = new Dictionary<Sprite, float[]>();
        for (int i = 0; i < states.Count; i++)
        {
            map.Add(states[i], new float[2] {i * changeFrameEveryStaminaUnits,(i + 1) * changeFrameEveryStaminaUnits});
        }
    }

    
    void Update()
    {
        foreach (KeyValuePair<Sprite, float[]> pair in map)
        {
            if (playerController.stamina >= pair.Value[0] && playerController.stamina <= pair.Value[1])
            {
                GetComponent<Image>().sprite = pair.Key;
            }
        }
    }
}
