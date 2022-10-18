using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = -600;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 200f;
    public float OrbitDampening = 100f;
    public float ScrollDampening = 6f;
    public float CloseDistance = 200f;
    public float MaxDistance = 860f;

    public bool GalacticPlane = false;
    public bool CameraDisabled = false;

    bool CameraMovementReady = true;

    Camera SystemCamera;

    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform.GetChild(0);
        this._XForm_Parent = this.transform;

        SystemCamera = GameObject.Find("System Camera").GetComponent<Camera>();

        GL.ClearWithSkybox(false, SystemCamera);

    }


    public void ResetCamera()
    {
        CameraToPos(new Vector3(0,0,0));
        _LocalRotation.y = 0;
        _LocalRotation.y = -30f;
        _CameraDistance = -200;
    }



    public void CameraToPos(Vector3 targetPos)
    {

        transform.parent.position = targetPos;


    }

    void CameraToMousePlane()
    {
        CameraMovementReady = false;
        Plane plane = new Plane(Vector3.up, 0);
        Vector3 pos = new Vector3(0, 0, 0);

        float distance;
        Ray ray = SystemCamera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            pos = ray.GetPoint(distance);
        }

        transform.parent.position = pos;

    }


    void ReadyCamera() {
        if (Input.GetButton("Fire3") == false)
        {
            CameraMovementReady = true;
        } else
        {
            Invoke("ReadyCamera", 0.11f);
        }
    }
    
    


    void LateUpdate()
    {



        if (!CameraDisabled && CameraMovementReady && Input.GetButton("Fire3") == true)
        {
            CameraToMousePlane();
            Invoke("ReadyCamera", 0.11f);
        }


        if (!CameraDisabled && Input.GetButton("Fire1") == true)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

               if (_LocalRotation.y < -90f)
                    _LocalRotation.y = -90f;
                else if (_LocalRotation.y > 30f)
                    _LocalRotation.y = 30f;
            }
        }





        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
            ScrollAmount = 20f * ScrollAmount;
            this._CameraDistance += ScrollAmount * -1f;

            this._CameraDistance = Mathf.Clamp(this._CameraDistance, -MaxDistance,-CloseDistance);


        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);

        //this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT,  OrbitDampening);


        //this._XForm_Parent.rotation = Quaternion.RotateTowards(this._XForm_Parent.rotation, QT, OrbitDampening * Time.deltaTime);
        this._XForm_Parent.rotation = Quaternion.Slerp(transform.rotation, QT, OrbitDampening * Time.deltaTime);

        if (Mathf.Round(this._XForm_Camera.localPosition.z) != Mathf.Round(this._CameraDistance))
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening) );
        }
    }
}