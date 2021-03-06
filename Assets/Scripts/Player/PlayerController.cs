﻿using Photon.Pun;
using UnityEngine;
using static Characteristicks;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Move of player
    public bool Flip;
    public float SpeedOfMove;
    private float RunSpeed;
    private float WalkSpeed;
    private Vector2 MoveVelocity;
    private bool Rotation;
    //<summary> Run Logick
    private bool BeAbleToRun = true;
    private Coroutine StaminaChange;
    //</summary>
    #endregion
    private PhotonView photonView;
    public float DistanseForDoor = 5;
    [HideInInspector]public Characteristicks playerChara;
    private SpriteRenderer SpriteRender;
    private void Start()
    {
        GameObject.Find("Player UI").GetComponent<Canvas>().worldCamera = GetComponentInChildren<Camera>();

        photonView = GetComponent<PhotonView>();
        RunSpeed = SpeedOfMove * 1.75f;
        WalkSpeed = SpeedOfMove;
        playerChara = GetComponentInChildren<Characteristicks>();
        SpriteRender = GetComponentInChildren<SpriteRenderer>();

        StartCoroutine(playerChara.ChangeSmothing(Properties.Hunger,DropValue: 0.03f));
        StartCoroutine(playerChara.ChangeSmothing(Properties.Thirst, DropValue: 0.04f));
        StartCoroutine(playerChara.ChangeSmothing(Properties.Mind, DropValue: 0.01f));
    }
    public void OpenTheDoor()
    {
        if (FindObjectOfType<Door>() == null)
            return;

        GameObject door = gameObject.FindNearestObjectOfType<Door>();
        if (gameObject.GetDistanse(door) > DistanseForDoor)
            return;

        door.GetComponent<Door>().Open();
    }
    #region Run Logic
    public void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift)&&BeAbleToRun&&playerChara.Stamina>0)
        {
                if (StaminaChange != null)
                    StopCoroutine(StaminaChange);

                BeAbleToRun = false;
                SpeedOfMove = RunSpeed;
                StaminaChange = StartCoroutine(playerChara.ChangeSmothing(Properties.Stamina,DropValue: 0.2f,EndValue:0,OnEndAction:GetRest));
        }
        if(Input.GetKey(KeyCode.LeftShift)==false&&BeAbleToRun==false)
        {
            GetRest();
        }
    }
    private void GetRest()
    {
        if (StaminaChange != null)
            StopCoroutine(StaminaChange);

        SpeedOfMove = WalkSpeed;
        StaminaChange = StartCoroutine(playerChara.ChangeSmothing(Properties.Stamina, DropValue: 0.2f, EndValue: 100, WaitTime: 2));  
        BeAbleToRun = true;
    }
    #endregion
    private void Update()
    {
        if (photonView.IsMine == false)
            return;

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveVelocity = moveInput.normalized * SpeedOfMove;
        MoveVelocity = Flip ? new Vector2(-MoveVelocity.x, -MoveVelocity.y) : MoveVelocity;

        if (MoveVelocity.x < 0)
            Rotation = Flip ? false : true;
        if (MoveVelocity.x > 0)
            Rotation = Flip?true:false;

        SpriteRender.flipX = Rotation;

        if (Input.GetKeyDown(KeyCode.E))
            OpenTheDoor();
    }
    private void FixedUpdate()
    {
        if (photonView.IsMine == false)
        {
            transform.Find("Inventory").gameObject.SetActive(false);
            return;
        }

        gameObject.MoveToVector(MoveVelocity,Time.fixedDeltaTime);
        Run();
    }
}
