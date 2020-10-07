using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class Item : MonoBehaviour
{
    public Sprite Icon;

    public Color HoverColor;
    private Color SimpleColor;

    private bool IsHover=false;
    private SpriteRenderer Renderer;
    private Inventory Inventory;
    private void Start()
    {
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Inventory = FindObjectOfType<Inventory>();
        SimpleColor = Renderer.color;
    }
    private void OnMouseEnter()
    {
        IsHover = true;
        Inventory.CurretPickUpItem = gameObject;
    }
    private void OnMouseExit()
    {
        IsHover = false;
        Inventory.CurretPickUpItem = null;
    }
    private void Update()
    {
        if (IsHover)
            Renderer.color = Color.Lerp(Renderer.color, HoverColor, Time.deltaTime * 4);
        else
            Renderer.color = Color.Lerp(Renderer.color, SimpleColor, Time.deltaTime * 4);
    }
}
