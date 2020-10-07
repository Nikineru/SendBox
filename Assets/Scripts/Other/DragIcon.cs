using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : DragObject
{
    public Color OutColor;
    public event System.Action<GameObject, Vector3, bool> MouseUp;
    private bool IsOut;

    public override void Move()
    {
        base.Move();

        IsOut = transform.localPosition.x >= 6 || transform.localPosition.x <= -6 || transform.localPosition.y >= 2.6f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (IsOut)
            renderer.color = Color.Lerp(renderer.color, OutColor, Time.deltaTime * 2f);
        else
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime * 2f);
    }
    private void OnMouseDrag()
    {
        Move();
    }
    private void OnMouseUp()
    {
        MouseUp.Invoke(gameObject, StartPos, IsOut);
    }
}
