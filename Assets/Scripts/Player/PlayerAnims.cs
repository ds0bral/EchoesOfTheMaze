using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    Animator _animator;
    Personagem _playerMovement;
    SomAndar _somAndar;
    readonly int _velocidadeHash = Animator.StringToHash("velocidade");

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<Personagem>();
        _somAndar = GetComponentInChildren<SomAndar>();
    }

    void Update()
    {
        if (_animator == null || _playerMovement == null) return;

        float velocidade = CalcularVelocidadeAnim();
        _animator.SetFloat(_velocidadeHash, velocidade, 0.1f, Time.deltaTime);

        // Para o som quando está parado
        //if (velocidade == 0f && _somAndar != null)
            //_somAndar.PararSom();
    }

    float CalcularVelocidadeAnim()
    {
        Vector2 mov = _playerMovement.Movimento;

        if (mov.sqrMagnitude < 0.01f)
            return 0f; // idle

        if (mov.y < -0.1f)
            return -1f; // andar para trás

        if (_playerMovement.EstaACorrer)
            return 2f; // correr

        return 1f; // andar (frente, esquerda ou direita)
    }
}