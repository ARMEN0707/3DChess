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
                GetPointForMove(points, x, y, 0, j);                
            }
        }else
        {
            GetPointForMove(points, x, y, 0, 1);
        }

        if(isTakingOnPass)
        {
            if(FindChess(currentX,currentY,1,0) is Pawn)
            {
                points.Add(new Cell(x + 1 * dir, y + 1 * dir, false));
            }
            if (FindChess(currentX, currentY, -1, 0) is Pawn)
            {
                points.Add(new Cell(x -1 * dir, y + 1 * dir, false));
            }            
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

    private void SetTakingOfPass(Chess chess)
    {
        if(chess is Pawn && chess.isWhite != isWhite)
        {
            Pawn pawn = chess as Pawn;
            pawn.isTakingOnPass = true;
        }
    }

    private void CheckAdjacentCell()
    {
        Chess chessRight = FindChess(currentX, currentY, 1, 0);
        Chess chessLeft = FindChess(currentX, currentY, -1, 0);
        SetTakingOfPass(chessRight);
        SetTakingOfPass(chessLeft);
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
        if((isTakingOnPass && isMove) || (isTakingOnPass && !isMove && ChessBoard.isMoveChess))
        {
            isTakingOnPass = false;
        }
    }


}
