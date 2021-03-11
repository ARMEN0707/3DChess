using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int n;

    public abstract List<(int, int)> GetPointForMove(int x, int y);

    public Chess FindChess(int x, int y, int i, int j) => 
        ChessBoard.chessInBoard.Find(chess => chess.currentX == (x + i * n) && chess.currentY == (y + j * n));

    public bool PointInBoard(int x, int y, int i, int j) => 
        (((x + i * n) >= 0 && (x + i * n) <= 7) && ((y + j * n) >= 0 && (y + j * n) <= 7));

    public void SetDir()
    {
        if (isWhite)
        {
            n = 1;
        }
        else
        {
            n = -1;
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

    //private void OnTriggerEntry(Collision collision)
    //{
    //    Debug.Log("asd");
    //    if(!isMove && collision.gameObject.layer == LayerMask.NameToLayer("Chess"))
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}



}
