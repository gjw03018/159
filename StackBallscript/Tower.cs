using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Tower : MonoBehaviour
{
    public GameObject[] floor;  //��ü �ٴ� �������� ������ �ִ� �迭
    [HideInInspector]
    public GameObject[] floorPrefabs = new GameObject[4];   //������ �ٴ� �������� ������ ���� �迭
    public GameObject lastFloor;    //������ �ٴ�

    GameObject temp1, temp2;    //������ ������ ������ ���� ����

    private int level = 1;  //���� ����
    private int addOn = 7;  //�������߰�
    private float i = 0;    //����

    public Material plateMat, baseMat;
    public MeshRenderer playerMesh;

    private void Awake()
    {
        plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        baseMat.color = plateMat.color + Color.gray;
        playerMesh.material.color = plateMat.color;

        level = PlayerPrefs.GetInt("Level", 1);

        if (level > 9)
            addOn = 0;

        FloorSelect();
        float random = Random.value;
        for (i = 0; i > -level - addOn; i -= 0.5f)
        {
            //������ ������ �ٴ� ����
            if (level <= 20)
                temp1 = Instantiate(floorPrefabs[Random.Range(0, 2)]);
            else if(level > 20 && level < 50)
                temp1 = Instantiate(floorPrefabs[Random.Range(1, 3)]);
            else if(level > 50 && level < 100)
                temp1 = Instantiate(floorPrefabs[Random.Range(2,4)]);
            else if (level > 100)
                temp1 = Instantiate(floorPrefabs[Random.Range(3, 4)]);

            //����
            temp1.transform.position = new Vector3(0, i - 0.01f, 0);

            //���밪 �������� ȸ������
            if(Mathf.Abs(i) >= level * 0.0f && Mathf.Abs(i) < level * 0.8f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                temp1.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(i) >= level * 0.8f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                if(random > 0.75f)
                    temp1.transform.eulerAngles += Vector3.up * 180;
            }


            //�θ� ����
            temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }
        //������ �ٴ�
        temp2 = Instantiate(lastFloor);
        temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }

    //�ٴ� ����
    void FloorSelect()
    {
        //�ٴ� ������ 5������ �ϳ� ����
        int randomModel = Random.Range(0, 5);

        //�ٴڴ� 4���� ������ ���� 
        switch(randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                    floorPrefabs[i] = floor[i];
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                    floorPrefabs[i] = floor[i+4];
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                    floorPrefabs[i] = floor[i + 8];
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                    floorPrefabs[i] = floor[i + 12];
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                    floorPrefabs[i] = floor[i + 16];
                break;
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
}
