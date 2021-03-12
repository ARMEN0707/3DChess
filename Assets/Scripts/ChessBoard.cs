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
    public AnimationCurve curve;
    public GameObject PlaceDeadChessBlack;
    public GameObject PlaceDeadChessWhite;

    [Header ("Set Dynamic")]
    public GameObject activeChess = null;
    public static bool isMoveChess = false;
    public float halfSizeBoard;
    public static List<Chess> chessInBoard = new List<Chess>();
    public bool isWhitePlayer = true;
    public int numberInactiveBlack = 0;
    public int numberInactiveWhite = 0;

    private RaycastHit hit;

    private void SpawnChess(int index, int x, int y)
    {
        GameObject go = listChessPrefabs[index];
        go = Instantiate(go, GetPosition(x, y),go.transform.rotation);
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

    public IEnumerator ClearChess(GameObject go)
    {
        float currentTime = 0;
        float timeCurve = curve[curve.keys.Length - 1].time;
        Vector3 endPoint;
        if(isWhitePlayer)
        {
            endPoint = PlaceDeadChessBlack.transform.position;
        }else
        {
            endPoint = PlaceDeadChessWhite.transform.position;
        }
        while (true)
        {
            currentTime += Time.deltaTime;
            Vector3 pos = Vector3.MoveTowards(go.transform.position, endPoint, 15.0f * Time.deltaTime);            
            if (currentTime < timeCurve)
            {                
                float y = curve.Evaluate(currentTime);
                pos.y = y * 13.0f;
            }
            if (go.transform.position == endPoint && currentTime > timeCurve)
            {
                //так как значение переменной isWhitePlayer изменяется сразу после вызова корутины
                //то значения которые тут используются пртивоположны
                if(!isWhitePlayer)
                {
                    if (numberInactiveBlack == 7)
                    {
                        endPoint.x += cellSize;
                        endPoint.z -= cellSize * 7;
                        numberInactiveBlack = 1;
                    }
                    else
                    {
                        endPoint.z += cellSize;
                        numberInactiveBlack++;
                    }
                    PlaceDeadChessBlack.transform.position = endPoint;
                }
                else
                {
                    if (numberInactiveWhite == 7)
                    {
                        endPoint.x -= cellSize;
                        endPoint.z += cellSize * 7;
                        numberInactiveWhite = 1;
                    }
                    else
                    {
                        endPoint.z -= cellSize;
                        numberInactiveWhite++;
                    }
                    PlaceDeadChessWhite.transform.position = endPoint;
                }
                break;
            }
            go.transform.position = pos;
            yield return null;
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
            Chess chessScript = chess.GetComponent<Chess>();
            if(isWhitePlayer == chessScript.isWhite)
            {
                int x, y;
                GetIndexCell(chess.transform.position, out x, out y);
                DrawCellForMove(chessScript.GetPointForMove(x, y));
                activeChess = chess;
            }
        }

        //если есть выбранная фигура, то переместить если существует возможнсть хода
        if(activeChess != null && Input.GetMouseButtonUp(0) && !isMoveChess && !PlayerCamera.isWait)
        {           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit, 50.0f))
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
                    if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Chess"))
                    {
                        //Destroy(hit.collider.gameObject,1.0f); 
                        Chess chessDead = hit.collider.gameObject.GetComponent<Chess>();
                        chessInBoard.Remove(chessDead);
                        hit.collider.enabled = false;
                        StartCoroutine(ClearChess(hit.collider.gameObject));
                    }
                    chessScript.point = GetPosition(x, y); ;
                    chessScript.isMove = true;
                    chessScript.currentX = x;
                    chessScript.currentY = y;
                    isMoveChess = true;
                    isWhitePlayer = !isWhitePlayer;
                    activeChess = null;
                    ClearCellForMove();
                }                    
            }           
        }
    }
}
