using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]
public class ImageReveal : MonoBehaviour
{
    public Image image;
    public int rows = 20; 
    public float delay = 0.05f;

    private RectMask2D mask;
    private Animator animator;

    void Awake()
    {
        mask = GetComponent<RectMask2D>();
        animator = image.GetComponent<Animator>();
        ChangeYPadding(image.rectTransform.rect.height);

        if (animator != null) 
        {
            animator.enabled = false;
        }
    }

    private void Start()
    {
        StartCoroutine(RevealImage());
    }

    private void ChangeYPadding(float y)
    {
        Vector4 padding = mask.padding;
        padding.y = y;
        mask.padding = padding;
    }

    IEnumerator RevealImage()
    {
        for (int i = rows; i >= 0; i--)
        {
            float percent = (float)i / rows;
            print(percent);
            ChangeYPadding(image.rectTransform.rect.height * percent);
            yield return new WaitForSeconds(delay);
        }

        // maybe use decorators?
        if (animator != null) 
        {
            animator.enabled = true;
        }
    }
}