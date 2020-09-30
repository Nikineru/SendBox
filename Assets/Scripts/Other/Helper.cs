using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Helper
{
    public static void MoveToVector(this GameObject _object, Vector2 direction,float speed)
    {
        Rigidbody2D body = _object.GetComponentInChildren<Rigidbody2D>();
        body.MovePosition(body.position + direction * speed);
    }
    public static Vector2 GetDirection(this GameObject _object, GameObject target)
    {
        var direction = (_object.transform.position - target.transform.position).normalized;
        return direction;
    }
    public static float GetDistanse(this GameObject _object, GameObject target)
    {
        var direction = (_object.transform.position - target.transform.position).magnitude;
        return direction;
    }
    public static float GetDistanse(this GameObject _object, Vector3 target)
    {
        var direction = (_object.transform.position - target).magnitude;
        return direction;
    }
    public static GameObject FindNearestObjectOfType<T>(this GameObject Object)where T:Component
    {
        List<GameObject> Objects = GameObject.FindObjectsOfType<T>().Select(i=>i.gameObject).ToList();
        if (Objects.Contains(Object))
            Objects.Remove(Object);
        try
        {
            float result = Object.GetDistanse(Objects[0]);
            for (int i = 1; i < Objects.Count; i++)
            {
                float distanse = Object.GetDistanse(Objects[i]);
                if (distanse < result)
                    result = distanse;
            }
            return Objects.FirstOrDefault(i => i.GetDistanse(Object) == result);
        }
        catch
        {
            Debug.Log("Нет таких обьеков :<");
            return null;
        }
    }
    public static GameObject FindNearestInArray(this GameObject Object, List<GameObject> Objects)
    {
        float NearestDis = 0;
        if (Objects.Contains(Object))
            Objects.Remove(Object);
        foreach (var item in Objects)
        {
            if (Object.GetDistanse(item) < NearestDis)
                NearestDis = Object.GetDistanse(item);
        }
        return Objects.FirstOrDefault(i => i.GetDistanse(Object) == NearestDis);
    }
    public static List<T> GetChildrensOfType<T>(this GameObject Object) where T : Component
    {
        List<T> Childrens = new List<T>();
        var childrens = Object.GetComponentsInChildren<T>();
        for(int i = 1;i< childrens.Length;i++)
        {
            Childrens.Add(childrens[i]); 
        }
        return Childrens;
    }
}
