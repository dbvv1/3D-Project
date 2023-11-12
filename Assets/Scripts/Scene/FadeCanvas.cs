using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;

    //ʵ�ֻ���Ľ��룺����͸���ȴ�0���1
    public void Fadein(float duration)
    {
        Color targetColor = fadeImage.color;
        targetColor.a = 1;
        fadeImage.DOBlendableColor(targetColor, duration);
    }
    
    //ʵ�ֻ���Ľ���������͸���ȴ�1���0
    public void FadeOut(float duration)
    {
        Color targetColor = fadeImage.color;
        targetColor.a = 0;
        fadeImage.DOBlendableColor(targetColor, duration);
    }
    
    
}
