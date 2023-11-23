using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    //设置tooltip的位置
    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        //得到tooltip的高和宽
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float height = corners[1].y - corners[0].y;
        float width = corners[2].x - corners[1].x;

        //默认使用右下方 不满足条件时改变方向

        //鼠标的位置小于tooltip的高度，只能往上面放
        rectTransform.position = mousePos;
        if (mousePos.y < height)
            rectTransform.position += Vector3.up * height * 0.6f;
        else
            rectTransform.position += Vector3.down * height * 0.6f;
        //鼠标距离右边的宽度小于tooltip的宽度 只能往左边放
        if (Screen.width - mousePos.x < width)
            rectTransform.position += Vector3.left * width * 0.6f;
        else
            rectTransform.position += Vector3.right * width * 0.6f;

    }

    public void SetItemText(ItemData_SO itemData)
    {
        if (itemData != null)
        {
            itemNameText.text = itemData.name;
            itemDescriptionText.text = itemData.itemDescription;
        }
        else
        {
            itemNameText.text = string.Empty;
            itemDescriptionText.text = string.Empty;
        }
    }
}
