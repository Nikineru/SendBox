using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<int, GameObject> Items = new Dictionary<int, GameObject>();
    public float ItemsCount = 5;
    public bool IsFull;
    public float PickUpDistanse;
    private List<int> BusyPlaces = new List<int>();
    private List<SpriteRenderer> Icons = new List<SpriteRenderer>();

    #region Show Inventory Logic
    public bool IsOpen;
    private float StartPoint;
    private GameObject inventory;
    private Camera PlayerCamera;
    #endregion
    private void Start()
    {
        PlayerCamera = GetComponentInChildren<Camera>();
        inventory = transform.Find("Inventory").gameObject;
        StartPoint = inventory.transform.localPosition.y;
        for (int i = 0; i < ItemsCount; i++)
            Items.Add(i, null);
        for (int i = 0; i < ItemsCount; i++)
        {
            BusyPlaces.Add(-1);
        }
        Icons = inventory.GetChildrensOfType<SpriteRenderer>();

        foreach (var item in Icons)
        {
            item.GetComponent<DragObject>().MouseUp += ChangePlaceOfItem;
        }
    }
    public void ChangePlaceOfItem(GameObject icon,Vector3 StartPos,bool IsOut)
    {
        GameObject NearestIcon = icon.FindNearestObjectOfType<DragObject>();

        var NewSpite = NearestIcon.GetComponentInChildren<SpriteRenderer>();
        int NewIndex = Icons.IndexOf(NewSpite);
        var OldSprite = icon.GetComponentInChildren<SpriteRenderer>();
        int OldIndex = Icons.IndexOf(OldSprite);
        if (IsOut)
        {
            Drop(OldIndex);
        }
        else if(BusyPlaces.Contains(NewIndex) == false)
        {
            NewSpite.sprite = OldSprite.sprite;
            OldSprite.sprite = null;
            BusyPlaces[OldIndex] = -1;
            BusyPlaces[NewIndex] = NewIndex;
            Items[NewIndex] = Items[OldIndex];
            Items[OldIndex] = null;
        }
        icon.transform.localPosition = StartPos;
    }

    private void Drop(int index)
    {
        GameObject item = Items[index];
        item.transform.position = transform.position;
        item.transform.Translate(-transform.right * item.GetComponent<RectTransform>().rect.width);
        item.SetActive(true);
        BusyPlaces[index] = -1;
        IsFull = false;
        Icons[index].GetComponent<SpriteRenderer>().sprite = null;
    }

    public void PickUpItem()
    {
        if (IsFull == false)
        {
            GameObject Item = gameObject.FindNearestObjectOfType<Item>();
            if (Item == null)
                return;
            if (gameObject.GetDistanse(Item) < PickUpDistanse)
            {
                if (BusyPlaces.Contains(-1) == false)
                {
                    IsFull = true;
                    return;
                }
                int Index = BusyPlaces.IndexOf(BusyPlaces.First(i => i == -1));

                Item itemProps = Item.GetComponent<Item>();
                Items[Index] = Item;
                BusyPlaces[Index] = Index;
                Item.SetActive(false);
                Icons[Index].sprite = itemProps.Icon;
        }
        }
    }
    public void ShowInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            IsOpen = !IsOpen;

        Vector3 pos = inventory.transform.position;
        if (IsOpen == false)
        {
            Vector3 viewPos = PlayerCamera.WorldToViewportPoint(inventory.transform.position);
            if (viewPos.y >= -0.15f)
                pos.y -= 0.1f;
        }
        else if (inventory.transform.localPosition.y <= StartPoint)
            pos.y += 0.1f;

        inventory.transform.position = pos;
    }
    private void Update()
    {
        ShowInventory();
        if (Input.GetKeyDown(KeyCode.F))
            PickUpItem();
    }
}
