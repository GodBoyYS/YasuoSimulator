using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


// �ýű�ֻ�����ܡ��չ��ȵ��ͷţ��Լ�Ч��ʵ��
// ���漰�����������

public class CameraController : MonoBehaviour
{
    //private float offsetX = -30;
    [SerializeField] private float offsetX = 0;
    [SerializeField] private float offsetY = 800;
    [SerializeField] private float offsetZ = 600;

    public float scrollSpeed; // ������ƶ��ٶ�
    public float edgeSize = 50.0f; // �߽�����Ŀ��

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
        // ��ȡ����λ��
        Vector3 mousePos = Input.mousePosition;

        // ˮƽ�ƶ�
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
            //Debug.Log("��곬���ϱ߽�");
            pos.z -= scrollSpeed * Time.deltaTime;
        }
        else if (mousePos.y <= edgeSize)
        {
            //Debug.Log("��곬���±߽�");
            pos.z += scrollSpeed * Time.deltaTime;
        }
    }
}
