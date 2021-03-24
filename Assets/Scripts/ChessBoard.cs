using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum typeMove
{
    move,
    attack,
    taking,
    castling
}
//шахматный ход
public struct ChessMove
{
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public typeMove type;
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
    public List<GameObject> listChessPrefabs; //префабы фигур
    public AnimationCurve curve;
    public GameObject PlaceDeadChessBlack;// позиция для уничтоженных шахмат
    public GameObject PlaceDeadChessWhite;//
    public AudioSource soundChess;
    public UIManager uiManager;

    [Header("Set Dynamic")]
    public bool checkmate = false;
    public bool pat = false;
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
        ////White
        ////Pawn
        //for (int i = 0; i < 8; i++)
        //{
        //    SpawnChess(0, i, 1);
        //}
        ////Rock
        //SpawnChess(1, 0, 0);
        //SpawnChess(1, 7, 0);
        ////Knight
        //SpawnChess(2, 1, 0);
        //SpawnChess(2, 6, 0);
        ////Bishop
        //SpawnChess(3, 2, 0);
        //SpawnChess(3, 5, 0);
        ////Queen
        //SpawnChess(4, 3, 0);
        //King
        SpawnChess(5, 4, 0);

        ////Black
        ////Pawn
        //for (int i = 0; i < 8; i++)
        //{
        //    SpawnChess(6, i, 6);
        //}
        ////Rock
        //SpawnChess(7, 0, 7);
        //SpawnChess(7, 7, 7);
        ////Knight
        //SpawnChess(8, 1, 7);
        //SpawnChess(8, 6, 7);
        ////Bishop
        //SpawnChess(9, 2, 7);
        //SpawnChess(9, 5, 7);
        ////Queen
        //SpawnChess(10, 3, 7);
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

    //является ли король под атакой
    private bool CheckKing()
    {       
        foreach (Chess ch in chessOnBoard)
        {
            if(!(ch is King))
            {
                List<Cell> moves = ch.GetPointForMove(ch.currentX, ch.currentY);
                King king = chessOnBoard.Find(k => k.tag == "King" && k.isWhite != ch.isWhite) as King;
                if (moves.Exists(move => move.x == king.currentX && move.y == king.currentY && !move.isCellForMove))
                {
                    if (activeChess.tag == "King" || king.underAttack)
                    {
                        checkmate = true;
                        isWhitePlayer = !king.isWhite;
                    }
                    king.underAttack = true;
                    return true;
                }
            }
        }
        return false;
    }
    //проверка конца игры
    private void CheckEndGame()
    {
        King[] arrayKing = FindObjectsOfType<King>();
        if (!CheckKing())
        {
            foreach (King k in arrayKing)
            {
                k.occupiedCell = false;
                k.underAttack = false;
            }
        }       
                  
        foreach (King k in arrayKing)
        {
            List<Cell> moves = k.GetPointForMove(k.currentX, k.currentY);
            //вычитаем одинаковые ходы
            k.Except(moves);
            //если у короля нет хода и он под шахом
            if (moves.Count == 0 && k.underAttack == true)
            {
                checkmate = true;
                isWhitePlayer = !k.isWhite;
                return;
            }                    
            if(moves.Count == 0 && k.occupiedCell)
            {
                foreach(Chess chess in chessOnBoard)
                {
                    if(chess.isWhite == k.isWhite && !(chess is King))
                    {
                        if(chess.GetPointForMove(chess.currentX, chess.currentY).Count != 0)
                        {
                            return;
                        }
                    }
                }
                pat = true;
            }
        }
        if (chessOnBoard.Count == 2 && chessOnBoard[0] is King && chessOnBoard[1] is King)
        {
            pat = true;
        }
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
            endPoint.z += cellSize * (numberInactiveBlack % 8);
            if (numberInactiveBlack > 7)
                endPoint.x += cellSize;
        }else
        {
            endPoint = PlaceDeadChessWhite.transform.position;
            endPoint.z -= cellSize * (numberInactiveWhite % 8);
            if (numberInactiveWhite > 7)
                endPoint.x -= cellSize;
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
                    numberInactiveBlack++;
                }
                else
                {
                    numberInactiveWhite++;
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
                    CheckEndGame();
                    if (checkmate)
                    {
                        uiManager.WinMenu(isWhitePlayer);
                    }
                    if(pat)
                    {
                        uiManager.DrawMenu();
                    }
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
                    if(chessScript is Rock)
                    {
                        Rock rock = chessScript as Rock;
                        rock.isFirstMove = true;
                    }
                    if(chessScript is King)
                    {
                        King king = chessScript as King;
                        king.isFirstMove = true;
                    }
                    if (chessScript is Pawn)
                    {
                        Pawn pawnScript = chessScript as Pawn;
                        if ((pawnScript.isWhite && pawnScript.currentY == 1) || (!pawnScript.isWhite && pawnScript.currentY == 6))
                        {
                            pawnScript.isFirstMove = true;
                        }
                    }
                    if (chessMove.type == typeMove.attack)
                    {
                        Chess chessInactive = listInactiveChess.Pop();
                        if(chessInactive.isWhite)
                        {
                            numberInactiveWhite--;
                        }else
                        {
                            numberInactiveBlack--;
                        }
                        chessInactive.gameObject.GetComponent<BoxCollider>().enabled = true;
                        Vector3 pos = GetPosition(chessInactive.currentX, chessInactive.currentY);
                        chessInactive.MoveBack(pos, chessInactive.currentX, chessInactive.currentY);
                        chessOnBoard.Add(chessInactive);
                    }
                    if(chessMove.type == typeMove.castling)
                    {
                        chessMove = listChessMove.Pop();
                        Rock rockScript = chessOnBoard.Find(chess => chess.currentX == chessMove.endX && chess.currentY == chessMove.endY) as Rock;
                        position = GetPosition(chessMove.startX, chessMove.startY);
                        rockScript.MoveBack(position, chessMove.startX, chessMove.startY);
                        rockScript.isFirstMove = true;
                        King king = chessScript as King;
                        king.isFirstMove = true;
                    }
                }
                else
                {
                    isWhitePlayer = true;
                    GameObject.Find("CameraManager").GetComponent<PlayerCamera>().SwapCamera();
                }
            }
            CheckEndGame();
            ClearCell();
        }
    }

    //рокировка
    private void Castling(Chess chessScript)
    {
        Rock rockRight = chessOnBoard.Find(chess => 
            chess.currentX == chessScript.currentX + 1 && chess.currentY == chessScript.currentY) as Rock;
        Rock rockLeft = chessOnBoard.Find(chess =>
            chess.currentX == chessScript.currentX - 2 && chess.currentY == chessScript.currentY) as Rock;
        ChessMove move = new ChessMove();

        if (rockRight != null)
        {
            move.startX = rockRight.currentX;
            move.startY = rockRight.currentY;
            move.endX = rockRight.currentX - 2;
            move.endY = rockRight.currentY;
            SetPointMoveForChess(rockRight, rockRight.currentX - 2, rockRight.currentY);
            
        }
        if (rockLeft != null)
        {
            move.startX = rockRight.currentX;
            move.startY = rockRight.currentY;
            move.endX = rockRight.currentX + 3;
            move.endY = rockRight.currentY;
            SetPointMoveForChess(rockLeft, rockLeft.currentX + 3, rockLeft.currentY);
        }
        move.type = typeMove.move;
        listChessMove.Push(move);
    }

    void Start()
    {
        checkmate = false;
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
                chessMove.type = typeMove.move;
                Chess chessScript = activeChess.GetComponent<Chess>();
                GameObject goHit = hit.collider.gameObject;
                int x = chessScript.currentX;
                int y = chessScript.currentY;
                chessMove.startX = x;
                chessMove.startY = y;
                List<Cell> points = chessScript.GetPointForMove(x,y);
                if (chessScript is King)
                {
                    King king = chessScript as King;
                    points = king.Except(points);
                }
                if (points == null)
                    return;
                GetIndexCell(hit.point, out x, out y);               
                //если попали в точку куда возможен ход
                if (points.Exists(p => p.x == x && p.y == y))
                {                     
                    //если на данной клетке есть чужая шахмата
                    if(goHit.layer == LayerMask.NameToLayer("Chess"))
                    {
                        Chess chessDead = goHit.GetComponent<Chess>();
                        listInactiveChess.Push(chessDead);
                        chessOnBoard.Remove(chessDead);
                        hit.collider.enabled = false;
                        chessMove.type =  typeMove.attack;
                        StartCoroutine(ClearChess(goHit));
                    }
                    chessMove.endX = x;
                    chessMove.endY = y;
                    SetPointMoveForChess(chessScript, x, y);
                    StartCoroutine(EndMoveChess(CheckLastLine(chessScript, y)));
                    //взятие на проходе
                    if (chessScript is Pawn && points.Exists(p => p.x == x && p.y == y && p.isCellForMove == false))
                    {
                        Pawn adjacentChess = chessScript.FindChess(x, y, 0, -1) as Pawn;
                        if(adjacentChess != null && adjacentChess.isTakingOnPass)
                        {
                            listInactiveChess.Push(adjacentChess);
                            chessOnBoard.Remove(adjacentChess);
                            adjacentChess.gameObject.GetComponent<BoxCollider>().enabled = false;
                            chessMove.type = typeMove.attack;
                            StartCoroutine(ClearChess(adjacentChess.gameObject));
                        }                      
                    }
                    //рокировка
                    if (chessScript.tag == "King")
                    {
                        if (Mathf.Abs((chessMove.startX - chessMove.endX) / 2) == 1)
                        {                            
                            Castling(chessScript);
                            chessMove.type = typeMove.castling;
                        }
                    }
                    listChessMove.Push(chessMove);
                    isWhitePlayer = !isWhitePlayer;
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
                List<Cell> points = chessScript.GetPointForMove(x, y);
                if(chessScript is King)
                {
                    King king = chessScript as King;
                    points = king.Except(points);
                }
                DrawCellForMove(points);
                DrawSelectedCell(x, y);
                activeChess = chess;
            }
        }
    }
}
