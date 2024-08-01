using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    //SerializeField
    [SerializeField] Image soundBtnOff;
    [SerializeField] Image soundBtnOn;
    [SerializeField] Image vibrateBtnOff;
    [SerializeField] Image vibrateBtnOn;
    [SerializeField] Canvas inGameCanvas;
    //
    //private
    public int soundState;
    public int vibrateState;
    //
    //public
    //
    private void Start()
    {
        soundState = PlayerPrefs.GetInt("sound");
        vibrateState = PlayerPrefs.GetInt("vibrate");
        if(soundState == 0)
        {
            soundBtnOff.gameObject.SetActive(true);
            soundBtnOn.gameObject.SetActive(false);
        }
        else
        {
            soundBtnOff.gameObject.SetActive(false); 
            soundBtnOn.gameObject.SetActive(true);
        }

        if (vibrateState == 0)
        {
            vibrateBtnOff.gameObject.SetActive(true);
            vibrateBtnOn.gameObject.SetActive(false);
        }
        else
        {
            vibrateBtnOff.gameObject.SetActive(false);
            vibrateBtnOn.gameObject.SetActive(true);
        }
    }
    public void OnBtnSoundClick()
    {
        if(soundState == 0)
        {
            soundState = 1;
            PlayerPrefs.SetInt("sound", soundState);
            soundBtnOff.gameObject.SetActive(false);
            soundBtnOn.gameObject.SetActive(true);
        }
        else
        {
            soundState = 0;
            PlayerPrefs.SetInt("sound", soundState);
            soundBtnOff.gameObject.SetActive(true);
            soundBtnOn.gameObject.SetActive(false);
        }
    }
    public void OnBtnVibrateClick()
    {
        if (vibrateState == 0)
        {
            vibrateState = 1;
            PlayerPrefs.SetInt("vibrate", vibrateState);
            vibrateBtnOff.gameObject.SetActive(false);
            vibrateBtnOn.gameObject.SetActive(true);
        }
        else
        {
            vibrateState = 0;
            PlayerPrefs.SetInt("vibrate", vibrateState);
            vibrateBtnOff.gameObject.SetActive(true);
            vibrateBtnOn.gameObject.SetActive(false);
        }
    }
    public void OnBtnHomeClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnBtnContinueClick()
    {
        inGameCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
