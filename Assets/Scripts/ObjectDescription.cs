using UnityEngine;

public class ObjectDescription : MonoBehaviour
{
    public GameObject descriptionPanel; // 引用对话框的 Panel

    private bool isPlayerNear = false;

    void Update()
    {
        // 检测玩家是否按下 E 键并且玩家在物体附近
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNear && descriptionPanel != null)
        {
            // 切换对话框的显示状态
            descriptionPanel.SetActive(!descriptionPanel.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检测玩家是否进入触发区域
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检测玩家是否离开触发区域
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            descriptionPanel.SetActive(false); // 玩家离开时隐藏对话框
        }
    }
}
