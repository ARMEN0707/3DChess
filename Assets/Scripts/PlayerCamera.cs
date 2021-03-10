using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header ("Set in inspector")]
    public GameObject whitePlayerCamera;
    public GameObject blackPlayerCamera;

    [Header("Set Dynamic")]
    public static bool isWait = false;

    // Start is called before the first frame update
    void Start()
    {
        whitePlayerCamera.SetActive(true);
        blackPlayerCamera.SetActive(false);
    }

    private void SwapCamera()
    {
        whitePlayerCamera.SetActive(!whitePlayerCamera.activeInHierarchy);
        blackPlayerCamera.SetActive(!blackPlayerCamera.activeInHierarchy);
    }

    private IEnumerator Wait()
    {
        while(true)
        {
            if(ChessBoard.isMoveChess)
            {
                yield return null;
            }else
            {
                break;
            }             
        }
        yield return new WaitForSeconds(1.0f);
        SwapCamera();
        isWait = false;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(ChessBoard.isMoveChess && !isWait)
        {
            isWait = true;
            StartCoroutine(Wait());

        }
    }

}
