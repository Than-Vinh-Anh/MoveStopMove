using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] Canvas settingCanvas;
    public void OnBtnSettingClick()
    {
        settingCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
