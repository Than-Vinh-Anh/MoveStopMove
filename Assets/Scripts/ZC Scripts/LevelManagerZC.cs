using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerZC : MonoBehaviour
{
    public static LevelManagerZC instance;
    private void Awake()
    {
        instance = this;
    }
    //SerializeField
    //
    //Private
    //
    //Public
    public GameObject[] listZombies;
    //
    void Start()
    {
        listZombies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {

    }
}
