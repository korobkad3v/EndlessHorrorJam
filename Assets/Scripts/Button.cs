using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public Vector3 pressedPosition;
    private Vector3 initialPosition;

    private Coroutine animationRoutine;
    public ButtonType buttonType;
    public enum ButtonType
    {
        left, 
        right,
        up,
        down,
        jump
    }

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 inputDir = inputValue.Get<Vector2>().normalized;
        bool shouldPress = false;

        switch (buttonType)
        {
            case ButtonType.left:
                shouldPress = inputDir.x < 0;
                break;
            case ButtonType.right:
                shouldPress = inputDir.x > 0;
                break;
            case ButtonType.up:
                shouldPress = inputDir.y > 0;
                break;
            case ButtonType.down:
                shouldPress = inputDir.y < 0;
                break;
        }

        if (shouldPress)
        {
            AudioManager.Instance.Play("click_2");
            StartButtonAnimation(pressedPosition);
        }
        else
        {
            StartButtonAnimation(initialPosition);
        }
    }

    private void OnJump(InputValue inputValue) 
    {
        if (buttonType == ButtonType.jump && inputValue.isPressed)
        {
            AudioManager.Instance.Play("click_2");
            StartButtonAnimation( pressedPosition);
            StartCoroutine(ReleaseAfterDelay(0.3f));
        }
    }

    private void StartButtonAnimation(Vector3 targetPos, float delay = 0f)
    {
        if (animationRoutine != null) StopCoroutine(animationRoutine);
        animationRoutine = StartCoroutine(AnimateButton(targetPos, delay));
    }

    private IEnumerator AnimateButton(Vector3 targetPos, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        float t = 0f;
        Vector3 startPos = transform.localPosition;
        
        while (t < 1f)
        {
            t += Time.deltaTime * 10f; 
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    private IEnumerator ReleaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartButtonAnimation(initialPosition);
    }
}
