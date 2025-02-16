using UnityEngine;

public interface IButton
{
    bool IsPressed { get; }
    public Vector3 PressedPosition { get; }

    private void Press() {}

    private void OnMove() {}
}

