using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowZC : MonoBehaviour
{
    //SerializeField
    [SerializeField] Vector3 offSet;
    [SerializeField] GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = player.transform.position + offSet;
    }
}
