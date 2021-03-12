using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Chess
{
    public override List<(int, int)> GetPointForMove(int x, int y)
    {
        List<(int, int)> points = new List<(int, int)>();

        ////относительно белых фигур
        ////вправо
        //for(int i = 1; i <= 7-x; i++)
        //{
        //    points.Add((x + i * n, y));
        //    if (FindChess(x,y,i,0) != null)
        //    {
        //        break;
        //    }
        //}

        ////влево
        //for (int i = 1; i <= x; i++)
        //{
        //    points.Add((x - i * n, y));
        //    if (FindChess(x, y, -i, 0) != null)
        //    {
        //        break;
        //    }
        //}

        ////верх
        //for (int j = 1; j <= 7 - y; j++)
        //{
        //    points.Add((x, y + j * n));            
        //    if (FindChess(x, y, 0, j) != null)
        //    {
        //        break;
        //    }
        //}

        ////вниз
        //for (int j = 1; j < y; j++)
        //{
        //    points.Add((x, y - j * n));
        //    if (FindChess(x, y, 0, -j) != null)
        //    {
        //        break;
        //    }
        //}

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

    private void GetPointInDir(List<(int,int)> points,int x, int y,int a, int b)
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
            }else
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
