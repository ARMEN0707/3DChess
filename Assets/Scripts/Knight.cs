using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chess
{
    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();

        //горизонталь
        int offsetX = 2 , offsetY = 1;
        Chess chess;
        for (int i = 1; i <= 4; i++,offsetY = -offsetY)
        {            
            if(PointInBoard(x,y,offsetX,offsetY))
            {
                chess = FindChess(x, y, offsetX, offsetY);
                if(chess == null)
                {
                    points.Add(new Cell(x + offsetX * dir, y + offsetY * dir,true));
                }else
                {
                    if(isWhite != chess.isWhite)
                    {
                        points.Add(new Cell(x + offsetX * dir, y + offsetY * dir, false));
                    }
                }
                
            }            
            if(i == 2)
            {
                offsetX = -offsetX;
            }
        }

        //вертикаль
        offsetX = 1; offsetY = 2;
        for (int i = 1; i <= 4; i++, offsetX = -offsetX)
        {
            if (PointInBoard(x, y, offsetX, offsetY))
            {
                chess = FindChess(x, y, offsetX, offsetY);
                if (chess == null)
                {
                    points.Add(new Cell(x + offsetX * dir, y + offsetY * dir, true));
                }
                else
                {
                    if (isWhite != chess.isWhite)
                    {
                        points.Add(new Cell(x + offsetX * dir, y + offsetY * dir, false));
                    }
                }

            }
            if (i == 2)
            {
                offsetY = -offsetY;
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
