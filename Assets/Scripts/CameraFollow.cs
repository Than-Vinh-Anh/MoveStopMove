using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject Player;
    [SerializeField] Vector3 offSet;
    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + offSet;
    }
    public Vector3 ChangeCameraOffset(Vector3 newOffset)
    {
        offSet = newOffset;
        return offSet;
    }
    public Vector3 AddToCameraOffset(Vector3 newOffset)
    {
        offSet += newOffset;
        return offSet;
    }
}
