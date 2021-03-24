using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public bool isFirstMove = true;
    public bool underAttack = false;

    public override List<Cell> GetPointForMove(int x, int y)
    {
        List<Cell> points = new List<Cell>();

        GetPoint(points,x,y,0,1);
        GetPoint(points,x,y,1,1);
        GetPoint(points,x,y,1,0);
        GetPoint(points,x,y,1,-1);
        GetPoint(points,x,y,0,-1);
        GetPoint(points,x,y,-1,-1);
        GetPoint(points,x,y,-1,0);
        GetPoint(points,x,y,-1,1);

        //рокировка
        if(isFirstMove && !underAttack)
        {
            GetPointForCastling(points, x, y, true);
            GetPointForCastling(points, x, y, false);
        }

        Debug.Log("King");
        return points;
    }

    private void GetPoint(List<Cell> points, int x, int y,int offsetX,int offsetY)
    {
        if (PointInBoard(x, y, offsetX, offsetY))
        {
            Chess chess = FindChess(x, y, offsetX, offsetY);
            if (chess == null)
            {
                Cell cell = new Cell(x + dir * offsetX, y + dir * offsetY, true);
                if(CheckCellOnAttack(cell))
                    points.Add(cell);
            }
            else
            {
                if(isWhite != chess.isWhite)
                {
                    Cell cell = new Cell(x + dir * offsetX, y + dir * offsetY, false);
                    if (CheckCellOnAttack(cell))
                        points.Add(cell);
                }
            }
        }        
    }

    private bool CheckCellOnAttack(Cell cell)
    {
        List<Cell> moves = new List<Cell>();
        foreach (Chess ch in ChessBoard.chessOnBoard)
        {
            if (ch.isWhite != isWhite && !(ch is King))
            {
                if (!(ch is Pawn))
                {
                    moves = ch.GetPointForMove(ch.currentX, ch.currentY);                    
                }else
                {
                    Pawn pawn = ch as Pawn;
                    pawn.GetPoint(moves, pawn.currentX, pawn.currentY, 1, 1);
                    pawn.GetPoint(moves, pawn.currentX, pawn.currentY, -1, 1);
                }
                if (moves.Exists(move => move.x == cell.x && move.y == cell.y))
                {
                    return false;
                }
            }
        }
        return true;
    }

    //рокировка
    private void GetPointForCastling(List<Cell> points, int x, int y, bool right)
    {
        int d;
        if(right)
        {
            d = 1;
        }else
        {
            d = -1;
        }
        if (!isWhite)
            d *= -1;

        //пересечение или ход на поле которое могут атакавать
        foreach(Chess ch in ChessBoard.chessOnBoard)
        {
            if(ch.isWhite != isWhite && !(ch is King))
            {
                List<Cell> moves = ch.GetPointForMove(ch.currentX, ch.currentY);
                if(moves.Exists(cell => 
                    ((cell.x == currentX + 1 * d * dir) || (cell.x == currentX + 2 * d * dir)) && 
                    cell.y == currentY))
                {
                    return;
                }
            }
        }

        //нет ли препядствий
        Chess chess = FindChess(x, y, 1 * d, 0);
        if (chess == null)
        {
            chess = FindChess(x, y, 2 * d, 0);
            if(chess == null)
            {
                Rock rock;
                if (right)
                {
                    rock = FindChess(x, y, 3 * d, 0) as Rock;
                }else
                {
                    rock = FindChess(x, y, 4 * d, 0) as Rock;
                }
                if(rock != null && rock.isFirstMove)
                {
                    points.Add(new Cell(x + dir * 2 * d, y + dir * 0, true));
                }
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
        //if(underAttack && ChessBoard.isMoveChess)
        //{
        //    underAttack = false;
        //}
        if (isFirstMove && isMove)
        {
            isFirstMove = false;
        }
        MoveChess(point);
    }
}
