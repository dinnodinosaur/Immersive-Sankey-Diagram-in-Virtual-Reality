using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaData : MonoBehaviour
{
    // Start is called before the first frame update
    public float alphaValue;
    public float lastAlpha;

    // 用于设置值的方法
    public void SetValue(float newValue)
    {
        lastAlpha = alphaValue;
        alphaValue = newValue;
    }

    // 用于获取值的方法
    public float GetValue()
    {
        alphaValue = lastAlpha;
        return lastAlpha;
    }
}
