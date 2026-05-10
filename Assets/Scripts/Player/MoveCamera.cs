using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    GameObject player;
    PlayerControols controls;

    [Header("Camera Settings")]
    float sensibilidade = 0.3f;
    [SerializeField] float suavizacao = 8f;

    Vector2 rotacaoCamera;
    Vector2 rotacaoAlvo;
    bool _ignorarInput = false;
    bool invertY = false;

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

        sensibilidade = PlayerPrefs.GetFloat("masterSen", 0.3f);
        invertY = PlayerPrefs.GetInt("masterInvertY", 0) == 1;
    }

    void LateUpdate()
    {
    if (Time.timeScale == 0f) return;

        if (_ignorarInput)
        {
            _ignorarInput = false;
            return;
        }

        Vector2 lookInput = controls.Gameplay.mouse.ReadValue<Vector2>();

        rotacaoAlvo.x += lookInput.x * sensibilidade;
        rotacaoAlvo.y += (invertY ? lookInput.y : -lookInput.y) * sensibilidade; // ← invertY aplicado
        rotacaoAlvo.y = Mathf.Clamp(rotacaoAlvo.y, -60f, 60f);

        rotacaoCamera = Vector2.Lerp(rotacaoCamera, rotacaoAlvo, suavizacao * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(rotacaoCamera.y, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, rotacaoCamera.x, 0f);
    }

    public void AtualizarDefinicoes() // ← novo
    {
        sensibilidade = PlayerPrefs.GetFloat("masterSen", 0.3f);
        invertY = PlayerPrefs.GetInt("masterInvertY", 0) == 1;
    }

    public void SincronizarRotacao()
    {
        rotacaoAlvo = rotacaoCamera;
    }

    public void IgnorarInputPorUmFrame()
    {
        _ignorarInput = true;
    }
}