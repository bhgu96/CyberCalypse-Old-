using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAAA : MonoBehaviour {
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
