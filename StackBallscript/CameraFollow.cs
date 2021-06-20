using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 camFollow;
    Transform player, win;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (win == null)
            win = GameObject.Find("win(Clone)").GetComponent<Transform>();

        //ȭ���� �������� ���� �߽����� �������� ������ �ٴ� ���� ���߰�
        if (transform.position.y > player.transform.position.y && transform.position.y > win.position.y + 3)
            camFollow = new Vector3(transform.position.x, player.transform.position.y
                , transform.position.z);

        transform.position = new Vector3(transform.position.x, camFollow.y, -5);
    }
}
