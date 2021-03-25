using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Chess
{
    public bool isFirstMove = true;

    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();

        //вправо
        GetPointInDir(points, x, y, 1, 0);
        //влево
        GetPointInDir(points, x, y, -1, 0);
        //верх
        GetPointInDir(points, x, y, 0, 1);
        //вниз
        GetPointInDir(points, x, y, 0, -1);

        Debug.Log("Rock");
        return points;
    }

    public override List<Cell> GetOccupiedPointForMove()
    {
        List<Cell> points = new List<Cell>();

        //вправо
        GetOccupiedPointInDir(points, 1, 0);
        //влево
        GetOccupiedPointInDir(points, -1, 0);
        //верх
        GetOccupiedPointInDir(points, 0, 1);
        //вниз
        GetOccupiedPointInDir(points, 0, -1);

        return points;
    }

    private void GetPointInDir(List<Cell> points,int x, int y,int offsetX, int offsetY)
    {
        for (int i = 1; i <= 8; i++)
        {
            if (PointInBoard(x, y, i * offsetX, i * offsetY))
            {
                Chess chess = FindChess(x, y, i * offsetX, i * offsetY);
                if (chess == null)
                {
                    points.Add(new Cell(x + i * dir * offsetX, y + i * dir * offsetY, true));
                }
                else
                {
                    if (isWhite != chess.isWhite)
                    {
                        points.Add(new Cell(x + i * dir * offsetX, y + i * dir * offsetY, false));
                    }
                    break;
                }
            }else
            {
                break;
            }
        }
    }

    private void GetOccupiedPointInDir(List<Cell> points, int offsetX, int offsetY)
    {
        for (int i = 1; i <= 8; i++)
        {
            if (PointInBoard(currentX, currentY, i * offsetX, i * offsetY))
            {
                Chess chess = FindChess(currentX, currentY, i * offsetX, i * offsetY);
                if (chess != null)
                {                   
                    if (isWhite == chess.isWhite)
                    {
                        points.Add(new Cell(currentX + i * dir * offsetX, currentY + i * dir * offsetY, false));
                    }
                    break;
                }
            }
            else
            {
                break;
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
        if(isFirstMove && isMove)
        {
            isFirstMove = false;
        }
        MoveChess(point);
    }
}
