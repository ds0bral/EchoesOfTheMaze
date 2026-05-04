using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuJogo : MonoBehaviour
{

    public GameObject PanelMenuJogo;
    GameObject _player;
    PlayerControols controls;
    public GameObject cameraObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controls = new PlayerControols();
    }

    void OnEnable() { if (controls != null) controls.Gameplay.Enable(); }
    void OnDisable() { if (controls != null) controls.Gameplay.Disable(); }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");

        // Encontra a câmara filho do Player
        if (cameraObject == null)
            cameraObject = _player.transform.Find("Camera").gameObject;

    }

    void Update()
    {
        if (controls.Gameplay.Escape.triggered)
        {
            if (PanelMenuJogo.activeSelf)
                ContinuarJogo();
            else
                PausarJogo();
        }
    }

    public void ContinuarJogo()
    {
        PanelMenuJogo.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var cam = cameraObject.GetComponent<MoveCamera>();
        cam.SincronizarRotacao();
        cam.IgnorarInputPorUmFrame();
    }

    public void PausarJogo()
    {
        PanelMenuJogo.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

    public void Terminar()
    {
    }
}


