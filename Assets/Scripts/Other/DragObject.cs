using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Color OutColor;
    public event System.Action<GameObject,Vector3,bool> MouseUp;
    private Vector3 StartPos;
    private bool IsOut;
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
        float y = transform.localPosition.y;
        float x = transform.localPosition.x;
        transform.localPosition = new Vector3(x,y,StartPos.z);
        IsOut = transform.localPosition.x >= 6 || transform.localPosition.x <= -6 || transform.localPosition.y >= 2.6f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (IsOut)
            renderer.color = Color.Lerp(renderer.color,OutColor,Time.deltaTime*2f);
        else
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime*2f);
    }
    private void OnMouseUp()
    {
        MouseUp.Invoke(gameObject,StartPos,IsOut);
    }
}