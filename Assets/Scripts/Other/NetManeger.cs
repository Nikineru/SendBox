using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetManeger : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject DoorPrefab;
    private void Start()
    {
        var pos = new Vector3(Random.Range(-5f, 5f), 0, 0);
        PhotonNetwork.Instantiate(PlayerPrefab.name,pos,Quaternion.identity);
    }
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        //Когда текущий игрок покидает комнату
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} has joined");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} has left");
    }
}
