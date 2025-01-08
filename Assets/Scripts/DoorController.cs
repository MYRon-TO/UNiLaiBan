using UnityEngine;

public class DoorController : MonoBehaviour
{
  // public Transform door_a;  // 门的 Transform
  public Transform door_b;  // 门的 Transform
  public Transform door_c;  // 门的 Transform
  public float moveSpeed_b = 2f;  // 门的移动速度
  public float moveSpeed_c = 3f;  // 门的移动速度
  private float triggerDistance_b = 1.36f;  // 门的最大移动距离
  private float triggerDistance_c = 2.7f;  // 门的最大移动距离
  private bool isPlayerNear = false;  // 玩家是否靠近门
  private float initalPositionX_b;
  private float initalPositionX_c;

  private void Start()
  {
    if (door_b != null && door_c != null)
    {
      // 记录门的初始位置
      initalPositionX_b = door_b.position.x;  // 3 - 4.36 = -1.36 = -1.36
      initalPositionX_c = door_c.position.x;  // 1.2 - 3.9 = -1.2 = -2.7
    }
  }

  private void Update()
  {
    // 如果玩家靠近门，门沿 X 轴正方向移动
    if (door_b != null && door_c != null)
    {
      if (isPlayerNear)
        MoveDoor(true);
      else
        MoveDoor(false);
    }
  }

  // 检测玩家是否进入触发区域
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))  // 检查是否是玩家
    {
      isPlayerNear = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))  // 检查玩家离开触发区域
    {
      isPlayerNear = false;
    }
  }

  // 控制门的移动
  private void MoveDoor(bool front)
  {
    if (front)
    {
      // 移动门，沿 X 轴正方向
      if (door_b.position.x < initalPositionX_b + triggerDistance_b)
        door_b.Translate(Vector3.right * moveSpeed_b * Time.deltaTime);
      if (door_c.position.x < initalPositionX_c + triggerDistance_c)
        door_c.Translate(Vector3.right * moveSpeed_c * Time.deltaTime);
    }
    else
    {
      if (door_b.position.x > initalPositionX_b)
        door_b.Translate(-Vector3.right * moveSpeed_b * Time.deltaTime);
      if (door_c.position.x > initalPositionX_c)
        door_c.Translate(-Vector3.right * moveSpeed_c * Time.deltaTime);
    }
  }
}
