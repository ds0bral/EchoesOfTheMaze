using UnityEngine;
using UnityEngine.InputSystem;

public class Personagem : MonoBehaviour
{
    public bool EstaACorrer => controleJogador.actions["Correr"].IsPressed();
    public Vector2 Movimento => movimento;

    [SerializeField] float velocidadeAndar = 4f;
    [SerializeField] float velocidadeCorrer = 9f;
    [HideInInspector] public StaminaController _staminaController;

    Vector2 movimento;
    Vector3 direcao;
    float velocidadeAtual;

    PlayerInput controleJogador;
    CharacterController controlarOPersonagem;

    void Start()
    {
        controlarOPersonagem = GetComponent<CharacterController>();
        controleJogador = GetComponent<PlayerInput>();
        _staminaController = GetComponent<StaminaController>();
        velocidadeAtual = velocidadeAndar;
    }

    public void SetRunSpeed(float speed)
    {
        velocidadeAtual = speed;
    }

    void Update()
    {
        movimento = controleJogador.actions["Move"].ReadValue<Vector2>();
        direcao = transform.forward * movimento.y + transform.right * movimento.x;

        bool estaACorrer = controleJogador.actions["Correr"].IsPressed();

        if (estaACorrer && controlarOPersonagem.velocity.sqrMagnitude > 0f)
        {
            // Está a tentar correr
            if (_staminaController.hasRegenerated)
            {
                _staminaController.weAreSprinting = true;
                _staminaController.Sprinting();
                velocidadeAtual = velocidadeCorrer;
            }
            else
            {
                // Sem stamina, anda devagar
                _staminaController.weAreSprinting = false;
                velocidadeAtual = velocidadeAndar;
            }
        }
        else
        {
            // Não está a correr
            _staminaController.weAreSprinting = false;
            velocidadeAtual = velocidadeAndar;
        }

        controlarOPersonagem.Move(direcao * velocidadeAtual * Time.deltaTime);
    }
}