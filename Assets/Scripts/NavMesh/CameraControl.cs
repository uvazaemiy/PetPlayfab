using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    private Transform target; //��������� ���� ��� ������

    public float zoomSpeed = 5.0f; //�������� ����������� ������
    private Vector3 _offset; //�������� ������ ������������ �������
    public float mouse_sens = 1f;
    public Camera cam_holder;
    float x_axis, y_axis, z_axis, _rotY, _rotX; //���� �� x, y, ���, ���������� ��� ������

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position; //�������� ��������� ��������
    }

    void LookAtTarget()
    {
        Quaternion rotation = Quaternion.Euler(_rotY, _rotX, 0); //������ �������� ������ 
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }

    void LateUpdate()
    {
        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input != 0) //���� �������� �������� ����
        {
            cam_holder.fieldOfView *= 1 - input;// *zoomSpeed; //���
        }

        if (Input.GetMouseButton(0)) //����� ������ ����
        { //�������� ������ �������
            _rotX -= Input.GetAxis("Mouse X") * mouse_sens * -1 ; //������� ������ ������ ������� � ���������� ���������
            _rotY -= Input.GetAxis("Mouse Y") * mouse_sens;
        }

        LookAtTarget();

        //if (Input.GetMouseButton(1)) //������ ������
        //{
        //    //����� ������ �������
        //    //�������� ������ �� ���� X � Y
        //    x_axis = Input.GetAxis("Mouse X") * mouse_sens;
        //    y_axis = Input.GetAxis("Mouse Y") * mouse_sens;

        //    target.position = new Vector3(target.position.x + x_axis, target.position.y + y_axis, target.position.z);

        //    LookAtTarget();
        //}
        if (Input.GetMouseButton(2)) //��������
        {
            //����� ������ ������
            x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            y_axis = Input.GetAxis("Mouse Y") * mouse_sens;
            //z_axis = Input.GetAxis("Mouse ScrollWheel") * wheel_sens;

            cam_holder.transform.Rotate(Vector3.up, x_axis, Space.World);
            cam_holder.transform.Rotate(Vector3.right, y_axis, Space.Self);
            //cam_holder.transform.localPosition = cam_holder.transform.localPosition * (1 - z_axis);
        }
    }
}