using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject CellForMove;
    public Vector3 startPosition;
    public float cellSize;
    public List<GameObject> listChessPrefabs;

    [Header ("Set Dynamic")]
    public GameObject activeChess = null;
    public static bool isMoveChess = false;
    public float halfSizeBoard;
    public static List<Chess> chessInBoard = new List<Chess>();

    private RaycastHit hit;

    private void SpawnChess(int index, int x, int y)
    {
        GameObject go = Instantiate(listChessPrefabs[index], GetPosition(x, y), Quaternion.identity);
        go.transform.SetParent(transform);
        go.name = go.name.Replace("(Clone)", "");
        Chess chessScript = go.GetComponent<Chess>();
        chessScript.currentX = x;
        chessScript.currentY = y;
        chessInBoard.Add(chessScript);
    }

    private Vector3 GetPosition(int indexCellX,int indexCellY)
    {
        Vector3 position = Vector3.zero;
        position.x = halfSizeBoard + (indexCellX * cellSize) + (cellSize / 2.0f);
        position.z = halfSizeBoard + (indexCellY * cellSize) + (cellSize / 2.0f);
        return position + startPosition;
    }
    private void GetIndexCell(Vector3 point,out int indexCellX, out int indexCellY)
    {
        indexCellX = (int)((point.x - halfSizeBoard) / cellSize);
        indexCellX = Mathf.Abs(indexCellX);
        indexCellY = (int)((point.z - halfSizeBoard) / cellSize);
        indexCellY = Mathf.Abs(indexCellY);
        Debug.Log((indexCellX, indexCellY));
    }

    private void SpawnAllChess()
    {
        //White
        //Pawn
        for(int i = 0; i < 8; i++)
        {
            SpawnChess(0, i, 1);
        }
        //Rock
        SpawnChess(1, 0, 0);
        SpawnChess(1, 7, 0);
        //Knight
        SpawnChess(2, 1, 0);
        SpawnChess(2, 6, 0);
        //Bishop
        SpawnChess(3, 2, 0);
        SpawnChess(3, 5, 0);
        //Queen
        SpawnChess(4, 3, 0);
        //King
        SpawnChess(5, 4, 0);

        //Black
        //Pawn
        for (int i = 0; i < 8; i++)
        {
            SpawnChess(6, i, 6);
        }
        //Rock
        SpawnChess(7, 0, 7);
        SpawnChess(7, 7, 7);
        //Knight
        SpawnChess(8, 1, 7);
        SpawnChess(8, 6, 7);
        //Bishop
        SpawnChess(9, 2, 7);
        SpawnChess(9, 5, 7);
        //Queen
        SpawnChess(10, 3, 7);
        //King
        SpawnChess(11, 4, 7);
    }

    //если мышка находится над фигурой, то вернёт её
    private GameObject DetectionChess()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 50.0f, LayerMask.GetMask("Chess"));
        if(hit.collider != null)
        {
            return hit.collider.gameObject;
        }else
        {
            return null;
        }
    }

    //рисует на доске все возможные ходы
    private void DrawCellForMove(List<(int,int)> points)
    {
        if (points == null)
            return;
        ClearCellForMove();
        foreach((int,int) p in points)
        {
            GameObject go = Instantiate(CellForMove, GetPosition(p.Item1, p.Item2), Quaternion.identity);
            go.transform.SetParent(transform);
            go.name = go.name.Replace("(Clone)", "");
        }
    }
    private void ClearCellForMove()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("CellMove");
        foreach(GameObject i in items)
        {
            Destroy(i);
        }
    }
    

    void Start()
    {
        halfSizeBoard = -4 * cellSize;
        transform.position = startPosition;
        SpawnAllChess();
    }

    void Update()
    {
        GameObject chess = DetectionChess();
        //выбор объекта
        if(chess != null && Input.GetMouseButtonUp(0) && !isMoveChess && !PlayerCamera.isWait)
        {            
            activeChess = chess;
            Chess chessScript = activeChess.GetComponent<Chess>();
            int x, y;
            GetIndexCell(activeChess.transform.position, out x, out y);
            DrawCellForMove(chessScript.GetPointForMove(x,y));
        }

        //если есть выбранная фигура, то переместить если существует возможнсть хода
        if(chess == null && Input.GetMouseButtonUp(0) && !isMoveChess && !PlayerCamera.isWait)
        {
            if(activeChess != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray,out hit, 50.0f, LayerMask.GetMask("Board")))
                {
                    int x, y;                    
                    Chess chessScript = activeChess.GetComponent<Chess>();
                    GetIndexCell(activeChess.transform.position, out x, out y);
                    List<(int,int)> points = chessScript.GetPointForMove(x,y);
                    if (points == null)
                        return;
                    GetIndexCell(hit.point, out x, out y);                    
                    if (points.Contains((x,y)))
                    {                        
                        chessScript.point = GetPosition(x, y); ;
                        chessScript.isMove = true;
                        chessScript.currentX = x;
                        chessScript.currentY = y;
                        isMoveChess = true;
                    }                   
                }
            }
            activeChess = null;
            ClearCellForMove();
        }
    }
}
