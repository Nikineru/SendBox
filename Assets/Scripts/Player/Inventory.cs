using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnEnventoryStarted;
    public List<GameObject> Items = new List<GameObject>();
    public float ItemsCount = 5;
    public bool IsFull;
    public float PickUpDistanse;
    public GameObject CurretPickUpItem;
    private List<int> BusyPlaces = new List<int>();
    private List<SpriteRenderer> Icons = new List<SpriteRenderer>();

    #region Show Inventory Logic
    public bool IsOpen;
    public bool IsLock=false;
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
            Items.Add(null);
        for (int i = 0; i < ItemsCount; i++)
            BusyPlaces.Add(-1);
        Icons = inventory.GetChildrensOfType<SpriteRenderer>().Where(i=>i.name.Contains("Icon")).ToList();

        foreach (var item in Icons)
        {
            item.GetComponent<DragIcon>().MouseUp += ChangePlaceOfItem;
        }
        OnEnventoryStarted?.Invoke();
    }
    public void ChangePlaceOfItem(GameObject icon,Vector3 StartPos,bool IsOut)
    {
        GameObject NearestIcon = icon.FindNearestObjectOfType<DragIcon>();

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
        Debug.Log(index);
        GameObject item = Items[index];
        Vector3 SpawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float xKof = item.GetComponent<RectTransform>().rect.width;
        float yKof = item.GetComponent<RectTransform>().rect.height;

        float x = Mathf.Clamp(SpawnPos.x, transform.position.x - xKof, transform.position.x + xKof);
        float y = Mathf.Clamp(SpawnPos.y, transform.position.y - yKof, transform.position.y + yKof);

        item.transform.position = new Vector3(x, y, transform.position.z);  
        item.SetActive(true);
        BusyPlaces[index] = -1;
        IsFull = false;
        SpriteRenderer renderer = Icons[index].GetComponent<SpriteRenderer>();
        renderer.color = Color.white;
        renderer.sprite = null;
        Items[index] = null;
    }
    public void AddToInventory(GameObject Item) 
    {
        if (Item == null)
            return;
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
    public void PickUpItem()
    {
        if (IsFull == false)
        {
            if(CurretPickUpItem==null)
            CurretPickUpItem = gameObject.FindNearestObjectOfType<Item>();

            if (gameObject.GetDistanse(CurretPickUpItem) < PickUpDistanse)
            {
                AddToInventory(CurretPickUpItem);
                CurretPickUpItem = null;
            }
        }
    }
    public void ShowInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab)&&IsLock==false)
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
