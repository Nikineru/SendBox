using UnityEngine;

public class DragObject : MonoBehaviour
{
    public event System.Action<GameObject,Vector3,bool> MouseUp;
    private Vector3 StartPos;
    private void Start()
    {
        StartPos = transform.localPosition;
    }
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
    private void OnMouseDrag()
    {
        var Position = GetMouseAsWorldPoint();
        transform.position = Position;
        float y = Mathf.Clamp(transform.localPosition.y, StartPos.y, 3);
        float x = Mathf.Clamp(transform.localPosition.x, -6.5f,6.5f);
        transform.localPosition = new Vector3(x,y,StartPos.z);
    }
    private void OnMouseUp()
    {
        bool IsOut = transform.localPosition.x >= 6 || transform.localPosition.x <= -6 || transform.localPosition.y >= 2.6f;
        MouseUp.Invoke(gameObject,StartPos,IsOut);
    }
}