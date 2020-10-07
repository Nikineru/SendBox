using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMiniGame : MiniGame
{
    public List<GameObject> DragObjects = new List<GameObject>();
    public List<GameObject> Holes = new List<GameObject>();
    public List<Color> Colors = new List<Color>();
    private void Start()
    {
        for(int i = 0;i<DragObjects.Count;i++)
        {
            GameObject Object = DragObjects[i];
            Holes[i].GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Holes.Count)];
            SpriteRenderer renderer = Object.GetComponent<SpriteRenderer>();
            renderer.color = Colors[Random.Range(0, Holes.Count)];
            Object.GetComponent<DragObject>().MouseUpEvent += () =>
            {
                GameObject Target = Object.FindNearestInArray(Holes);
                int ObjectColorIndex = Colors.IndexOf(renderer.color);
                int HoleColorIdex = Colors.IndexOf(Target.GetComponent<SpriteRenderer>().color);
                if (ObjectColorIndex == HoleColorIdex)
                {
                    CurretScore++;
                    Debug.Log("Won");
                }
            };
        }
    }
}
