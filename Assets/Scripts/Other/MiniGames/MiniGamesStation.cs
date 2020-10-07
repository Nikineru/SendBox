using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesStation : MonoBehaviour
{
    public List<GameObject> MiniGames = new List<GameObject>();
    public Color WorkColor;
    public Color RestColor;
    private PlayerController PlayerController;
    private Inventory Inventory;
    private bool IsWorking = false;
    private Image BlackScreen;
    private GameObject Game;
    private Vector3 GameEndPos = Vector3.zero;

    private void Start()
    {
        PlayerController = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        Inventory = PlayerController.gameObject.GetComponent<Inventory>();
        BlackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartWork();
    }
    public void StartWork()
    {
        PlayerController.enabled = false;
        IsWorking = true;
        Game = MiniGames[Random.Range(0, MiniGames.Count)];
        Game.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, -Screen.height / 2, 0));
        GameEndPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Game.SetActive(true);
        Inventory.IsOpen = false;
        Inventory.IsLock = true;
    }
    public void StopWork()
    {
        PlayerController.enabled = true;
        IsWorking = false;
        Inventory.IsOpen = true;
        Inventory.IsLock = false;
    }
    private void Update()
    {
        if (IsWorking)
        {
            if (Input.GetKeyDown(KeyCode.E))
                StopWork();

            BlackScreen.color = Color.Lerp(BlackScreen.color, WorkColor, Time.deltaTime * 4f);
            Game.transform.position = Vector3.Lerp(Game.transform.position, GameEndPos, Time.fixedTime * 0.0035f);
            Game.transform.localPosition = new Vector3(Game.transform.localPosition.x, Game.transform.localPosition.y, 0);
        }
        else
        {
            BlackScreen.color = Color.Lerp(BlackScreen.color, RestColor, Time.deltaTime * 3.5f);

            if (Game != null)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(Game.transform.position);
                if (viewPos.y >= -1)
                    Game.transform.position = new Vector3(Game.transform.position.x, Game.transform.position.y - 0.02f*Time.fixedTime, Game.transform.position.z);
                else
                    Game.SetActive(false);
            }     
        } 
    }
}
