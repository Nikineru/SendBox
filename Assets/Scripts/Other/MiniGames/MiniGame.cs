using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public int MaxWinScore;
    public int CurretScore;
    public MiniGamesStation Station;
    public enum Quality
    {
        bad,
        medium,
        good
    }
    public Quality QualityMark;
    public virtual void ResetGame() 
    {
        CurretScore = 0;
    }
}
