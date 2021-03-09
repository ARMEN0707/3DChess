using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [Header ("Set in Inspector")]
    public Vector3 startPosition;
    public List<GameObject> listChessPrefabs;
    public float cellSize;

    [Header ("Set Dynamic")]
    public GameObject activeChess = null;


    private RaycastHit hit;

    private void SpawnChess(int index, Vector3 position)
    {
        GameObject go = Instantiate(listChessPrefabs[index], position, Quaternion.identity);
        go.transform.SetParent(transform);
        go.name = go.name.Replace("(Clone)", "");
    }

    private Vector3 GetPosition(int indexCellX,int indexCellY)
    {
        Vector3 position = Vector3.zero;
        position.x = (-4 * cellSize) + (indexCellX * cellSize) + (cellSize / 2.0f);
        position.z = (-4 * cellSize) + (indexCellY * cellSize) + (cellSize / 2.0f);
        return position + startPosition;
    }

    private void SpawnAllChess()
    {
        //White
        //Pawn
        for(int i = 0; i < 8; i++)
        {
            SpawnChess(0, GetPosition(i, 1));
        }
        //Rock
        SpawnChess(1, GetPosition(0, 0));
        SpawnChess(1, GetPosition(7, 0));
        //Knight
        SpawnChess(2, GetPosition(1, 0));
        SpawnChess(2, GetPosition(6, 0));
        //Bishop
        SpawnChess(3, GetPosition(2, 0));
        SpawnChess(3, GetPosition(5, 0));
        //Queen
        SpawnChess(4, GetPosition(3, 0));
        //King
        SpawnChess(5, GetPosition(4, 0));

        //Black
        //Pawn
        for (int i = 0; i < 8; i++)
        {
            SpawnChess(6, GetPosition(i, 6));
        }
        //Rock
        SpawnChess(7, GetPosition(0, 7));
        SpawnChess(7, GetPosition(7, 7));
        //Knight
        SpawnChess(8, GetPosition(1, 7));
        SpawnChess(8, GetPosition(6, 7));
        //Bishop
        SpawnChess(9, GetPosition(2, 7));
        SpawnChess(9, GetPosition(5, 7));
        //Queen
        SpawnChess(10, GetPosition(3, 7));
        //King
        SpawnChess(11, GetPosition(4, 7));
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

    void Start()
    {
        transform.position = startPosition;
        SpawnAllChess();
    }

    void Update()
    {
        GameObject chess = DetectionChess();
        //выбор объекта
        if(chess != null && Input.GetMouseButtonUp(0))
        {            
            activeChess = chess;            
        }

        //если есть выбранная фигура, то переместить если существует возможнсть хода
        if(chess == null && Input.GetMouseButtonUp(0))
        {
            if(activeChess)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray,out hit, 50.0f, LayerMask.GetMask("Board")))
                {
                    Vector3 position = Vector3.zero;
                    int x = (int)(hit.point.x / cellSize);
                    x = Mathf.Abs(x);
                    int y = (int)(hit.point.z / cellSize);
                    y = Mathf.Abs(y);
                    Debug.Log((x,y));
                    position = GetPosition(x, y);
                    Chess chessScript = activeChess.GetComponent<Chess>();
                    chessScript.GetPointForMove();
                }
            }
            activeChess = null;
        }
    }
}
