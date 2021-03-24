using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chess
{
    public bool isTakingOnPass = false;
    public bool isFirstMove = true;    

    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();
        
        //перемещение
        if(isFirstMove)
        {
            for(int j = 1; j <= 2; j++)
            {
                GetPoint(points, x, y, 0, j);                
            }
        }else
        {
            GetPoint(points, x, y, 0, 1);
        }

        //взятие на проходе
        Pawn pawn = FindChess(currentX, currentY, 1, 0) as Pawn;
        if(pawn != null && pawn.isTakingOnPass)
        {
            points.Add(new Cell(x + 1 * dir, y + 1 * dir, false));
        }
        pawn = FindChess(currentX, currentY, -1, 0) as Pawn;
        if (pawn != null && pawn.isTakingOnPass)
        {
            points.Add(new Cell(x - 1 * dir, y + 1 * dir, false));
        }

        //атака
        GetPointForAttack(points, x, y, 1, 1);
        GetPointForAttack(points, x, y, -1, 1);

        return points;
    }

    public void GetPoint(List<Cell> points,int x,int y, int offsetX,int offsetY)
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

    //обнаружение соседних пешок 
    private void CheckAdjacentCell()
    {
        int y;
        if(isWhite)
        {
            y = 3;
        }else
        {
            y = 4;
        }
        Pawn chessRight = FindChess(currentX, y, 1, 0) as Pawn;
        Pawn chessLeft = FindChess(currentX, y, -1, 0) as Pawn;
        if((chessRight != null || chessLeft != null))
        {
            isTakingOnPass = true;
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

        if (MoveChess(point) && isFirstMove)
        {
            CheckAdjacentCell();
            isFirstMove = false;
        }
        if(isTakingOnPass && !isMove && ChessBoard.isMoveChess)
        {
            isTakingOnPass = false;
        }
    }


}
