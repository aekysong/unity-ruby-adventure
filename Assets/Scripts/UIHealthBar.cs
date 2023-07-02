using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }  // 정적 멤버는 같은 유형의 모든 오브젝트에서 공유
    // 어느 스크립트에서도 UIHealthBar.instance를 입력해 이 get 프로퍼티를 호출할 수 있음

    public Image mask;

    float originalSize;

    private void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
