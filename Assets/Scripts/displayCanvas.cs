using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public Camera mainCamera; // 引用主摄像机

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 自动获取主摄像机
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键是否被按下
        {
            CheckForInteraction();
        }
    }

    private void CheckForInteraction()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f); // 检查周围一定半径内的碰撞体
        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactableObject = hitCollider.GetComponent<InteractableObject>();
            if (interactableObject != null && interactableObject.inter)
            {
                ToggleCanvasVisibility(interactableObject.canvasName);
                break; // 假设一次只与一个物体交互
            }
        }
    }

    private void ToggleCanvasVisibility(string canvasName)
    {
        Transform canvasTransform = FindCanvasByName(mainCamera.transform, canvasName);
        if (canvasTransform != null)
        {
            GameObject canvas = canvasTransform.gameObject;
            canvas.SetActive(!canvas.activeSelf); // 直接切换GameObject的活动状态
        }
    }

    private Transform FindCanvasByName(Transform parent, string name)
    {
        // 如果当前对象就是我们要找的Canvas，则返回它
        if (parent.name == name && parent.GetComponent<Canvas>() != null)
        {
            return parent;
        }

        // 遍历所有子对象，递归查找
        foreach (Transform child in parent)
        {
            Transform foundCanvas = FindCanvasByName(child, name);
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
        }

        // 如果没有找到匹配的Canvas，返回null
        return null;
    }
}