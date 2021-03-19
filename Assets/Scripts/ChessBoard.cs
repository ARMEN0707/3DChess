using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChessMove
{
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public bool isAttack;
}

public struct Cell
{
    public int x;
    public int y;
    public bool isCellForMove;

    public Cell(int x, int y, bool isCellForMove)
    {
        this.x = x;
        this.y = y;
        this.isCellForMove = isCellForMove;
    }
}


public class ChessBoard : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject CellForMove; //
    public GameObject CellForAttack;// ячейки для ходов
    public GameObject SelectedCell;//
    public Vector3 startPosition; // позиция доски
    public float cellSize; // размер одной клетки доски
    public List<GameObject> listChessPrefabs; 
    public AnimationCurve curve;
    public GameObject PlaceDeadChessBlack;// позиция для уничтоженных шахмат
    public GameObject PlaceDeadChessWhite;//
    public AudioSource soundChess;
    public UIManager uiManager;

    [Header ("Set Dynamic")]
    public GameObject activeChess = null;
    public static bool isMoveChess;
    public float halfSizeBoard;
    public static List<Chess> chessOnBoard; // шахматы которые находятся на доске
    public bool isWhitePlayer = true;
    public int numberInactiveBlack = 0;
    public int numberInactiveWhite = 0;
    public Stack<ChessMove> listChessMove; // список всех ходом 
    public Stack<Chess> listInactiveChess; // список уничтоженных шахмат
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
        chessOnBoard.Add(chessScript);
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
        //Debug.Log((indexCellX, indexCellY));
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
    private void DrawCellForMove(List<Cell> points)
    {
        if (points == null)
            return;
        GameObject go;
        ClearCell();
        foreach (Cell p in points)
        {
            if (p.isCellForMove)
            {
                go = Instantiate(CellForMove, GetPosition(p.x, p.y), Quaternion.identity);
            }else
            {
                go = Instantiate(CellForAttack, GetPosition(p.x, p.y), Quaternion.identity);
            }
            go.transform.SetParent(transform);
            go.name = go.name.Replace("(Clone)", "");
        }
    }
    private void DrawSelectedCell(int x,int y)
    {
        GameObject go = Instantiate(SelectedCell, GetPosition(x, y), Quaternion.identity);
        go.transform.SetParent(transform);
        go.name = go.name.Replace("(Clone)", "");
    }
    private void ClearCell()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Cell");
        foreach(GameObject i in items)
        {
            Destroy(i);
        }
    }

    //задает координаты движения шахматам
    private void SetPointMoveForChess(Chess chessScript,int x, int y)
    {
        chessScript.point = GetPosition(x, y);
        chessScript.currentX = x;
        chessScript.currentY = y;
        chessScript.isMove = true;
        isMoveChess = true;
    }

    //проверяет находится ли пешка на 8 или 1 линии
    private bool CheckLastLine(Chess chessScript,int y)
    {
        if(chessScript.isWhite && y == 7)
        {
            return true;
        }
        if(!chessScript.isWhite && y == 0)
        {
            return true;
        }
        return false;
    }

    //перемещение шахматы за тереторию доски
    private IEnumerator ClearChess(GameObject go)
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

    //ожидание конца движения шахмат
    private IEnumerator EndMoveChess(bool isLastLine)
    {
        while (true)
        {
            if (isMoveChess)
            {
                yield return null;
            }
            else
            {
                soundChess.Play();
                if (activeChess.tag == "Pawn" && isLastLine)
                {
                    uiManager.ReplaceChessMenu();
                }
                else
                {
                    activeChess = null;
                }
                yield break;
            }
        }
    }

    //заменяет пешку на другую шахмату
    public void ReplacePawn(string name)
    {
        Chess chessScript = activeChess.GetComponent<Chess>();     
        string color = "";
        if(!chessScript.isWhite)
        {
            color = "Black";
        }
        GameObject prefab = listChessPrefabs.Find(chess => chess.name == (name + color));
        Destroy(activeChess);
        GameObject go = Instantiate(prefab, GetPosition(chessScript.currentX, chessScript.currentY), Quaternion.identity);
        go.transform.SetParent(transform);
        go.name = go.name.Replace("(Clone)", "");
        Chess newChessScript = go.GetComponent<Chess>();
        newChessScript.currentX = chessScript.currentX;
        newChessScript.currentY = chessScript.currentY;
        uiManager.ReplaceChessMenu();
        chessOnBoard.Remove(chessScript);
        chessOnBoard.Add(newChessScript);
        activeChess = null;
    }

    //отменяет два предыдущих хода
    public void BackStep()
    {
        if (!isMoveChess && !PlayerCamera.isWait)
        {
            for (int i = 0; i < 2; i++)
            {
                if (listChessMove.Count != 0)
                {
                    ChessMove chessMove = listChessMove.Pop();
                    Chess chessScript = chessOnBoard.Find(chess => chess.currentX == chessMove.endX && chess.currentY == chessMove.endY);
                    Vector3 position = GetPosition(chessMove.startX, chessMove.startY);
                    chessScript.MoveBack(position, chessMove.startX, chessMove.startY);
                    if (chessScript.tag == "Pawn")
                    {
                        Pawn pawnScript = chessScript as Pawn;
                        if ((pawnScript.isWhite && pawnScript.currentY == 1) || (!pawnScript.isWhite && pawnScript.currentY == 6))
                        {
                            pawnScript.isFirstMove = true;
                        }
                    }
                    if (chessMove.isAttack)
                    {
                        Chess chessInactive = listInactiveChess.Pop();
                        chessInactive.gameObject.GetComponent<BoxCollider>().enabled = true;
                        Vector3 pos = GetPosition(chessInactive.currentX, chessInactive.currentY);
                        chessInactive.MoveBack(pos, chessInactive.currentX, chessInactive.currentY);
                        chessOnBoard.Add(chessInactive);
                    }
                }
                else
                {
                    isWhitePlayer = true;
                    GameObject.Find("CameraManager").GetComponent<PlayerCamera>().SwapCamera();
                }
            }
        }
    }

    void Start()
    {
        isMoveChess = false;
        chessOnBoard = new List<Chess>();
        listChessMove = new Stack<ChessMove>();
        listInactiveChess = new Stack<Chess>();
        halfSizeBoard = -4 * cellSize;
        transform.position = startPosition;
        SpawnAllChess();
    }

    void Update()
    {     
        //если есть выбранная фигура, то переместить если существует возможнсть хода
        if(activeChess != null && Input.GetMouseButtonUp(0) && !isMoveChess && !PlayerCamera.isWait)
        {           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //если во что-то попали 
            if(Physics.Raycast(ray,out hit, 50.0f))
            {                
                ChessMove chessMove = new ChessMove();
                Chess chessScript = activeChess.GetComponent<Chess>();
                GameObject goHit = hit.collider.gameObject;
                //GetIndexCell(activeChess.transform.position, out x, out y);
                int x = chessScript.currentX;
                int y = chessScript.currentY;
                chessMove.startX = x;
                chessMove.startY = y;
                List<Cell> points = chessScript.GetPointForMove(x,y);
                if (points == null)
                    return;
                GetIndexCell(hit.point, out x, out y);               
                //если попали в точку куда возможен ход
                if (points.Exists(p => p.x == x && p.y == y))
                { 
                    //если на данной клетке есть чужая шахмата
                    if(goHit.layer == LayerMask.NameToLayer("Chess"))
                    {
                        //Destroy(hit.collider.gameObject,1.0f); 
                        Chess chessDead = goHit.GetComponent<Chess>();
                        listInactiveChess.Push(chessDead);
                        chessOnBoard.Remove(chessDead);
                        hit.collider.enabled = false;
                        if(goHit.tag == "King")
                        {
                            uiManager.WinMenu(chessScript.isWhite);
                            Debug.Log("End");
                        }
                        chessMove.isAttack = true;
                        StartCoroutine(ClearChess(goHit));
                    }
                    chessMove.endX = x;
                    chessMove.endY = y;
                    SetPointMoveForChess(chessScript, x, y);
                    StartCoroutine(EndMoveChess(CheckLastLine(chessScript, y)));
                    //взятие на проходе
                    if (chessScript is Pawn && points.Exists(p => p.x == x && p.y == y && p.isCellForMove == false))
                    {
                        Pawn pawn = chessScript as Pawn;
                        if (pawn.isTakingOnPass)
                        {
                            Chess adjacentChess = chessScript.FindChess(x, y, 0, -1);
                            listInactiveChess.Push(adjacentChess);
                            chessOnBoard.Remove(adjacentChess);
                            adjacentChess.gameObject.GetComponent<BoxCollider>().enabled = false;
                            chessMove.isAttack = true;
                            StartCoroutine(ClearChess(adjacentChess.gameObject));
                        }                        
                    }
                    isWhitePlayer = !isWhitePlayer;
                    listChessMove.Push(chessMove);
                    ClearCell();
                }
            }           
        }
        GameObject chess = DetectionChess();
        //выбор объекта
        if (chess != null && Input.GetMouseButtonUp(0) && !isMoveChess && !PlayerCamera.isWait && !UIManager.isPause)
        {
            Chess chessScript = chess.GetComponent<Chess>();
            if (isWhitePlayer == chessScript.isWhite)
            {
                int x, y;
                GetIndexCell(chess.transform.position, out x, out y);
                DrawCellForMove(chessScript.GetPointForMove(x, y));
                DrawSelectedCell(x, y);
                activeChess = chess;
            }
        }
    }
}
