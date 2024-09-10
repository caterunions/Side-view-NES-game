using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorWhenEnabled : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
