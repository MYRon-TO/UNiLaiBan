using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    // 公共变量，用于指定目标物体的名字。
    public string targetName = "";

    // 私有变量，保存目标物体的坐标系。
    private Transform targetTransform = null;

    // 保存玩家自己作为参考。
    public Transform myself; 

    // 保存被抓取物体的相对旋转位置。
    private Vector3 targetEuler = Vector3.zero;

    // 保存被抓取物体相对抓把的位置。
    private Vector3 targetDistance = Vector3.zero;

    // 定义物体摆正时的角度。
    public Vector3 restAngle = Vector3.zero;

    // 记录上一次抓取物体时的旋转角度。
    private Vector3 lastAngle = Vector3.zero;

    // 记录物体原始的位置。
    private Vector3 originalPosition = Vector3.zero;

    // 记录物体原始的旋转状态。
    private Quaternion originalRotation = Quaternion.identity;

    // 标记是否正在抓取物体。
    private bool isGrabbing = false;

    void Start()
    {
        // 初始化myself为玩家的transform，假设它位于层级结构中的特定位置。
        if (transform.parent != null && transform.parent.parent != null)
        {
            myself = transform.parent.parent.transform;
        }
    }

    void Update()
    {
        // 指定鼠标左键作为抓取按键。
        KeyCode grabberKey = KeyCode.Mouse0;

        // 当按下鼠标左键并且没有正在抓取的物体时，尝试寻找新目标并开始抓取。
        if (Input.GetKeyDown(grabberKey) && !isGrabbing)
        {
            TryBeginGrab();
        }

        // 如果正在抓取并且持续按住鼠标左键，则更新物体位置和旋转。
        if (isGrabbing && Input.GetKey(grabberKey) && targetTransform != null)
        {
            UpdateGrab();
        }

        // 当松开鼠标左键时结束抓取并复原物体位置和旋转。
        if (Input.GetKeyUp(grabberKey) && isGrabbing)
        {
            EndGrab();
        }

        // 处理其他输入，如滚动轮、快捷键等。
        HandleOtherInputs();
    }

    private void TryBeginGrab()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f); // 检查周围一定半径内的碰撞体
        foreach (var hitCollider in hitColliders)
        {
            Grabbable temp;
            if (hitCollider.TryGetComponent<Grabbable>(out temp))
            {
                targetTransform = hitCollider.transform;
                BeginGrab();
                return;
            }
        }
    }

    private void BeginGrab()
    {
        // 设置抓取标记为真，并保存当前的状态。
        isGrabbing = true;
        lastAngle = myself.eulerAngles;
        targetEuler = targetTransform.eulerAngles - myself.eulerAngles;
        targetDistance = targetTransform.position - transform.position;
        originalPosition = targetTransform.position;
        originalRotation = targetTransform.rotation;
    }

    private void UpdateGrab()
    {
        Vector3 currentAngle = myself.eulerAngles;
        Vector3 angleDifference = currentAngle - lastAngle;
        targetDistance = Quaternion.Euler(angleDifference) * targetDistance;
        lastAngle = currentAngle;

        // 更新物体位置，使其跟随手部移动。
        targetTransform.position = transform.position + targetDistance;

        // 更新物体旋转，使其与手部联动。
        targetTransform.rotation = myself.rotation * Quaternion.Euler(targetEuler);
    }

    private void EndGrab()
    {
        // 结束抓取，将物体恢复到原始位置和旋转。
        isGrabbing = false;
        targetTransform.position = originalPosition;
        targetTransform.rotation = originalRotation;

        // 重置目标信息，以便可以再次抓取其他物体。
        targetTransform = null;
        targetName = "";
        targetEuler = Vector3.zero;
        targetDistance = Vector3.zero;
    }

    private void HandleOtherInputs()
    {
        // 滚动轮控制自身位置沿Z轴移动。
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
        }

        // 'R' 键重置被抓物体的旋转。
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetEuler = restAngle;
            targetDistance = Vector3.zero;
        }

        // 'F1' 键隐藏抓把。
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        // 'F2' 键显示抓把。
        if (Input.GetKeyDown(KeyCode.F2))
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
        }

        // 'X', 'Y', 'Z' 键分别沿对应的轴旋转被抓物体。
        if (Input.GetKey(KeyCode.X))
        {
            targetEuler = new Vector3(targetEuler.x + 0.1f, targetEuler.y, targetEuler.z);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            targetEuler = new Vector3(targetEuler.x, targetEuler.y + 0.1f, targetEuler.z);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            targetEuler = new Vector3(targetEuler.x, targetEuler.y, targetEuler.z + 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 这里不再设置目标物体，改为在TryBeginGrab中动态查找。
    }

    private void OnTriggerExit(Collider other)
    {
        // 这里也不再清除目标物体信息，因为我们在EndGrab中已经处理了。
    }
}