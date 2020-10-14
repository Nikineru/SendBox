using System;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Vector3 StartLocalPos;
    public Vector3 StartPos;
    public event Action MouseUpEvent;
    public event Action MouseDownEvent;
    public event Action MouseDragEvent;
    public bool LockMove = false;
    public Vector2 MaxPos;
    public Vector2 MinPos;
    private void Start()
    {
        StartLocalPos = transform.localPosition;
        StartPos = transform.position;
    }
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private void OnMouseUp()
    {
        MouseUpEvent?.Invoke();
    }
    private void OnMouseDown()
    {
        MouseDownEvent?.Invoke();
        StartPos = transform.position;
    }
    private void OnMouseDrag()
    {
        Move();
    }
    public virtual void Move()
    {
        var Position = GetMouseAsWorldPoint();;

        transform.position = new Vector3(Position.x,Position.y, transform.position.z);

        if (LockMove)
        {
            float x = Mathf.Clamp(transform.localPosition.x, MinPos.x, MaxPos.x);
            float y = Mathf.Clamp(transform.localPosition.y, MinPos.y, MaxPos.y);
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }
        MouseDragEvent?.Invoke();
    }
}