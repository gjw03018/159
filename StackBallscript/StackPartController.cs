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
        // 물리 효과 on, 콜라이더 off
        rb.isKinematic = false;
        collider.enabled = false;

        //중심좌표
        Vector3 forcePoint = transform.parent.position;
        //부모의 x좌표
        float pareXpos = transform.parent.position.x;
        //자신 x 좌표
        float xPos = meshRender.bounds.center.x;

        //날아가는 방향
        Vector3 subDir = (pareXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        //방향으로 날리기
        rb.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        //회전
        rb.AddTorque(Vector3.left * torque);
        rb.velocity = Vector3.down;
    }
}
