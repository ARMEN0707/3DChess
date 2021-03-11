using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chess
{
    public override List<(int, int)> GetPointForMove(int x, int y)
    {
        List<(int, int)> points = new List<(int, int)>();

        //горизонталь
        int a = 2 , b = 1;
        Chess chess;
        for (int i = 1; i <= 4; i++,b = -b)
        {            
            if(PointInBoard(x,y,a,b))
            {
                chess = FindChess(x, y, a, b);
                if(chess == null)
                {
                    points.Add((x + a * n, y + b * n));
                }else
                {
                    if(isWhite != chess.isWhite)
                    {
                        points.Add((x + a * n, y + b * n));
                    }
                }
                
            }            
            if(i == 2)
            {
                a = -a;
            }
        }

        //вертикаль
        a = 1; b = 2;
        for (int i = 1; i <= 4; i++, a = -a)
        {
            if (PointInBoard(x, y, a, b))
            {
                points.Add((x + a * n, y + b * n));
            }
            if (i == 2)
            {
                b = -b;
            }
        }
        Debug.Log("Knight");
        return points;
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
