using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManeger : MonoBehaviourPunCallbacks
{
    public Text Log;
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
        PhotonNetwork.CreateRoom(null,new Photon.Realtime.RoomOptions { MaxPlayers=2});
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Write("Joined Room");
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnConnectedToMaster()
    {
        Write("Connected to Master");
    }
    public void Write(string message)
    {
        Log.text += $"{message}\n";
        Debug.Log(message);
    }
}
