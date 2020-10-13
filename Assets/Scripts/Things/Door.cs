using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Characteristicks;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour,IPunObservable
{
    public bool IsLock;
    public float SpeedOfOpening;
    public Roles AccessibilityLevel;
    private AudioSource AudioPlayer;
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
        AudioPlayer = GetComponentInChildren<AudioSource>();
    }
    public void Open()
    {
        Inventory inventory = gameObject.FindNearestObjectOfType<Inventory>().GetComponent<Inventory>();
        KeyCard card = null;
        bool AcceptAccessibility = false;
        try
        {
            card = inventory.CurretItem.GetComponent<KeyCard>();
            AcceptAccessibility = (int)card.AccessibilityLevel >= (int)AccessibilityLevel;
        }
        catch
        {
                AcceptAccessibility = AccessibilityLevel == Roles.ClassD;
        }   

        if (IsLock == false && IsRunning==false&&IsAbleToOpen&&AcceptAccessibility)
        {
            OpeningCorutin = StartCoroutine(Opening());
            AudioPlayer.Play();
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
                    AudioPlayer.Stop();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsLock);
            stream.SendNext(IsAbleToOpen);
            stream.SendNext(IsRunning);
            stream.SendNext(Direction);
        }
        else
        {
            IsLock = (bool)stream.ReceiveNext();
            IsAbleToOpen = (bool)stream.ReceiveNext();
            IsRunning = (bool)stream.ReceiveNext();
            Direction = (Directions)stream.ReceiveNext();
        }
    }
}
