using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private bool smash;

    private float currentTime;
    private bool fever;

    private int currentBrokenStacks, totalStacks;

    public GameObject feverUI;
    public Image feverImage;
    public GameObject fireFX, winFX, splashFx;

    public enum PlayerState
    {
        Ready,
        Playing,
        Death,
        Finish
    }
    [HideInInspector]
    public PlayerState playerState = PlayerState.Ready;

    public AudioClip bounceClip, deathClip, winClip, destroyClip, iDestroyClip;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }

    private void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
        feverImage.fillAmount = 0;
        feverImage.color = Color.white;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            smash = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            smash = false;
        }

        if (playerState == PlayerState.Playing)
        {
            //피버 모드
            if (fever)
            {
                currentTime -= Time.deltaTime * 0.35f;
                if (!fireFX.activeInHierarchy)
                    fireFX.SetActive(true);
            }
            else
            {
                if (fireFX.activeInHierarchy)
                    fireFX.SetActive(false);

                if (smash)
                    currentTime += Time.deltaTime * 0.8f;
                else
                    currentTime -= Time.deltaTime * 0.5f;
            }

            if(currentTime >= 0.3f || feverImage.color == Color.red)
            {
                feverUI.SetActive(true);
            }
            else
            {
                feverUI.SetActive(false);
            }
            if (currentTime >= 1)
            {
                currentTime = 1;
                fever = true;
                feverImage.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                fever = false;
                feverImage.color = Color.white;
            }

            if (feverUI.activeInHierarchy)
                feverImage.fillAmount = currentTime / 1;
        }

        if (playerState == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<Tower>().NextLevel();
        }
        if (playerState == PlayerState.Death)
        {
            //죽으면 처음부터
            PlayerPrefs.SetInt("Level", 1);

            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        if(playerState == PlayerState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }

            if (rb.velocity.y > 5)
                rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        }
    }

    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;

        if (!fever)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(iDestroyClip, 0.5f);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);

            if(collision.gameObject.tag != "Win")
            {
                GameObject splash = Instantiate(splashFx);
                splash.transform.SetParent(collision.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0,359), 0);
                float randomScale = Random.Range(0.15f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            SoundManager.instance.PlaySoundFX(bounceClip, 0.5f);
        }
        else
        {
            //피버모드
            if(fever)
            { 
                if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Block"))
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
            }
            else
            {
                //부서지는 바닥
                if(collision.gameObject.CompareTag("Floor"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
                //부서지지 않는 바닥
                if (collision.gameObject.CompareTag("Block"))
                {
                    rb.isKinematic = true;
                    playerState = PlayerState.Death;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ScoreManager.instance.ResetScore();
                    SoundManager.instance.PlaySoundFX(deathClip, 0.5f);
                }
            }
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if(collision.gameObject.CompareTag("Win") && playerState == PlayerState.Playing)
        {
            playerState = PlayerState.Finish;
            SoundManager.instance.PlaySoundFX(winClip, 0.7f);
            GameObject win = Instantiate(winFX);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.CompareTag("Win"))
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }
}
