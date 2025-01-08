using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Rigidbody rb;
  public float moveSpeed = 5f; // 移动速度
  public float lookSpeedX = 2f; // 水平旋转速度
  public float lookSpeedY = 2f; // 垂直旋转速度
  public Transform playerBody; // 玩家身体 Transform
  private float xRotation = 0f; // 垂直旋转角度
  public float jumpForce = 10f;  // 跳跃的力量
  public float gravity = -9.8f;  // 自定义的重力加速度（可以调整）

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.useGravity = false;
  }

  void Update()
  {
    // 获取鼠标输入（控制视角）
    float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
    float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

    // 水平旋转玩家
    playerBody.Rotate(Vector3.up * mouseX);

    // 垂直旋转摄像机（限制垂直角度）
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    // 获取键盘输入（控制移动）
    float moveX = Input.GetAxis("Horizontal");
    float moveZ = Input.GetAxis("Vertical");

    Vector3 move = transform.right * moveX + transform.forward * moveZ;
    transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    {
      rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // 使角色向上跳跃
    }
  }

  void FixedUpdate()
  {
    // 模拟重力
    if (!IsGrounded())  // 如果角色不在地面上，就模拟重力
    {
      rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }
  }

  private bool IsGrounded()
  {
    // 检查是否接触地面（通过射线检测角色脚下是否有碰撞体）
    return Physics.Raycast(transform.position, Vector3.down, 1.1f);
  }
}
