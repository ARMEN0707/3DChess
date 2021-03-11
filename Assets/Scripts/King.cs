using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public override List<(int, int)> GetPointForMove(int x, int y)
    {
        List<(int, int)> points = new List<(int, int)>();

        GetPoint(points,x,y,0,1);
        GetPoint(points,x,y,1,1);
        GetPoint(points,x,y,1,0);
        GetPoint(points,x,y,1,-1);
        GetPoint(points,x,y,0,-1);
        GetPoint(points,x,y,-1,-1);
        GetPoint(points,x,y,-1,0);
        GetPoint(points,x,y,-1,1);

        Debug.Log("King");
        return points;
    }

    private void GetPoint(List<(int, int)> points, int x, int y,int a,int b)
    {
        if (PointInBoard(x, y, a, b))
        {
            Chess chess = FindChess(x, y, a, b);
            if (chess == null)
            {
                points.Add((x + n * a, y + n * b));
            }
            else
            {
                if(isWhite != chess.isWhite)
                {
                    points.Add((x + n * a, y + n * b));
                }
            }

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetDir();
    }

    // Update is called once per frame
    void Update()
    {
        MoveChess(point);
    }
}
