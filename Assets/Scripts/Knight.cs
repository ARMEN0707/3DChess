using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chess
{
    public override List<(int, int)> GetPointForMove(int x, int y)
    {
        Debug.Log("Knight");
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveChess(point);
    }
}
