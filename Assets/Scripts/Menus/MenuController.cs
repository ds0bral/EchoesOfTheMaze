using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public AudioSource selectSound;
    [SerializeField] private string NomeCena;
    [SerializeField] private GameObject painelMenu;
    [SerializeField] private GameObject painelOpcoes;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Gameplay Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private float defaultValue = 0.3f;
    public float MainControllerSen = 0.3f;

    [Header("Toggle Setting")]
    [SerializeField] private Toggle InvertY = null;
    [SerializeField] private Toggle fullscreenToggle = null;
    [SerializeField] private Toggle fpsToggle = null;
    [SerializeField] private GameObject fpsDisplay = null;
    private bool _isFullScreen;

    private bool activated = false;

    [Header("Resolution DropDown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("masterSen", MainControllerSen);
        PlayerPrefs.SetInt("masterInvertY", InvertY.isOn ? 1 : 0);
        PlayerPrefs.SetInt("masterFullscreen", _isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("masterResolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("masterFPS", fpsToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        Screen.fullScreenMode = _isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        SetResolution(resolutionDropdown.value);
        fpsDisplay.SetActive(fpsToggle.isOn);

        FindObjectOfType<MoveCamera>()?.AtualizarDefinicoes();
        StartCoroutine(ConfirmationBox());
    }

    void Start()
    {

        // Volume
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeTextValue.text = savedVolume.ToString("0.0");

        // Sensibilidade
        float savedSen = PlayerPrefs.GetFloat("masterSen", defaultValue);
        MainControllerSen = savedSen;
        ControllerSenSlider.value = savedSen;
        controllerSenTextValue.text = savedSen.ToString("0.0");

        // InvertY
        InvertY.isOn = PlayerPrefs.GetInt("masterInvertY", 0) == 1;

        if (confirmationPrompt != null)
            confirmationPrompt.SetActive(false);

        // Resoluções
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        List<Resolution> filteredResolutions = new List<Resolution>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
                filteredResolutions.Add(resolutions[i]);
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                    currentResolutionIndex = filteredResolutions.Count - 1;
            }
        }
        resolutions = filteredResolutions.ToArray();
        resolutionDropdown.AddOptions(options);
        int savedResolution = PlayerPrefs.GetInt("masterResolution", currentResolutionIndex);
        resolutionDropdown.value = savedResolution;
        resolutionDropdown.RefreshShownValue();

        // Fullscreen
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        _isFullScreen = PlayerPrefs.GetInt("masterFullscreen", 1) == 1;
        Screen.fullScreenMode = _isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        fullscreenToggle.isOn = _isFullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);

        // FPS Display
        bool savedFPS = PlayerPrefs.GetInt("masterFPS", 0) == 1;
        fpsToggle.isOn = savedFPS;
        fpsDisplay.SetActive(savedFPS);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        FullScreenMode mode = _isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolution.width, resolution.height, mode);
    }

    void Update()
    {
        if (activated) return;

        bool keyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (keyPressed || mouseClicked)
        {
            if (selectSound != null)
                selectSound.Play();

            menuPanel.SetActive(true);
            activated = true;
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void SetControllerSen(float sensitivy)
    {
        MainControllerSen = sensitivy;
        controllerSenTextValue.text = sensitivy.ToString("0.0");
    }

    public void SetFullScreen(bool IsFullScreen)
    {
        _isFullScreen = IsFullScreen;
    }

    public void resetButton()
    {
        // Volume
        AudioListener.volume = defaultVolume;
        volumeSlider.value = defaultVolume;
        volumeTextValue.text = defaultVolume.ToString("0.0");

        // Sensibilidade
        MainControllerSen = defaultValue;
        ControllerSenSlider.value = defaultValue;
        controllerSenTextValue.text = defaultValue.ToString("0.0");

        // InvertY
        InvertY.isOn = false;

        // Fullscreen
        _isFullScreen = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = true;

        // FPS
        fpsToggle.isOn = false;
        fpsDisplay.SetActive(false);

        // Resolução nativa do monitor
        Resolution nativeRes = Screen.resolutions[Screen.resolutions.Length - 1];
        Screen.SetResolution(nativeRes.width, nativeRes.height, FullScreenMode.FullScreenWindow);

        string nativeOption = nativeRes.width + "x" + nativeRes.height;
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == nativeOption)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
        resolutionDropdown.RefreshShownValue();
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2f);
        confirmationPrompt.SetActive(false);
    }

    public void AbrirMenuJogo()
    {
        Debug.Log("Abrindo menu do jogo...");
    }

    public void Jogar()
    {
        SceneManager.LoadScene(NomeCena);
    }

    public void AbrirOpcoes()
    {
        painelMenu.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelMenu.SetActive(true);
        painelOpcoes.SetActive(false);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}