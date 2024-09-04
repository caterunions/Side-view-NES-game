using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Vector2 _lastAimInput = new Vector2();

    public void HandleAim(Vector2 aimInput)
    {
        _lastAimInput = aimInput;
    }

    private void Update()
    {
        Vector2 facingDirection = _lastAimInput - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
