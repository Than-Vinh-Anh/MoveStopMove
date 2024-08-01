using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopController : MonoBehaviour
{
    public static WeaponShopController instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] RawImage weaponImage;
    [SerializeField] Image[] borderImages;
    [SerializeField] RawImage[] weaponSkinsImage;

    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] RenderTexture[] renderTextures1;
    [SerializeField] RenderTexture[] renderTextures2;

    [SerializeField] Button btnLeft;
    [SerializeField] Button btnRight;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSelect;

    [SerializeField] Canvas homeCanvas;
    public int weaponIndex;
    public int weaponSkinIndex;
    private RenderTexture curRenderTexture;
    private void Start()
    {
        foreach (Image image in borderImages)
        {
            image.gameObject.SetActive(false);
        }
        weaponIndex = PlayerPrefs.GetInt("weapon");
        weaponSkinIndex = PlayerPrefs.GetInt("weaponSkin");
        switch (weaponIndex)
        {
            case 0:
                curRenderTexture = renderTextures1[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                break;
            case 1:
                curRenderTexture = renderTextures2[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                break;
        }

        nameTxt.text = curRenderTexture.name;
    }
    public void onBtnLeftClick()
    {
        weaponIndex--;
        if (weaponIndex < 0)
        {
            weaponIndex = 0;
        }
        foreach (RawImage image in weaponSkinsImage)
        {
            image.gameObject.SetActive(false);
        }
        switch (weaponIndex)
        {
            case 0:
                curRenderTexture = renderTextures1[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                for (int i = 0; i < renderTextures1.Length; i++)
                {
                    if (i < renderTextures1.Length)
                    {
                        weaponSkinsImage[i].gameObject.SetActive(true);
                        weaponSkinsImage[i].texture = renderTextures1[i];
                    }
                }
                break;
            case 1:
                curRenderTexture = renderTextures2[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                for (int i = 0; i < renderTextures2.Length; i++)
                {
                    if (i < renderTextures2.Length)
                    {
                        weaponSkinsImage[i].gameObject.SetActive(true);
                        weaponSkinsImage[i].texture = renderTextures2[i];
                    }
                }
                break;
        }
        nameTxt.text = curRenderTexture.name;
        weaponImage.texture = curRenderTexture;
    }
    public void onBtnRightClick()
    {
        weaponIndex++;
        foreach (RawImage image in weaponSkinsImage)
        {
            image.gameObject.SetActive(false);
        }
        if (weaponIndex > 1)
        {
            weaponIndex = 1;
        }
        switch (weaponIndex)
        {
            case 0:
                curRenderTexture = renderTextures1[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                for (int i = 0; i < renderTextures1.Length; i++)
                {
                    if (i < renderTextures1.Length)
                    {
                        weaponSkinsImage[i].gameObject.SetActive(true);
                        weaponSkinsImage[i].texture = renderTextures1[i];
                    }
                }
                break;
            case 1:
                curRenderTexture = renderTextures2[weaponSkinIndex];
                weaponImage.texture = curRenderTexture;
                for (int i = 0; i < renderTextures2.Length; i++)
                {
                    if (i < renderTextures2.Length)
                    {
                        weaponSkinsImage[i].gameObject.SetActive(true);
                        weaponSkinsImage[i].texture = renderTextures2[i];
                    }
                }
                break;
        }
        nameTxt.text = curRenderTexture.name;
        weaponImage.texture = curRenderTexture;
    }
    public void onBtnCloseClick()
    {
        gameObject.SetActive(false);
        homeCanvas.gameObject.SetActive(true);

    }
    public void onSkinClick(int index)
    {
        weaponSkinIndex = index;
        for (int i = 0; i < borderImages.Length; i++)
        {
            if (i == index)
            {
                borderImages[i].gameObject.SetActive(true);
            }
            else
            {
                borderImages[i].gameObject.SetActive(false);
            }
        }
        switch (weaponIndex)
        {
            case 0:
                weaponImage.texture = renderTextures1[weaponSkinIndex];
                break;
            case 1:
                weaponImage.texture = renderTextures2[weaponSkinIndex];
                break;
        }
    }
    public void onBtnSelectClick()
    {
        PlayerPrefs.SetInt("weapon", weaponIndex);
        PlayerPrefs.SetInt("weaponSkin", weaponSkinIndex);
        PlayerController.instance.SetWeapon(weaponIndex, weaponSkinIndex);
    }
}
