using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    GameObject player;
    PlayerControols controls;

    [Header("Camera Settings")]
    [SerializeField] float sensibilidade = 0.3f;
    [SerializeField] float suavizacao = 8f;

    Vector2 rotacaoCamera;
    Vector2 rotacaoAlvo;
    bool _ignorarInput = false; // ← ADD THIS

    void Awake()
    {
        controls = new PlayerControols();
        controls.Gameplay.Enable();
    }

    void OnEnable()
    {
        if (controls != null)
            controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Gameplay.Disable();
    }

    void Start()
    {
        player = transform.parent.gameObject;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f) return; // Pausado: sai sem tocar em nada

        if (_ignorarInput)
        {
            _ignorarInput = false;
            return;
        }

        Vector2 lookInput = controls.Gameplay.mouse.ReadValue<Vector2>();

        rotacaoAlvo.x += lookInput.x * sensibilidade;
        rotacaoAlvo.y -= lookInput.y * sensibilidade;
        rotacaoAlvo.y = Mathf.Clamp(rotacaoAlvo.y, -60f, 60f);

        rotacaoCamera = Vector2.Lerp(rotacaoCamera, rotacaoAlvo, suavizacao * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(rotacaoCamera.y, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, rotacaoCamera.x, 0f);
    }

    public void SincronizarRotacao()
    {
        rotacaoAlvo = rotacaoCamera;
    }

    public void IgnorarInputPorUmFrame() // ← ADD THIS
    {
        _ignorarInput = true;
    }
}