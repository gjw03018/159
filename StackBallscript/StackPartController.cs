using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRender;
    private StackController stackController;
    private Collider collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRender = GetComponent<MeshRenderer>();
        stackController = transform.parent.GetComponent<StackController>();
        collider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        // ���� ȿ�� on, �ݶ��̴� off
        rb.isKinematic = false;
        collider.enabled = false;

        //�߽���ǥ
        Vector3 forcePoint = transform.parent.position;
        //�θ��� x��ǥ
        float pareXpos = transform.parent.position.x;
        //�ڽ� x ��ǥ
        float xPos = meshRender.bounds.center.x;

        //���ư��� ����
        Vector3 subDir = (pareXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        //�������� ������
        rb.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        //ȸ��
        rb.AddTorque(Vector3.left * torque);
        rb.velocity = Vector3.down;
    }
}
