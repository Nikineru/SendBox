using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristicks : MonoBehaviour
{
    public float Stamina => Features[Properties.Stamina];
    public float Health => Features[Properties.Health];
    public float Hunger => Features[Properties.Hunger];

    private Dictionary<Properties, float> Features = new Dictionary<Properties, float>()
    {
        {Properties.Stamina,100},
        {Properties.Health,100},
        {Properties.Hunger,100}
    };

    public enum Properties
    {
        Stamina,
        Health,
        Hunger
    }
    public float this[Properties chara]
    {
        get
        {
            return Features[chara];
        }
        set
        {
            Features[chara] = value;
        }
    }
    public IEnumerator ChangeSmothing(Properties chara, float DropValue, float EndValue = 0,float WaitTime = 0,Action OnEndAction = null)
    {
        float Chara = this[chara];
        bool bigger = (Chara > EndValue);

        yield return new WaitForSeconds(WaitTime);
        if (bigger)
        {
            while (Chara > EndValue)
            {
                yield return new WaitForFixedUpdate();
                this[chara]-= DropValue;
            }
        }
        else
        {
            while (Chara < EndValue)
            {
                yield return new WaitForFixedUpdate();
                this[chara] += DropValue;
            }
        }
        OnEndAction?.Invoke();
    }
}
