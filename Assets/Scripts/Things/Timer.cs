using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region InputData
    [Header("Время с которого начнётся отсчёт")]
    public float StartSeconds;
    public float StartMinutes;
    public float StartHours;

    [Header("Время на котором закончится отсчёт")]
    public float EndSeconds;
    public float EndMinutes;
    public float EndHours;

    [Header("Скорость течения времени")]
    public float Speed = 1;
    #endregion

    public TimeData CurretTime;
    public TimeData EndTime;
    public event System.Action OnTimeIsUp;
    private bool IsWorking = false;
    public struct TimeData 
    {
        public float Seconds;
        public float Minutes;
        public float Hours;
        public float Speed;
        public bool IsAbleToWork;

        public TimeData(float seconds, float minutes, float hours, float period,bool IsWork)
        {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
            Speed = period;
            IsAbleToWork = IsWork;
        }

        public bool IsSame(TimeData Second) 
        {
            return (int)Seconds == (int)Second.Seconds && (int)Minutes == (int)Second.Minutes && (int)Hours == (int)Second.Hours;
        }
        public void CheckTime() 
        {
            if (Seconds >= 60) 
            {
                Seconds = 0;
                Minutes++;
            }
            if (Minutes>= 60)
            {
                Minutes = 0;
                Hours++;
            }
        }
        public void Tick() 
        {
            if (IsAbleToWork == false)
                return;

            CheckTime();
            Seconds += Time.deltaTime * Speed;
        }
        public override string ToString()
        {
            string seconds = Seconds >= 10 ? ((int)Seconds).ToString() : $"0{(int)Seconds}";
            string minutes = Minutes >= 10 ? Minutes.ToString() : $"0{Minutes}";
            string hour = Hours >= 10 ? Hours.ToString() : $"0{Hours}";
            return $"{hour}:{minutes}:{seconds}";
        }
    }
    public void CheckTimer(TimeData EndData) 
    {
        if (CurretTime.IsSame(EndData))
        {
            OnTimeIsUp?.Invoke();
            CurretTime.IsAbleToWork = false;
            IsWorking = false;
        }
    }
    public void StartTimer() 
    {
        CurretTime = new TimeData(StartSeconds, StartMinutes, StartHours, Speed, true);
        EndTime = new TimeData(EndSeconds, EndMinutes, EndHours, Speed, true);
        IsWorking = true;
    }
    private void Update()
    {
        if (IsWorking) 
        {
            CurretTime.Tick();
            CheckTimer(EndTime);
            Debug.Log(CurretTime.ToString());
        }
    }
}
