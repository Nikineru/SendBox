using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public int WinScore;
    public int MaxWinScore;
    public int CurretScore;
    public enum Quality
    {
        bad,
        medium,
        good
    }
    public Quality QualityMark;
}
