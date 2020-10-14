using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIcon : DragObject
{
    public Color OutColor;
    public Color IsCurretColor;
    public event System.Action<GameObject, Vector3, bool> MouseUp;
    public bool IsCurret;
    private bool IsOut;
    private Inventory inventory;
    private SpriteRenderer IconRenderer;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        IconRenderer = GetComponent<SpriteRenderer>();
        StartLocalPos = transform.localPosition;
    }
    public override void Move()
    {
        base.Move();

        IsOut = transform.localPosition.x >= 6 || transform.localPosition.x <= -6 || transform.localPosition.y >= 2.6f;
        if (IsOut)
            ChangeColor(OutColor, 4f);
        else
            ChangeColor(Color.white, 4f);
    }
    public void ChangeColor(Color NewColor,float Speed)
    {
        IconRenderer.color = Color.Lerp(IconRenderer.color, NewColor, Time.deltaTime * 4f);
    }
    private void OnMouseDrag()
    {
        Move();
    }
    private void OnMouseUp()
    {
        MouseUp.Invoke(gameObject, StartLocalPos, IsOut);
    }
    private void Update()
    {
        if (IsCurret)
            ChangeColor(IsCurretColor, 4f);
        else
            ChangeColor(Color.white, 4f);
    }
}
