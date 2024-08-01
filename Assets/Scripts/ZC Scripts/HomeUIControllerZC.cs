using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIControllerZC : MonoBehaviour
{
    //SerializeField
    [SerializeField] Canvas touchCanvas;

    [SerializeField] GameObject powerUpPanel;
    [SerializeField] GameObject upgradePanel;

    [SerializeField] Image coin;
    [SerializeField] Image pause;
    //
    //Private
    //
    //Public
    //
    private void Start()
    {
        touchCanvas.gameObject.SetActive(false);
    }
    public void OnBtnHomeClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OnBtnNoClick()
    {
        touchCanvas.gameObject.SetActive(true);
        coin.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        powerUpPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }
    public void OnBtnSelectClick()
    {
        touchCanvas.gameObject.SetActive(true);
        coin.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        powerUpPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }
}
