using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Chess
{
    public int n = 1;

    public override void GetPointForMove()
    {
        Debug.Log("Rock");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
