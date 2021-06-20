using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI;
    public GameObject allBtn;

    private bool btns;

    public Button soundBtn;
    public Sprite soundOn, soundOff;

    public Image levelSlider;
    public Text currentLevel;
    public Text nextLevel;
    public Image currentLevelImage;
    public Image nextLevelImage;

    public Text scoreText;
    public Text currentScoreText;
    public Text bestScoreText;

    private Material playerMaterial;
    private Player player;

    private void Awake()
    {
        playerMaterial = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<Player>();
        levelSlider.transform.parent.GetComponent<Image>().color = playerMaterial.color + Color.gray;
        levelSlider.color = playerMaterial.color;

        currentLevelImage.color = playerMaterial.color;
        nextLevelImage.color = playerMaterial.color;

        soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }

    private void Start()
    {
        currentLevel.text = PlayerPrefs.GetInt("Level").ToString();
        nextLevel.text = (PlayerPrefs.GetInt("Level")+1).ToString();
    }
    private void Update()
    {
        if(player.playerState == Player.PlayerState.Ready)
        {
            if (SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOn)
                soundBtn.GetComponent<Image>().sprite = soundOn;

            else if (!SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOff)
                soundBtn.GetComponent<Image>().sprite = soundOff;

        }

        if(Input.GetMouseButtonDown(0) && player.playerState == Player.PlayerState.Ready && !IgnoreUI() )
        {
            player.playerState = Player.PlayerState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            
        }

        if(player.playerState == Player.PlayerState.Death)
        {
            gameOverUI.SetActive(true);
            currentScoreText.text = ScoreManager.instance.scoreText.text;
            bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        }

        if (player.playerState == Player.PlayerState.Finish)
        {
            finishUI.SetActive(true);
        }
    }
    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for(int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.tag == "UI")
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        return raycastResults.Count > 0;
    }
    public void LevelSliderFill(float value)
    {
        levelSlider.fillAmount = value;
    }

    public void Setting()
    {
        btns = !btns;
        allBtn.SetActive(btns);
    }
}
