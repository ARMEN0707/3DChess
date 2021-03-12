using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chess
{

    public override List<(int, int)> GetPointForMove(int x, int y)
    {
        List<(int, int)> points = new List<(int, int)>();

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

    private void GetPointInDir(List<(int, int)> points, int x, int y, int a, int b)
    {
        for (int i = 1; i <= 8; i++)
        {
            if (PointInBoard(x, y, i * a, i * b))
            {
                Chess chess = FindChess(x, y, i * a, i * b);
                if (chess == null)
                {
                    points.Add((x + i * n * a, y + i * n * b));
                }
                else
                {
                    if (isWhite != chess.isWhite)
                    {
                        points.Add((x + i * n * a, y + i * n * b));
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

    private void GetPointInDirDiagonal(List<(int, int)> points, int x, int y, int a, int b)
    {
        for (int i = 1; ; i++)
        {
            if (PointInBoard(x, y, i * a, i * b))
            {                
                Chess chess = FindChess(x, y, i * a, i * b);
                if (chess == null)
                {
                    points.Add((x + i * n * a, y + i * n * b));
                }else
                {
                    if(isWhite != chess.isWhite)
                    {
                        points.Add((x + i * n * a, y + i * n * b));
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
