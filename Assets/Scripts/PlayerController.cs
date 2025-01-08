using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private CharacterController characterController;
  private Camera mainCamera;

  // 移动速度
  public float moveSpeed = 3f;

  // 鼠标灵敏度
  public float sen = 600f;

  // 旋转角度
  private float xRotation;
  private float yRotation;

  // 重力
  private float gravity = 20;
  // 跳跃速度
  private float jumpSpeed = 7;
  // 竖直方向速度
  private float verticalSpeed = -1f;

  // 水平平面移动方向
  private Vector3 moveDirection;
  // 竖直方向移动
  private Vector3 verticalMovement;

  // 拾取范围
  public float pickUpRange = 7f;
  public Transform handPosition;   // 玩家手部位置（或者展示物体的区域）
  public float itemMoveSpeed = 5f;     // 物体跟随手部的移动速度

  // 拿起的物体
  private GameObject heldObject = null;
  private Vector3 originalPosition; // 物体原始位置
  private Quaternion originalRotation; // 物体原始旋转

  void Start()
  {
    // 指针锁定在屏幕中央
    Cursor.lockState = CursorLockMode.Locked;
    // 指针不可见
    Cursor.visible = false;

    characterController = GetComponent<CharacterController>();
    mainCamera = GetComponentInChildren<Camera>();
  }

  void Update()
  {
    JumpHandler();
    MovementHandler();
    RotationHandler();
    PickUpHandler();
  }

  // 控制器移动
  void MovementHandler()
  {
    // 获取键盘水平方向（A、D）输入
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    // 获取键盘垂直方向（W、S）输入
    float verticalInput = Input.GetAxisRaw("Vertical");

    // 水平平面上控制器的移动方向
    moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

    // moveDirection * moveSpeed 为水平平面上的移动，verticalMovement 为竖直方向上的移动
    characterController.Move((moveDirection * moveSpeed + verticalMovement) * Time.deltaTime);
  }

  // 视角转动
  void RotationHandler()
  {
    // 获取鼠标X（水平）方向输入，并转换为角度
    float angleX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sen;
    // 获取鼠标X（垂直）方向输入，并转换为角度
    float angleY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sen;


    // Unity的坐标系是左手系
    // 左手握拳伸出拇指，拇指指向坐标轴正向，四指方向是旋转的正方向
    // 向上的坐标轴为Y轴，所以鼠标左右移动是物体绕Y轴转动
    yRotation += angleX;

    // 向右的坐标轴为X轴，所以鼠标上下移动是物体绕X轴转动
    xRotation -= angleY;
    // 限制视角上下转动的角度，不允许无限制转动
    xRotation = Mathf.Clamp(xRotation, -70f, 50f);

    // 控制器执行在水平方向上的旋转
    transform.rotation = Quaternion.Euler(0, yRotation, 0);
    // 摄像机执行在竖直方向上的旋转
    mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
  }

  // 跳跃
  void JumpHandler()
  {
    // Character Controller中的API：isGrounded，判断是否碰地
    if (characterController.isGrounded)
    {
      if (Input.GetKeyDown(KeyCode.Space))
        verticalSpeed = jumpSpeed;
      // 如果未跳跃，给一个较小的向下速度，便于isGrounded判断
      else
        verticalSpeed = -1f;
    }
    else
    {
      // 自由落体
      verticalSpeed -= gravity * Time.deltaTime;
    }

    // Y轴表示竖直方向
    verticalMovement.y = verticalSpeed;
  }

  void PickUpHandler()
  {
    if (Input.GetKeyDown(KeyCode.E)) // 按下 E 键拾取或放下物体
    {
      if (heldObject == null)
        TryPickUp();
      else
        DropObject();
    }

    if (heldObject != null)
    {
      MoveObjectToHand();
    }
  }

  private void TryPickUp()
  {
    RaycastHit hit;
    // 发射射线，检测物体
    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickUpRange))
    {
      if (hit.collider.CompareTag("Pickup"))  // 物体标签为 "Pickup"
      {
        heldObject = hit.collider.gameObject;  // 拾取物体
        originalPosition = heldObject.transform.position;  // 记录原始位置
        originalRotation = heldObject.transform.rotation;  // 记录原始旋转
        heldObject.transform.SetParent(handPosition);  // 将物体附加到手部位置
      }
    }
  }
  private void DropObject()
  {
    heldObject.transform.SetParent(null);  // 解除父物体
    // heldObject.GetComponent<Rigidbody>().isKinematic = false;  // 恢复物理模拟
    heldObject.transform.position = originalPosition;  // 将物体放回原处
    heldObject.transform.rotation = originalRotation;  // 恢复原始旋转
    heldObject = null;  // 清空当前拿的物体
  }

  private void MoveObjectToHand()
  {
    // 平滑移动物体到手部位置
    heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, handPosition.position, moveSpeed * Time.deltaTime);
    heldObject.transform.rotation = Quaternion.Slerp(heldObject.transform.rotation, handPosition.rotation, moveSpeed * Time.deltaTime);
  }
}

