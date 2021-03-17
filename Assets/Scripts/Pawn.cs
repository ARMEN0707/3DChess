using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chess
{
    public bool isFirstMove = true;    

    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();
        
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

    private void GetPointForMove(List<Cell> points,int x,int y, int offsetX,int offsetY)
    {
        Chess chess = FindChess(x, y, offsetX, offsetY);
        if (chess == null)
        {
            if (PointInBoard(x, y, offsetX, offsetY))
            {
                points.Add(new Cell(x + offsetX * dir, y + offsetY * dir, true));
            }
        }        
    }

    private void GetPointForAttack(List<Cell> points, int x, int y, int offsetX, int offsetY)
    {
        Chess chess = FindChess(x, y, offsetX, offsetY);
        if (chess != null)
        {
            if (isWhite != chess.isWhite)
            {
                points.Add(new Cell(x + offsetX * dir, y + offsetY * dir, false));
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
