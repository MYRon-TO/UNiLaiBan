using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool inter = true;
    public string canvasName; // 这里是物体名称

    private void Start()
    {
        canvasName = gameObject.name; // 如果Canvas名字和物体相同，可以直接赋值
    }
}