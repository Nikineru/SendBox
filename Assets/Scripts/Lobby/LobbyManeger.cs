using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManeger : MonoBehaviourPunCallbacks
{
    public Text Log;
    private bool Joined = false;
    private void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 100);
        Write("Player`s name set to " + PhotonNetwork.NickName);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }
    public void CreateRoom()
    {
        if (Joined)
            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        else
            Write("You aren`t ready now");
    }
    public void JoinRandomRoom()
    {
        if(Joined)
        PhotonNetwork.JoinRandomRoom();
        else
            Write("You aren`t ready now");
    }
    public override void OnJoinedRoom()
    {
        Write("Joined Room");
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnConnectedToMaster()
    {
        Write("Connected to Master");
        Joined = true;
    }
    public void Write(string message)
    {
        Log.text += $"{message}\n";
        Debug.Log(message);
    }
}
