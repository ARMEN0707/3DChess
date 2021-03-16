using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header ("Set in inspector")]
    public GameObject whitePlayerCamera;
    public GameObject blackPlayerCamera;
    public Canvas canvas;

    [Header("Set Dynamic")]
    public static bool isWait;
    private Vector3 startRotationWhitePlayerCamera;
    private Vector3 startRotationBlackPlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        isWait = false;
        startRotationWhitePlayerCamera = whitePlayerCamera.transform.localEulerAngles;
        startRotationBlackPlayerCamera = blackPlayerCamera.transform.localEulerAngles;
        whitePlayerCamera.SetActive(true);
        blackPlayerCamera.SetActive(false);
    }

    private void SwapCamera()
    {
        whitePlayerCamera.SetActive(!whitePlayerCamera.activeInHierarchy);
        blackPlayerCamera.SetActive(!blackPlayerCamera.activeInHierarchy);
        whitePlayerCamera.transform.localEulerAngles = startRotationWhitePlayerCamera;
        blackPlayerCamera.transform.localEulerAngles = startRotationBlackPlayerCamera;
        canvas.worldCamera = Camera.main;
    }

    private IEnumerator Wait()
    {
        while(true)
        {
            if(ChessBoard.isMoveChess || UIManager.isPause)
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
