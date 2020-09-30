using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsLock;
    public float SpeedOfOpening;

    #region OpenLogick
    private Vector2 EndPoint;
    private Vector2 StartPoint;
    private float Height => GetComponent<RectTransform>().rect.yMax * 2;
    private Coroutine OpeningCorutin;
    private bool IsRunning;
    private bool IsAbleToOpen = true;
    private bool IsOpened = false;
    #endregion
    private enum Directions
    {
        down = -1,
        neutral,
        up
    }
    [SerializeField]private Directions Direction;
    private void Start()
    {
        EndPoint = transform.position;
        EndPoint.y = Height;
        StartPoint = transform.position;
    }
    public void Open()
    {
        if (IsLock == false && IsRunning==false&&IsAbleToOpen)
        {
            OpeningCorutin = StartCoroutine(Opening()); 
            IsRunning = true;
        }
    }
    private IEnumerator Opening()
    {
        bool Stop = false;
        Direction = (IsOpened) ? Directions.down : Directions.up;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (IsAbleToOpen == false)
            {
                if (IsRunning&&Direction==Directions.down)
                {
                    Direction = Directions.up;
                    IsRunning = false;
                    IsOpened = !IsOpened;
                }
            }
            if (IsOpened == false)
            {
                if (transform.position.y >= EndPoint.y)
                {
                    IsOpened = true;
                    IsRunning = false;
                    Stop = true;
                }
            }
            else if(transform.position.y <= StartPoint.y)
            {
                    IsOpened = false;
                    IsRunning = false;
                    Stop = true;
            }
            if (Stop)
            {
                StopCoroutine(OpeningCorutin);
                Direction = Directions.neutral;
            }
            else
            {
                int kof = (int)Direction;
                transform.position = new Vector3(transform.position.x, transform.position.y + kof*SpeedOfOpening, transform.position.z);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Wall")
            IsAbleToOpen = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsAbleToOpen = true;
    }
}
