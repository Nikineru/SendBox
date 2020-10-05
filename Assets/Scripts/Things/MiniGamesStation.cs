using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesStation : MonoBehaviour
{
    public GameObject Player;
    public float TriggerDistance;
    public bool IsWorking = false;
    private PlayerController Controller;
    public Color WorkColor;
    public Color RestColor;
    private Image BlackScreen;

    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        BlackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        Controller = Player.GetComponent<PlayerController>();
    }

    public void StartWork()
    {
        Controller.enabled = false;
        IsWorking = true;
    }
    public void StopWork()
    {
        Controller.enabled = true;
        IsWorking = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& Player.GetDistanse(gameObject) < TriggerDistance)
        {
            if (IsWorking == false)
                StartWork();
            else
                StopWork();
        }
        if(IsWorking)
            BlackScreen.color = Color.Lerp(BlackScreen.color,WorkColor, Time.deltaTime * 4f);
        else
            BlackScreen.color = Color.Lerp(BlackScreen.color,RestColor, Time.deltaTime * 4f);
    }
}
