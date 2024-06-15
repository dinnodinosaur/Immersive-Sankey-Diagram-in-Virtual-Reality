using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeText : MonoBehaviour
{
    public string textToDisplay = "Hello World";
    private GameObject textGameObject;
    private Camera mainCamera;

    void Start()
    {
        // 创建文本对象并作为子对象添加
        textGameObject = new GameObject("TextNode");
        var textMesh = textGameObject.AddComponent<TextMeshPro>();
        textMesh.text = textToDisplay;
        textGameObject.transform.SetParent(this.transform);

        // 设置文本位置和大小
        textGameObject.transform.localPosition = Vector3.zero; // 或者您选择的位置
        textMesh.fontSize = 150;
        textMesh.margin = new Vector4(18,0,0,0);
        textMesh.alignment = TextAlignmentOptions.BaselineLeft;
        textMesh.enableWordWrapping = false;
        textMesh.color = new Color(0,0,0);
        // Material mat = textMeshPro.fontMaterial;
        // // 更改材质的颜色
        // mat.SetColor("_FaceColor", Color.blue); // 更改前景色
        // mat.SetColor("_OutlineColor", Color.black);

        // textMesh.characterSize = 15f;
        // textMesh.anchor = TextAnchor.MiddleLeft;

        // 获取主摄像头
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 使文本始终面向摄像头
        if (mainCamera != null)
        {
            Vector3 cameraRotation = mainCamera.transform.eulerAngles;
            cameraRotation.x = 0; // 移除X轴旋转
            cameraRotation.z = 0; // 移除Z轴旋转

            // 将文本的旋转设置为修改后的摄像机旋转
            textGameObject.transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }

    // 可选：提供一个方法来动态更改文本内容
    public void SetText(string newText)
    {
        textToDisplay = newText;
        if (textGameObject != null)
        {
            var textMesh = textGameObject.GetComponent<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = newText;
            }
        }
    }
}
