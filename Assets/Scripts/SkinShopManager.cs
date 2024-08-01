using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{
    public static SkinShopManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Image[] slots;
    [SerializeField] TextMeshProUGUI attributeTxt;
    [SerializeField] RenderTexture[] renderTexturesHat;
    [SerializeField] RenderTexture[] renderTexturesPant;
    [SerializeField] RenderTexture[] renderTexturesOffhand;
    [SerializeField] RenderTexture[] renderTexturesSkin;
    [SerializeField] GameObject[] categoryImage;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSelect;
    [SerializeField] Canvas homeCanvas;
    [SerializeField] GameObject player;
    [SerializeField] Camera skinShopCamera;

    public int skinIndex;
    private void OnEnable()
    {
        skinIndex = PlayerPrefs.GetInt("skin");
        categoryImage[0].GetComponent<Image>().color = new Color32(152, 152, 152, 255);
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < renderTexturesHat.Length)
            {
                slots[i].transform.GetChild(0).gameObject.SetActive(true);
                slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = renderTexturesHat[i];
            }
            else
            {
                slots[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        player.GetComponent<Animator>().SetBool("IsWin", true);
        skinShopCamera.depth = Camera.main.depth + 1;
    }

    private void OnDisable()
    {
        player.GetComponent<Animator>().SetBool("IsWin", false);
        skinShopCamera.depth = 0;
    }
    public void onBtnCloseClick()
    {
        gameObject.SetActive(false);
        homeCanvas.gameObject.SetActive(true);

    }
    public void onBtnSelectClick()
    {
        PlayerPrefs.SetInt("skin", skinIndex);
        PlayerController.instance.SetSkin(skinIndex);
    }
    public void onSkinClick(int index)
    {
        skinIndex = index;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == index)
            {
                if (slots[i].transform.childCount > 1)
                {
                    slots[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                attributeTxt.text = slots[i].name;
            }
            else
            {
                if (slots[i].transform.childCount > 1)
                {
                    slots[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
    public void onBtnCategoryClick(int index)
    {
        for (int i = 0; i < categoryImage.Length; i++)
        {
            if (i == index)
            {
                categoryImage[i].GetComponent<Image>().color = new Color32(152,152,152, 255);
            }
            else
            {
                categoryImage[i].GetComponent<Image>().color = new Color32(57, 57, 57, 255);
            }
        }
        switch (index)
        {
            case 0:
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < renderTexturesHat.Length)
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(true);
                        slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = renderTexturesHat[i];
                    }
                    else
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < renderTexturesPant.Length)
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(true);
                        slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = renderTexturesPant[i];
                    }
                    else
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < renderTexturesOffhand.Length)
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(true);
                        slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = renderTexturesOffhand[i];
                    }
                    else
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < renderTexturesSkin.Length)
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(true);
                        slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = renderTexturesSkin[i];
                    }
                    else
                    {
                        slots[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }
}
