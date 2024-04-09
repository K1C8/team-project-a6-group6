using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSingleManager : MonoBehaviour
{
    public static MultiSingleManager Instance;
    public bool isMulti = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
