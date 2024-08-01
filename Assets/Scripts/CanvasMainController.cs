using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMainController : MonoBehaviour
{
    [SerializeField] Button btnWeapon;
    [SerializeField] Button btnSkin;
    [SerializeField] Button btnZombieCity;
    [SerializeField] Slider expSlider;

    [SerializeField] Canvas weaponCanvas;
    [SerializeField] Canvas skinCanvas;

    public void onBtnWeaponClick()
    {
        weaponCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);

    }

    public void onBtnSkinClick()
    {
        skinCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onBtnZombieCityClick()
    {
        SceneManager.LoadScene("ZombieCity", LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        expSlider.value = PlayerPrefs.GetInt("totalKill");
    }
}
