using UnityEngine;

public class Close : MonoBehaviour
{
    void Update()
    {
        // 检查鼠标左键是否被按下
        if (Input.GetMouseButtonDown(0))
        {
            // 确保只有在Canvas可见的情况下才能隐藏它
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }
}