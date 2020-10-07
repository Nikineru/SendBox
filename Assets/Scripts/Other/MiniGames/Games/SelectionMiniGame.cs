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
    private List<Vector3> ObjectsPos = new List<Vector3>();
    private void Start()
    {
        for (int o = 0; o < DragObjects.Count; o++)
        {
            int index = Random.Range(0, Colors.Count);
            if (UseColors.Contains(Colors[index]) == false)
                UseColors.Add(Colors[index]);
            else
                o--;
        }
        for(int j = 0;j<DragObjects.Count;j++)
        {
            GameObject Object = DragObjects[j];
            SpriteRenderer renderer = Object.GetComponent<SpriteRenderer>();

            renderer.color = UseColors[j];
            List<SpriteRenderer> HolesRenderes = Holes.Select(i => i.GetComponent<SpriteRenderer>()).Where(i => i.color == Color.white).ToList();
            HolesRenderes[Random.Range(0, HolesRenderes.Count)].color = UseColors[j];

            ObjectsPos.Add(Object.transform.localPosition);
            Object.GetComponent<DragObject>().MouseUpEvent += () =>
            {
                GameObject Target = Object.FindNearestInArray(Holes);
                int ObjectColorIndex = Colors.IndexOf(renderer.color);
                int HoleColorIdex = Colors.IndexOf(Target.GetComponent<SpriteRenderer>().color);
                if (ObjectColorIndex == HoleColorIdex)
                {
                    CurretScore++;
                    Object.SetActive(false);
                    if (CurretScore >= MaxWinScore)
                        Station.StopWork();
                }
            };
        }
    }
    public override void ResetGame() 
    {
        base.ResetGame();

        for(int i =0;i<DragObjects.Count;i++)
        {
            GameObject item = DragObjects[i];
            item.SetActive(true);
            item.transform.localPosition = ObjectsPos[i];
        }
    }
}
