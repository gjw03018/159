using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Tower : MonoBehaviour
{
    public GameObject[] floor;  //전체 바닥 프리팹을 가지고 있는 배열
    [HideInInspector]
    public GameObject[] floorPrefabs = new GameObject[4];   //생성될 바닥 프리팹을 가지고 있을 배열
    public GameObject lastFloor;    //마지막 바닥

    GameObject temp1, temp2;    //생성될 프리팹 정보를 가질 변수

    private int level = 1;  //현재 레벨
    private int addOn = 7;  //레벨당추가
    private float i = 0;    //개수

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
            //레벨당 생성될 바닥 구현
            if (level <= 20)
                temp1 = Instantiate(floorPrefabs[Random.Range(0, 2)]);
            else if(level > 20 && level < 50)
                temp1 = Instantiate(floorPrefabs[Random.Range(1, 3)]);
            else if(level > 50 && level < 100)
                temp1 = Instantiate(floorPrefabs[Random.Range(2,4)]);
            else if (level > 100)
                temp1 = Instantiate(floorPrefabs[Random.Range(3, 4)]);

            //간격
            temp1.transform.position = new Vector3(0, i - 0.01f, 0);

            //절대값 기준으로 회전설정
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


            //부모 설정
            temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }
        //마지막 바닥
        temp2 = Instantiate(lastFloor);
        temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }

    //바닥 선택
    void FloorSelect()
    {
        //바닥 프리팹 5개에서 하나 선택
        int randomModel = Random.Range(0, 5);

        //바닥당 4개의 프리팹 존재 
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
