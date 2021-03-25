using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chess : MonoBehaviour
{
    [Header ("Set in inspector")]
    public float speed;   
    public bool isWhite;

    [Header("Set Dynamic")]
    public bool isMove;
    public Vector3 point;
    public int currentX;
    public int currentY;
    //для направления движения
    public int dir;

    //получить все возможные ходы
    public abstract List<Cell> GetPointForMove(int x, int y);
    //получить точки хода которые заняты своими шахматами
    public abstract List<Cell> GetOccupiedPointForMove();

    //проверяет наличие фигуры на данной клетке
    public Chess FindChess(int x, int y, int offsetX, int offsetY) => 
        ChessBoard.chessOnBoard.Find(chess => chess.currentX == (x + offsetX * dir) && chess.currentY == (y + offsetY * dir));

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

    //ход назад
    public void MoveBack(Vector3 point,int x, int y)
    {
        transform.position = point;
        currentX = x;
        currentY = y;
    }

    //перемещение шахмат
    public bool MoveChess(Vector3 position)
    {
        if (isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            if(transform.position == position)
            {
                isMove = false;
                ChessBoard.isMoveChess = false;
                return true;
            }
        }
        return false;
    }
}
