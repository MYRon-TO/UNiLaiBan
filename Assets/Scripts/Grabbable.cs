using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool grabbable = true;
    public float angularDamping = 1f;
    public float linearDamping = 5f;

    // 用来标记物件是不是可以被抓取

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // 获取Rigidbody组件并缓存它
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearDamping = linearDamping;
            rb.angularDamping = angularDamping;
        }
        else
        {
            Debug.LogError("Grabbable: No Rigidbody component found on this GameObject.");
        }

        // 注意：确保给物体添加了Rigidbody组件和Collider组件，否则这个脚本将不起作用。
    }

    // Update is called once per frame
    void Update()
    {
        // 如果需要在此方法中处理逻辑，请在这里添加代码
    }
}
