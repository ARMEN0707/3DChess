using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chess
{
    private bool isFirstMove = true;
    

    public override List<(int,int)> GetPointForMove(int x, int y)
    {
        List<(int,int)> points = new List<(int,int)>();
        
        //перемещение
        if(isFirstMove)
        {
            for(int j = 1; j <= 2; j++)
            {
                GetPointForMove(points, x, y, 0, j);                
            }
        }else
        {
            GetPointForMove(points, x, y, 0, 1);
        }

        //атака
        GetPointForAttack(points, x, y, 1, 1);
        GetPointForAttack(points, x, y, -1, 1);

        return points;
    }

    private void GetPointForMove(List<(int,int)> points,int x,int y, int i,int j)
    {
        Chess chess = FindChess(x, y, i, j);
        if (chess == null)
        {
            points.Add((x + i * n, y + j * n));
        }        
    }

    private void GetPointForAttack(List<(int, int)> points, int x, int y, int i, int j)
    {
        Chess chess = FindChess(x, y, i, j);
        if (chess != null)
        {
            if (isWhite != chess.isWhite)
            {
                points.Add((x + i * n, y + j * n));
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
        if (isFirstMove && isMove)
        {
            isFirstMove = false;
        }
        MoveChess(point);
    }


}
