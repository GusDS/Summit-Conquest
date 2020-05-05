using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    [Header("Control")]
    public bool isActionOn;
    public GameObject player;
    [Header("Stats")]
    public int level = 0;
    public int levelSecondsTotal = 120;
    public int levelSecondsLeft;
    public int livesTotal = 3;
    public int livesLeft;
    public int livesAtPoints = 20000;
    public enum Scores : int
    {
        scoreZero = 0,
        scoreEnemySmall = 100,
        scoreEnemyBig = 2000,
        scoreCannonBall = 50,
        scorePowerupPush = 200,
        scorePowerupExplode = 300,
        scoreLevel = 5000,
        scoreSecond = 10
    }
    private int score = 0;
    private int hiScore = 0;
    private bool hasHiScore = false;
    private int histScore;
    private int histLevels;
    private int histSeconds;
    private int histLivesLost;
    private int histLivesExtra;
    private int beatenEnemySmall;
    private int beatenEnemyBig;
    private int beatenEnemyCannonBall;
    private int histBeatenEnemySmall;
    private int histBeatenEnemyBig;
    private int histBeatenEnemyCannonBall;
    private int gottenPowerupPush;
    private int gottenPowerupExplode;
    private int histGottenPowerupPush;
    private int histGottenPowerupExplode;
    [Header("Options")]
    public AudioSource musicSource;
    public AudioSource audioSource;
    public float audioVolume = 0.4f;
    public float musicVolume = 0.2f;
    public bool inputKeyboard = true;
    public bool inputMouse = true;
    [Header("UI")]
    public GameObject tittlePanel;
    public Button startButton;
    public Button instructionsButton;
    public Button quitButton;
    public Button returnButton;
    public GameObject instructionsPanel;
    public Text levelText;
    public Text timeText;
    public Text scoreText;
    public Text hiScoreText;
    public Text livesText;
    public RawImage iconKeyboard;
    public RawImage iconMouse;
    public GameObject menuPanel;
    public Text menuPanelText;
    public Button restartButton;
    public Button exitButton;
    public Button saveButton;
    public Text saveButtonText;
    public Toggle keyboardToggle;
    public Toggle mouseToggle;
    public Image musicIconOn;
    public Image musicIconOff;
    public Image audioIconOn;
    public Image audioIconOff;
    public Slider musicSlider;
    public Slider audioSlider;
    public AudioClip audioSample;
    public AudioClip beepSound;
    public Text statsText;
    public GameObject messagePanel;
    public Text messageText;
    public bool isPointerUp = false;

    private PlayerController playerController;
    private MeshRenderer playerMesh;
    private enum MenuAction : int
    {
        open = 1,
        close = 2,
        wait = 3
    }
    private bool isFirstRun = true;
    private bool isMenuOpen = false;
    private bool isReseting = false;
    private bool isExiting = false;
    private bool isGameOver = false;
    private bool hasOptionsChanged = false;
    private ConfirmDialog confirmDialog;

    private Color defaultColor = new Color32(255, 207, 0, 255); // FFCF00 yellow
    private Color offColor = new Color32(0, 0, 0, 255); // 000000 black
    private Color victoryColor = new Color32(0, 255, 64, 255); // 00FF40 green
    private Color failColor = new Color32(255, 0, 0, 255); // FF0000 red
    private Color buttonOnColor = new Color32(255, 255, 255, 255); // FF0000 white
    private Color buttonOffColor = new Color32(171, 171, 171, 255); // ABABAB gray

    void Start()
    {
        // Menu Initialization
        startButton.onClick.AddListener(StartGame);
        instructionsButton.onClick.AddListener(Instructions);
        quitButton.onClick.AddListener(QuitGame);
        returnButton.onClick.AddListener(Instructions);
        keyboardToggle.onValueChanged.AddListener(ToggleKeyboard);
        mouseToggle.onValueChanged.AddListener(ToggleMouse);
        musicSlider.onValueChanged.AddListener(ToggleMusic);
        audioSlider.onValueChanged.AddListener(ToggleAudio);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        saveButton.onClick.AddListener(SaveMenuOptions);
        confirmDialog = GameObject.Find("ConfirmDialog").GetComponent<ConfirmDialog>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerMesh = player.GetComponent<MeshRenderer>();

        TitleScreen();
    }

    void Update()
    {
        if (isActionOn) {
            GetSpecialActions(); // Catch menu+dialog keys. See note inside.
            if (isReseting) RestartGame();
            if (isExiting) ExitGame();
            if (isMenuOpen) Menu(MenuAction.wait); // Must be the last one: receive all y/n/esc flags unused
        }
    }

    public void GetSpecialActions()
    {
        // ESC key for both cancel (multiple!) confirmation dialog AND open+close menu is a nightmare. Resolved!
        if (confirmDialog.isConfirmDialogOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) confirmDialog.Close();
        }
        else {
            if (isMenuOpen) {
                if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver) Menu(MenuAction.close);
            }
            else {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
                {
                    if (isActionOn) Menu(MenuAction.open);
                }
                if (Input.GetKeyDown(KeyCode.F1)) // Toggle Keyboard on/off
                {
                    inputKeyboard = !inputKeyboard;
                    iconKeyboard.color = inputKeyboard ? defaultColor : offColor;
                    PlayerPrefs.SetInt("Input Keyboard", (inputKeyboard ? 1 : 0));
                }
                if (Input.GetKeyDown(KeyCode.F2)) // Toggle Mouse on/off
                {
                    inputMouse = !inputMouse;
                    iconMouse.color = inputMouse ? defaultColor : offColor;
                    PlayerPrefs.SetInt("Input Mouse", (inputMouse ? 1 : 0));
                }
            }
        }
    }

    void TitleScreen()
    {
        playerMesh.gameObject.SetActive(false);
        isActionOn = false;
        tittlePanel.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void StartGame()
    {
        levelText.gameObject.SetActive(true);
        iconKeyboard.gameObject.SetActive(true);
        iconMouse.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        hiScoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        tittlePanel.gameObject.SetActive(false);
        ReadMenuOptions();
        ReadStatistics();
        if (isFirstRun)
        {
            isFirstRun = false;
            SaveMenuOptions(); // if no values saved to SO, save default values
            SaveStatistics();
        }
        musicSource.Play();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMesh.gameObject.SetActive(true);
        NewLevel(true, levelSecondsTotal); // New Game
    }

    void Instructions()
    {
        instructionsPanel.gameObject.SetActive(!instructionsPanel.gameObject.activeSelf);
    }

    void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void NewLevel(bool resetGame, int newLevelTime)
    {
        isActionOn = false;
        if (resetGame)
        {
            level = 0;
            score = 0;
            livesLeft = livesTotal;
            levelText.color = defaultColor;
            scoreText.color = defaultColor;
            hiScoreText.color = defaultColor;
            hiScoreText.text = "high " + hiScore.ToString();
        } else {
            histLevels++;
        }
        level++;
        levelText.text = "level " + level.ToString("D2");

        StartCoroutine(TimeMgm(newLevelTime));
        ScoreUpdate(0);
        LivesUpdate(0);
    }

    IEnumerator ReadyCountDown(string baseMsg)
    {
        isActionOn = false;
        playerController.transform.position = Vector3.up * 15;
        playerController.playerRb.velocity = Vector3.zero;
        playerController.playerRb.angularVelocity = Vector3.zero;
        playerController.playerRb.useGravity = false;

        messageText.text = baseMsg + "ready!" + "\n";
        messagePanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        for (int i = 3; i > 0; i--)
        {
            messageText.text = baseMsg + "ready!" + "\n" + i;
            audioSource.PlayOneShot(beepSound, audioVolume);
            yield return new WaitForSeconds(1f);
        }
        messageText.text = baseMsg + "ready!" + "\n" + "go!";
        yield return new WaitForSeconds(1f);

        // return the ball to field if not gameover yet
        playerController.playerRb.useGravity = true;
        playerController.playerRb.AddForce(Vector3.down * 50, ForceMode.VelocityChange);
        // Activate settings to unleash a powerup explosion when player spawns
        playerController.hasPowerupExplode = true;
        playerController.isOnGround = false;
        playerController.isExploding = true;

        messagePanel.gameObject.SetActive(false);
        isActionOn = true;
    }

    public void LivesUpdate(int livesUpdate)
    {
        livesLeft += livesUpdate;
        livesText.text = "lives " + livesLeft.ToString();
        livesText.color = defaultColor;

        if (livesLeft == 0)
        {
            livesText.color = failColor;
            isGameOver = true;
            Menu(MenuAction.open); // Call endgame + summary + save statistics + restart
        }
        else if (livesUpdate == -1)
        {
            histLivesLost += -livesUpdate;
            StartCoroutine(ReadyCountDown("reinforKements arrive. keep fighting." + "\n"));
        }
        else if (livesUpdate == 0)
        {
            if (level == 1) StartCoroutine(ReadyCountDown("level 01" + "\n"));
            else StartCoroutine(ReadyCountDown("new level " + level.ToString("D2") + "\n"));
        }
    }

    public void ScoreUpdate(Scores scoreUpdate)
    {

        int scoreOld = score;
        score += (int)scoreUpdate;
        histScore += (int)scoreUpdate;
        scoreText.text = "sKore " + score.ToString(); // Weird font used: no lowercase, "c" is the same as "e", "K" (capital) is similar to real "c"

        if (scoreUpdate == Scores.scoreEnemySmall)
        {
            beatenEnemySmall++;
            histBeatenEnemySmall++;
        }
        else if (scoreUpdate == Scores.scoreEnemyBig)
        {
            beatenEnemyBig++;
            histBeatenEnemyBig++;
        }
        else if (scoreUpdate == Scores.scoreCannonBall)
        {
            beatenEnemyCannonBall++;
            histBeatenEnemyCannonBall++;
        }
        else if (scoreUpdate == Scores.scorePowerupPush)
        {
            gottenPowerupPush++;
            histGottenPowerupPush++;
        }
        else if (scoreUpdate == Scores.scorePowerupExplode)
        {
            gottenPowerupExplode++;
            histGottenPowerupExplode++;
        }

        if (score > hiScore) {
            if (!hasHiScore) {
                hasHiScore = true;
                if (hiScore > 0) scoreText.color = victoryColor;
            }
            hiScore = score;
        }
        if (score / livesAtPoints > scoreOld / livesAtPoints) { // +1 life after 20000 points
            histLivesExtra++;
            LivesUpdate(1);
        }
    }

    IEnumerator TimeMgm(int timeSeconds)
    {
        timeText.color = defaultColor;

        if (timeSeconds > 0)
        {
            while (timeSeconds >= 0)
            {
                timeText.text = "time " + (timeSeconds / 60).ToString("D2") + ":" + (timeSeconds % 60).ToString("D2");
                if (timeSeconds <= 10) timeText.color = victoryColor;
                yield return new WaitForSeconds(1f);
                if (isActionOn) {
                    timeSeconds--;
                    histSeconds++;
                    levelSecondsLeft = timeSeconds;
                    ScoreUpdate(Control.Scores.scoreSecond);
                }
            }
        }
        levelSecondsLeft = 0;
        timeText.color = victoryColor;
        timeText.text = "time 00:00";
        NewLevel(false, levelSecondsTotal); // New Level
    }

    void Menu(MenuAction action)
    {
        if (action == MenuAction.open && !isMenuOpen)
        {
            isMenuOpen = true;
            if (isGameOver) menuPanelText.text = "game over";
            else menuPanelText.text = "paused";
            menuPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            ReadMenuOptions(); // Read values from SO
            SaveStatistics();
            ShowStatistics();
        }
        if (action == MenuAction.close) // If menu opened, closes it (upd: 1st check changes + confirm dialog)
        {
            if (hasOptionsChanged && !confirmDialog.isConfirmDialogOn) // Ask for save changes before exit
            {
                if (!confirmDialog.isYesSelected && !confirmDialog.isNoSelected) {
                    confirmDialog.ConfirmationDialog("save Khanges?");
                }
            }
            else {
                if (!isGameOver) {
                    isMenuOpen = false;
                    menuPanel.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f;
                }
            }
        }
        if (action == MenuAction.wait)
        {
            if (confirmDialog.isYesSelected || confirmDialog.isNoSelected) {
                if (confirmDialog.isYesSelected)  SaveMenuOptions(); // If confirms, save and apply changes globally
                if (confirmDialog.isNoSelected)   ReadMenuOptions(); // If cancel changes, read again from SO
                // isDialogOpen = false;
                confirmDialog.Close();
                Menu(MenuAction.close);
            }
        }
    }

    void ReadMenuOptions()
    {
        isFirstRun = (PlayerPrefs.GetInt("First Run", (isFirstRun ? 1 : 0)) != 0);
        inputKeyboard = (PlayerPrefs.GetInt("Input Keyboard", (inputKeyboard ? 1 : 0)) != 0);
        inputMouse = (PlayerPrefs.GetInt("Input Mouse", (inputMouse ? 1 : 0)) != 0);
        musicVolume = PlayerPrefs.GetFloat("Music Volume", musicVolume);
        audioVolume = PlayerPrefs.GetFloat("Audio Volume", audioVolume);
        // Apply values to menu
        keyboardToggle.isOn = inputKeyboard;
        mouseToggle.isOn = inputMouse;
        musicIconOn.enabled = (musicVolume > 0);
        musicIconOff.enabled = (musicVolume == 0);
        audioIconOn.enabled = (audioVolume > 0);
        audioIconOff.enabled = (audioVolume == 0);
        musicSlider.value = musicVolume;
        audioSlider.value = audioVolume;
        // Apply values to game
        iconKeyboard.color = inputKeyboard ? defaultColor : offColor;
        iconMouse.color = inputMouse ? defaultColor : offColor;
        // Reset changes flag
        SetChangedStatus(false);
    }

    void SaveMenuOptions()
    {
        PlayerPrefs.SetInt("First Run", (isFirstRun ? 1 : 0));
        PlayerPrefs.SetInt("Input Keyboard", (inputKeyboard ? 1 : 0));
        PlayerPrefs.SetInt("Input Mouse", (inputMouse ? 1 : 0));
        PlayerPrefs.SetFloat("Music Volume", musicVolume);
        PlayerPrefs.SetFloat("Audio Volume", audioVolume);
        iconKeyboard.color = inputKeyboard ? defaultColor : offColor;
        iconMouse.color = inputMouse ? defaultColor : offColor;
        SetChangedStatus(false);
    }

    void ReadStatistics()
    {
        hiScore = PlayerPrefs.GetInt("High Score", 0);
        histScore = PlayerPrefs.GetInt("Total Score", 0);
        histLevels = PlayerPrefs.GetInt("Total Levels Passed", 0);
        histSeconds = PlayerPrefs.GetInt("Total Time Survived", 0);
        histLivesLost = PlayerPrefs.GetInt("Total Lives Lost", 0);
        histLivesExtra = PlayerPrefs.GetInt("Total Extra Lives", 0);
        histBeatenEnemySmall = PlayerPrefs.GetInt("Total Beaten Enemy Small", 0);
        histBeatenEnemyBig = PlayerPrefs.GetInt("Total Beaten Enemy Big", 0);
        histBeatenEnemyCannonBall = PlayerPrefs.GetInt("Total Beaten Enemy Cannon Ball", 0);
        histGottenPowerupPush = PlayerPrefs.GetInt("Total Collected Powerup Push", 0);
        histGottenPowerupExplode = PlayerPrefs.GetInt("Total Collected Powerup Explode", 0);
    }

    void SaveStatistics()
    {
        PlayerPrefs.SetInt("High Score", hiScore);
        PlayerPrefs.SetInt("Total Score", histScore);
        PlayerPrefs.SetInt("Total Levels Passed", histLevels);
        PlayerPrefs.SetInt("Total Time Survived", histSeconds);
        PlayerPrefs.SetInt("Total Lives Lost", histLivesLost);
        PlayerPrefs.SetInt("Total Extra Lives", histLivesExtra);
        PlayerPrefs.SetInt("Total Beaten Enemy Small", histBeatenEnemySmall);
        PlayerPrefs.SetInt("Total Beaten Enemy Big", histBeatenEnemyBig);
        PlayerPrefs.SetInt("Total Beaten Enemy Cannon Ball", histBeatenEnemyCannonBall);
        PlayerPrefs.SetInt("Total Collected Powerup Push", histGottenPowerupPush);
        PlayerPrefs.SetInt("Total Collected Powerup Explode", histGottenPowerupExplode);
    }

    void ShowStatistics()
    {
        int days = histSeconds / (24 * 3600);
        int secs = histSeconds % (24 * 3600);
        int hours = secs / 3600;
        secs %= 3600;
        int minutes = secs / 60;
        secs %= 60;
        int seconds = secs;

        statsText.text = "sKore: " + score + "\t\t" + "high sKore: " + hiScore + "\t\t" + "total sKore: " + histScore;
        statsText.text += "\n" + "total levels passed: " + histLevels;
        statsText.text += "\n" + "total time survived: " + days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seKonds";
        statsText.text += "\n" + "total lives lost: " + histLivesLost + "\t\t\t" + "total eXtra lives: " + histLivesExtra;
        //statsText.text += "\n" + "______________________________________________________________________";
        statsText.text += "\n" + "beaten enemy small:" + "\t\t\t\t" + "(session) " + beatenEnemySmall + "\t\t" + "(total) " + histBeatenEnemySmall;
        statsText.text += "\n" + "beaten enemy big:" + "\t\t\t\t" + "(session) " + beatenEnemyBig + "\t\t" + "(total) " + histBeatenEnemyBig;
        statsText.text += "\n" + "beaten enemy Kannon ball:" + "\t\t" + "(session) " + beatenEnemyCannonBall + "\t\t" + "(total) " + histBeatenEnemyCannonBall;
        //statsText.text += "\n" + "______________________________________________________________________";
        statsText.text += "\n" + "KolleKted powerup super push:" + "\t" + "(session) " + gottenPowerupPush + "\t\t" + "(total) " + histGottenPowerupPush;
        statsText.text += "\n" + "KolleKted powerup Kharged bomb:" + "\t" + "(session) " + gottenPowerupExplode + "\t\t" + "(total) " + histGottenPowerupExplode;
    }

    void SetChangedStatus(bool isChanged)
    {
        hasOptionsChanged = isChanged;
        saveButton.interactable = isChanged;
        saveButtonText.color = isChanged ? buttonOnColor : buttonOffColor;
    }

    void ToggleKeyboard(bool isOn)
    {
        inputKeyboard = isOn;
        SetChangedStatus(true);
    }

    void ToggleMouse(bool isOn)
    {
        inputMouse = isOn;
        SetChangedStatus(true);
    }

    void ToggleMusic(float value)
    {
        musicVolume = value;
        musicIconOn.enabled = (musicVolume > 0);
        musicIconOff.enabled = (musicVolume == 0);
        musicSource.volume = musicVolume;
        SetChangedStatus(true);
    }

    void ToggleAudio(float value)
    {
        audioVolume = value;
        audioIconOn.enabled = (audioVolume > 0);
        audioIconOff.enabled = (audioVolume == 0);
        if (audioVolume > 0 && isPointerUp)
        {
            audioSource.PlayOneShot(audioSample, audioVolume); // Plays a sound for feedback
            isPointerUp = false;
        }
        SetChangedStatus(true);
    }

    public void SliderHandleClickRelease()
    {
        isPointerUp = true;
    }

    private void RestartGame()
    {
        if (isReseting) // wait for cancel or confirmation
        {
            if (confirmDialog.isNoSelected || !confirmDialog.isConfirmDialogOn)
            { // cancel
                isReseting = false;
                confirmDialog.Close();
            }
            if (confirmDialog.isYesSelected)
            {
                Time.timeScale = 1f;
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            isReseting = true;
            confirmDialog.ConfirmationDialog("restart game?");
        }
    }

    private void ExitGame()
    {
        if (isExiting) // wait for cancel or confirmation
        {
            if (confirmDialog.isNoSelected || !confirmDialog.isConfirmDialogOn)
            { // cancel
                isExiting = false;
                confirmDialog.Close();
            }
            if (confirmDialog.isYesSelected)
            {
                Debug.Log("Exit Game");
                Application.Quit();
            }
        }
        else
        {
            isExiting = true;
            confirmDialog.ConfirmationDialog("eXit game?");
        }
    }

}
