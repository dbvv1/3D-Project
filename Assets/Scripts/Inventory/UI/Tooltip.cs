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

    //����tooltip��λ��
    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        //�õ�tooltip�ĸߺͿ�
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float height = corners[1].y - corners[0].y;
        float width = corners[2].x - corners[1].x;

        //Ĭ��ʹ�����·� ����������ʱ�ı䷽��

        //����λ��С��tooltip�ĸ߶ȣ�ֻ���������
        rectTransform.position = mousePos;
        if (mousePos.y < height)
            rectTransform.position += Vector3.up * height * 0.6f;
        else
            rectTransform.position += Vector3.down * height * 0.6f;
        //�������ұߵĿ��С��tooltip�Ŀ�� ֻ������߷�
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
