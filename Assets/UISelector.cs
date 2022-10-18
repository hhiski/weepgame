using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelector : MonoBehaviour
{
    Camera Camera;
    public GameObject Corner;
    GameObject TopLeft;
    GameObject TopRight;
    GameObject ButtomLeft;
    GameObject ButtomRight;

    public bool Tracking = false;
    public Vector3 TargetPos;
    public float TargetSize;

    void Start()
    {
        Camera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera").GetComponent<Camera>();
        TopLeft = Instantiate(Corner, transform);
        TopRight = Instantiate(Corner, transform);
        ButtomLeft = Instantiate(Corner, transform);
        ButtomRight = Instantiate(Corner, transform);

        TopRight.transform.rotation = Quaternion.Euler(0, 0,-90);
        ButtomLeft.transform.rotation = Quaternion.Euler(0, -180, 90);
        ButtomRight.transform.rotation = Quaternion.Euler(180, 0, 0);


    }


        public void UpdateSelectorCorners(Vector3 targetPos, float targetSize)
        {

        float offset = targetSize * 7f;

        Camera camera = Camera;

        Vector3 topLeftPos = targetPos + camera.transform.right * -offset + camera.transform.up * offset;
        Vector3 topRightPos = targetPos + camera.transform.right * offset + camera.transform.up * offset;
        Vector3 buttomLeftPos = targetPos + camera.transform.right * offset + camera.transform.up * -offset;
        Vector3 buttomRightPos = targetPos + camera.transform.right * -offset + camera.transform.up * -offset;

        
        Vector3 targetPosViewport = camera.WorldToScreenPoint(topLeftPos);
        topLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        TopLeft.transform.position = topLeftPos;

        targetPosViewport = camera.WorldToScreenPoint(topRightPos);
        topRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        TopRight.transform.position = topRightPos;

        targetPosViewport = camera.WorldToScreenPoint(buttomLeftPos);
        buttomLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        ButtomLeft.transform.position = buttomLeftPos;

        targetPosViewport = camera.WorldToScreenPoint(buttomRightPos);
        buttomRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        ButtomRight.transform.position = buttomRightPos;



    }

    void Update()
    {
        if (Tracking)
        {
            UpdateSelectorCorners(TargetPos, TargetSize);
        }
    }
}
