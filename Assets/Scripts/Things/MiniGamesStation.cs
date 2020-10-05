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
    private Image BlackScreen;
    private Color ScreenColor;
    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        BlackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        Controller = Player.GetComponent<PlayerController>();
        ScreenColor = BlackScreen.color;
    }

    public void StartWork()
    {
        Controller.enabled = false;
        IsWorking = true;
        Color color = new Color(ScreenColor.r, ScreenColor.g, ScreenColor.b,100);
        BlackScreen.color = Color.Lerp(BlackScreen.color,color, Time.deltaTime * 2f);
    }
    public void StopWork()
    {
        Controller.enabled = true;
        IsWorking = false;
        Color color = new Color(ScreenColor.r, ScreenColor.g, ScreenColor.b, 0);
        BlackScreen.color = Color.Lerp(BlackScreen.color, color, Time.deltaTime * 2f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& Player.GetDistanse(gameObject) < TriggerDistance)
        {
            if (IsWorking==false)
                StartWork();
            else
                StopWork();
        }
    }
}
