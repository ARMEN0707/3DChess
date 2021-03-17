using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();

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

    private void GetPoint(List<Cell> points, int x, int y,int offsetX,int offsetY)
    {
        if (PointInBoard(x, y, offsetX, offsetY))
        {
            Chess chess = FindChess(x, y, offsetX, offsetY);
            if (chess == null)
            {
                points.Add(new Cell(x + dir * offsetX, y + dir * offsetY, true));
            }
            else
            {
                if(isWhite != chess.isWhite)
                {
                    points.Add(new Cell(x + dir * offsetX, y + dir * offsetY, false));
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
