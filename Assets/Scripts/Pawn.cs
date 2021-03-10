using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chess
{
    private bool isFirstMove = true;
    

    public override List<(int,int)> GetPointForMove(int x, int y)
    {
        List<(int,int)> points = new List<(int,int)>();
        if(isFirstMove)
        {
            for(int i = 1; i <= 2; i++)
            {
                if (ChessBoard.chessInBoard.Find(chess => chess.currentX == x && chess.currentY == (y + i * n)) == null)
                {
                    points.Add((x, y + i * n));
                }else
                {
                    break;
                }                
            }
        }else
        {
            if (ChessBoard.chessInBoard.Find(chess => chess.currentX == x && chess.currentY == (y + 1 * n)) == null)
            {
                points.Add((x, y + 1 * n));
            }
            if(ChessBoard.chessInBoard.Find(chess => chess.currentX == (x + 1 * n) && chess.currentY == (y + 1 * n)) != null)
            {
                points.Add((x + 1 * n, y + 1 * n));
            }
            if (ChessBoard.chessInBoard.Find(chess => chess.currentX == (x - 1 * n) && chess.currentY == (y + 1 * n)) != null)
            {
                points.Add((x - 1 * n, y + 1 * n));
            }
        }

        return points;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isWhite)
        {
            n = 1;
        }else
        {
            n = -1;
        }
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
