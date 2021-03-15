using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public int x;
    public int y;
    public bool isCellForMove;

    public Cell(int x,int y,bool isCellForMove)
    {
        this.x = x;
        this.y = y;
        this.isCellForMove = isCellForMove;
    }
}

public abstract class Chess : MonoBehaviour
{
    [Header ("Set in inspector")]
    public float speed = 1.0f;   
    public bool isWhite;


    [Header("Set Dynamic")]
    public bool isMove;
    public Vector3 point;
    public int currentX;
    public int currentY;
    //для направления движения
    public int dir;

    public abstract List<Cell> GetPointForMove(int x, int y);

    //проверяет наличие фигуры на данной клетке
    public Chess FindChess(int x, int y, int offsetX, int offsetY) => 
        ChessBoard.chessInBoard.Find(chess => chess.currentX == (x + offsetX * dir) && chess.currentY == (y + offsetY * dir));

    //находится ли точка на доске
    public bool PointInBoard(int x, int y, int offsetX, int offsetY) => 
        (((x + offsetX * dir) >= 0 && (x + offsetX * dir) <= 7) && ((y + offsetY * dir) >= 0 && (y + offsetY * dir) <= 7));

    public void SetDir()
    {
        if (isWhite)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
    }

    public void MoveChess(Vector3 position)
    {
        if (isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            if(transform.position == position)
            {
                isMove = false;
                ChessBoard.isMoveChess = false;
            }
        }
    }
}
