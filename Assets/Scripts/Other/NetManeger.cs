using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Characteristicks;

public class NetManeger : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject DoorPrefab;
    public List<GameObject> Cards = new List<GameObject>();
    private Dictionary<Roles, SpawnInfo> RolesSpawnInfo = new Dictionary<Roles, SpawnInfo>()
    {
        {Roles.ClassD,new SpawnInfo(new Vector3(-26,0,0),null)},
        {Roles.Cleaner,new SpawnInfo(new Vector3(-14,0,0),null)},
        {Roles.Scientist,new SpawnInfo(new Vector3(0,0,0),null)},
        {Roles.Engineer,new SpawnInfo(new Vector3(12,0,0),null)},
        {Roles.Manager,new SpawnInfo(new Vector3(26,0,0),null)},
    };
    public struct SpawnInfo 
    {
       public Vector3 SpawnPosition;
       public List<GameObject> StartItems;

        public SpawnInfo(Vector3 spawnPosition, List<GameObject> startItems)
        {
            SpawnPosition = spawnPosition;
            StartItems = startItems;
        }
    }
    private void Start()
    {
        Roles Role = (Roles)Random.Range(0, System.Enum.GetValues(typeof(Roles)).Length);
        SpawnInfo Info = RolesSpawnInfo[Role];
        GameObject Player = PhotonNetwork.Instantiate(PlayerPrefab.name, Info.SpawnPosition, Quaternion.identity);
        Player.GetComponent<Characteristicks>().Role = Role;
        if (Role == Roles.ClassD)
            return;

        Inventory inventory = Player.GetComponent<Inventory>();
        inventory.OnEnventoryStarted += () =>
        {
            GameObject Card = PhotonNetwork.Instantiate(Cards[(int)Role].name, Info.SpawnPosition, Quaternion.identity);
            inventory.AddToInventory(Card);
        };
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
