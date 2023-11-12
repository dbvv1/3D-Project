using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;

    //实现画面的渐入：即将透明度从0变成1
    public void Fadein(float duration)
    {
        Color targetColor = fadeImage.color;
        targetColor.a = 1;
        fadeImage.DOBlendableColor(targetColor, duration);
    }
    
    //实现画面的渐出：即将透明度从1变成0
    public void FadeOut(float duration)
    {
        Color targetColor = fadeImage.color;
        targetColor.a = 0;
        fadeImage.DOBlendableColor(targetColor, duration);
    }
    
    
}
