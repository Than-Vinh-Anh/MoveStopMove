using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControllerZC : MonoBehaviour
{
    [SerializeField] GameObject targetedCircle;
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void ToggleTargetedCircle(bool condition)
    {
        targetedCircle.SetActive(condition);
    }
}
