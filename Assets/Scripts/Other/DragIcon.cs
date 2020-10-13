using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : DragObject
{
    public Color OutColor;
    public event System.Action<GameObject, Vector3, bool> MouseUp;
    public bool IsCurret;
    private bool IsOut;
    private Inventory inventory;
    private SpriteRenderer renderer;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        renderer = GetComponent<SpriteRenderer>();
    }
    public override void Move()
    {
        base.Move();

        IsOut = transform.localPosition.x >= 6 || transform.localPosition.x <= -6 || transform.localPosition.y >= 2.6f;
        if (IsOut)
            renderer.color = Color.Lerp(renderer.color, OutColor, Time.deltaTime * 4f);
        else
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime * 4f);
    }
    private void OnMouseDrag()
    {
        Move();
    }
    private void OnMouseUp()
    {
        MouseUp.Invoke(gameObject, StartPos, IsOut);
    }
    private void Update()
    {
        if(IsCurret)
            renderer.color = Color.Lerp(renderer.color, Color.yellow, Time.deltaTime * 4f);
        else
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime * 4f);
    }
}
