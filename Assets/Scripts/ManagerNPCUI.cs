using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerNPCUI : MonoBehaviour
{
    [SerializeField] GameObject nameTagPrefab;
    [SerializeField] GameObject colorBody;
    [SerializeField] GameObject arrowPrefab;

    private GameObject uiUse;
    private Canvas canvasName;
    private TextMeshProUGUI[] texts;
    private Transform nameTagPosition;
    private EnemyController owner;
    private RectTransform canvasRect;

    private float width, height;
    private float x, y;

    private GameObject arrow;
    private Image numberBackground;
    // Start is called before the first frame update
    void OnEnable()
    {
        owner = GetComponent<EnemyController>();
        nameTagPosition = transform.GetChild(4);
        Canvas[] listCanvas = GameObject.FindObjectsOfType<Canvas>();

        foreach (Canvas c in listCanvas)
        {
            if (c.name == "NameCanvas")
            {
                canvasName = c;
                canvasRect = c.GetComponent<RectTransform>();
            }
        }
        uiUse = Instantiate(nameTagPrefab, canvasName.transform);
        arrow = Instantiate(arrowPrefab, canvasName.transform);

        texts = uiUse.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = gameObject.name;
        texts[1].text = "1";

        numberBackground = uiUse.GetComponentsInChildren<Image>()[0];

        width = canvasName.GetComponent<RectTransform>().sizeDelta.x;
        height = canvasName.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow != null)
        {
            arrow.transform.GetComponent<Image>().color = colorBody.GetComponent<SkinnedMeshRenderer>().material.color;
        }
        numberBackground.color = colorBody.GetComponent<SkinnedMeshRenderer>().material.color;
        if (canvasName != null)
        {
            Vector3 indicatorPosition = Camera.main.WorldToScreenPoint(nameTagPosition.position);
            Vector3 indicatorPositionArrow = new Vector3();
            Vector3 indicatorPositionNumber = new Vector3();

            if (indicatorPosition.z >= 0f & indicatorPosition.x <= canvasRect.rect.width * canvasRect.localScale.x
                & indicatorPosition.y <= canvasRect.rect.height * canvasRect.localScale.x & indicatorPosition.x >= 0f & indicatorPosition.y >= 0f)
            {
                indicatorPositionNumber = Camera.main.WorldToScreenPoint(nameTagPosition.position);
                indicatorPositionNumber.z = 0f;
                targetOutOfSight(false, indicatorPositionNumber);
            }
            else if (indicatorPosition.z >= 0f)
            {
                indicatorPositionNumber = OutOfRangeIndicatorPositionA(indicatorPosition);
                indicatorPositionArrow = OutOfRangeIndicatorPositionB(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }
            else
            {
                indicatorPosition *= -1f;
                indicatorPositionNumber = OutOfRangeIndicatorPositionA(indicatorPosition);
                indicatorPositionArrow = OutOfRangeIndicatorPositionB(indicatorPosition);
                targetOutOfSight(true, indicatorPosition);
            }
            if (uiUse != null) uiUse.GetComponent<RectTransform>().position = indicatorPositionNumber;
            if (arrow != null) arrow.GetComponent<RectTransform>().position = indicatorPositionArrow;
            if (owner.isDead)
            {
                Destroy(uiUse);
                Destroy(arrow);
            }
        }
    }

    private Vector3 OutOfRangeIndicatorPositionB(Vector3 indicatorPosition)
    {
        indicatorPosition.z = 0f;
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        indicatorPosition -= canvasCenter;
        float divX = (canvasRect.rect.width / 2f) / Mathf.Abs(indicatorPosition.x);
        float divY = (canvasRect.rect.height / 2f) / Mathf.Abs(indicatorPosition.y);
        if (divX < divY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasRect.rect.width * 0.5f - 30f) * canvasRect.localScale.x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }
        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);

            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasRect.rect.height / 2f - 50f) * canvasRect.localScale.y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }
        indicatorPosition += canvasCenter;
        return indicatorPosition;
    }

    private Vector3 OutOfRangeIndicatorPositionA(Vector3 indicatorPosition)
    {
        indicatorPosition.z = 0f;
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        indicatorPosition -= canvasCenter;
        float divX = (canvasRect.rect.width / 2f) / Mathf.Abs(indicatorPosition.x);
        float divY = (canvasRect.rect.height / 2f) / Mathf.Abs(indicatorPosition.y);
        if (divX < divY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) * (canvasRect.rect.width * 0.5f - 70f) * canvasRect.localScale.x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }
        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);
            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) * (canvasRect.rect.height / 2f - 90f) * canvasRect.localScale.y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
        }
        indicatorPosition += canvasCenter;
        return indicatorPosition;
    }

    private void targetOutOfSight(bool oos, Vector3 indicatorPosition)
    {
        if (arrow != null)
        {
            if (oos)
            {
                texts[0].text = "";
                arrow.gameObject.SetActive(true);
                arrow.GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(rotationOutOfSightTargetIndicator(indicatorPosition));
            }
            else
            {
                arrow.gameObject.SetActive(false);
                texts[0].text = gameObject.name;
            }
        }

    }
    private Vector3 rotationOutOfSightTargetIndicator(Vector3 indicatorPosition)
    {
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition - canvasCenter, Vector3.forward);
        return new Vector3(0f, 0f, angle);
    }
}
