using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionMiniGame : MiniGame
{
    public List<GameObject> DragObjects = new List<GameObject>();
    public List<GameObject> Holes = new List<GameObject>();
    public List<Color> Colors = new List<Color>();
    private List<Color> UseColors = new List<Color>();
    private List<LineRenderer> Lines = new List<LineRenderer>();
    public List<Transform> StartPoses = new List<Transform>();
    private void Start()
    {
        ResetColors();
        foreach(var Object in DragObjects)
        {
            LineRenderer line = Object.GetComponent<LineRenderer>();
            Lines.Add(line);
            Color color = Object.GetComponent<SpriteRenderer>().color;
            line.endColor = color;
            line.startColor = color;
            Object.GetComponent<DragObject>().MouseUpEvent += () =>
            {
                GameObject Target = Object.FindNearestInArray(Holes);
                int ObjectColorIndex = Colors.IndexOf(Object.GetComponentInChildren<SpriteRenderer>().color);
                int HoleColorIdex = Colors.IndexOf(Target.GetComponent<SpriteRenderer>().color);
                if (ObjectColorIndex == HoleColorIdex&&Object.GetDistanse(Target)<2)
                {
                    CurretScore++;
                    Object.GetComponent<DragObject>().IsAbleToMove = false;
                    if (CurretScore >= MaxWinScore)
                    {
                        Station.StopWork();
                    }
                }
            };
        }
    }
    public void ResetColors()
    {
        UseColors = new List<Color>();
        List<Color> Buffer = new List<Color>();
        foreach (var item in Colors)
            Buffer.Add(item);

        foreach (var hole in Holes)
            hole.GetComponentInChildren<SpriteRenderer>().color = Color.white;

        for (int i = 0; i < DragObjects.Count; i++)
        {
            Color color = Buffer[Random.Range(0, Buffer.Count)];
            UseColors.Add(color);
            Buffer.Remove(color);
        }

        for (int j = 0; j < DragObjects.Count; j++)
        {
            Color color = UseColors.FirstOrDefault(i => i != null);
            GameObject Object = DragObjects[j];
            SpriteRenderer renderer = Object.GetComponent<SpriteRenderer>();
            renderer.color = color;
            List<SpriteRenderer> HolesRenderes = Holes.Select(i => i.GetComponent<SpriteRenderer>()).Where(i => i.color == Color.white).ToList();
            HolesRenderes[Random.Range(0, HolesRenderes.Count)].color = color;
            LineRenderer line = Object.GetComponent<LineRenderer>();
            line.endColor = color;
            line.startColor = color;
            UseColors.Remove(color);
        }
    }
    private void Update()
    {
        for (int i = 0; i < DragObjects.Count; i++)
        {
            Lines[i].SetPosition(0, StartPoses[i].position);
            Lines[i].SetPosition(1, DragObjects[i].transform.position);
        }
    }
    public override void ResetGame() 
    {
        base.ResetGame();

        for(int i =0;i<DragObjects.Count;i++)
        {
            GameObject item = DragObjects[i];
            Lines[i].SetPosition(0, new Vector2(0,0));
            Lines[i].SetPosition(1, new Vector2(0,0));
            item.GetComponent<DragObject>().IsAbleToMove = true;
            item.transform.localPosition = item.GetComponent<DragObject>().StartLocalPos;
        }      
        ResetColors();
    }
}
