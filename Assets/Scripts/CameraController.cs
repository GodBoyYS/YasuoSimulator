using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


// 该脚本只负责技能、普攻等的释放，以及效果实现
// 不涉及动画相关问题

public class CameraController : MonoBehaviour
{
    //private float offsetX = -30;
    [SerializeField] private float offsetX = 0;
    [SerializeField] private float offsetY = 800;
    [SerializeField] private float offsetZ = 600;

    public float scrollSpeed; // 摄像机移动速度
    public float edgeSize = 50.0f; // 边界区域的宽度

    public GameObject hero;
    private Vector3 cameraOffset;

    private Vector3 pos;

    private bool isCameraLocked;

    private void Start()
    {
        //Vector3 offset = this.transform.position - hero.transform.position;
        //Debug.Log($"offset = {offset}");
        cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
        transform.position = hero.transform.position + cameraOffset;
        isCameraLocked = false;
    }

    void Update()
    {
        pos = transform.position;

        if (Input.GetKeyDown(KeyCode.Y))
        {// lock the camera
            isCameraLocked = !isCameraLocked;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            pos = hero.transform.position + cameraOffset; 
        }

        //CheckMouseOutofScreen();
        
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0)
        {
            pos.y += mouseScroll * 100;
        }

    }

    private void FixedUpdate()
    {
        if(isCameraLocked == true)
        {
            transform.position = hero.transform.position + cameraOffset;
        }
        else
        {
            transform.position = pos;
        }
    }

    void LockCamera()
    {
        
    }

    void CheckMouseOutofScreen()
    {
        // 获取鼠标的位置
        Vector3 mousePos = Input.mousePosition;

        // 水平移动
        if (mousePos.x >= Screen.width - edgeSize)
        {
            pos.x -= scrollSpeed * Time.deltaTime;
        }
        else if (mousePos.x <= edgeSize)
        {
            pos.x += scrollSpeed * Time.deltaTime;
        }

        if (mousePos.y >= Screen.height - edgeSize)
        {
            //Debug.Log("鼠标超过上边界");
            pos.z -= scrollSpeed * Time.deltaTime;
        }
        else if (mousePos.y <= edgeSize)
        {
            //Debug.Log("鼠标超过下边界");
            pos.z += scrollSpeed * Time.deltaTime;
        }
    }
}
