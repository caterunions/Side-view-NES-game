using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private void Update()
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Mouse.current.position.ReadValue(), _canvas.worldCamera, out pos);

        transform.position = _canvas.transform.TransformPoint(pos);
    }
}
