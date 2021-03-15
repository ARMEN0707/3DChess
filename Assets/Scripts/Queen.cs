using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chess
{

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

        //право вверх
        GetPointInDirDiagonal(points, x, y, 1, 1);
        //право вниз
        GetPointInDirDiagonal(points, x, y, 1, -1);
        //лево вверх
        GetPointInDirDiagonal(points, x, y, -1, 1);
        //лево вниз
        GetPointInDirDiagonal(points, x, y, -1, -1);

        Debug.Log("Queen");
        return points;
    }

    private void GetPointInDir(List<Cell> points, int x, int y, int offsetX, int offsetY)
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
            }
            else
            {
                break;
            }
        }
    }

    private void GetPointInDirDiagonal(List<Cell> points, int x, int y, int offsetX, int offsetY)
    {
        for (int i = 1; ; i++)
        {
            if (PointInBoard(x, y, i * offsetX, i * offsetY))
            {                
                Chess chess = FindChess(x, y, i * offsetX, i * offsetY);
                if (chess == null)
                {
                    points.Add(new Cell(x + i * dir * offsetX, y + i * dir * offsetY, true));
                }else
                {
                    if(isWhite != chess.isWhite)
                    {
                        points.Add(new Cell(x + i * dir * offsetX, y + i * dir * offsetY, false));
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
        MoveChess(point);
    }
}
