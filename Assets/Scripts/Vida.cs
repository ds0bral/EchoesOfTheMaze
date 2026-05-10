using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Script que implementa o mecanismo de vida de uma personagem
/// </summary>


public class Vida : MonoBehaviour
{
    public int maxVida = 100; // Indica a vida com que a personagem começa o nível
    public int vidaAtual = 0;
    public bool isDead = false;
    public event Action OnMorreu;
    public event Action<int> OnPerdeuVida; // Evento para notificar mudanças na vida

    // Audio
    AudioSource source;
    [SerializeField] AudioClip[] clip;
    float ultimoSom = 0f;
    [SerializeField] float cooldown = 0.15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaAtual = maxVida;
        AtualizaVida(); // Notificar os ouvintes sobre a vida inicial
        source = GetComponent<AudioSource>();
        if (source == null)
            source.gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        source.spatialBlend = 1;
    }

    void TocarSom(AudioClip[] clips)
    {
        if (source == null || clips == null || clips.Length == 0) return;
        if (Time.time - ultimoSom < cooldown) return;

        ultimoSom = Time.time;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    // Função que retira vida da personagem e deteta se morreu
    public void perdeVida(int valor)
    {
        vidaAtual -= valor;
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            if (!isDead) // evita chamar duas vezes
            {
                isDead = true;
                OnMorreu?.Invoke();
            }
        }
        AtualizaVida();
        TocarSom(clip);
    }
    //Função que adiciona vida á personagem respeitando o maxVida
    public void ganhaVida(int valor)
    {
        vidaAtual += valor;
        if (vidaAtual > maxVida)
            vidaAtual = maxVida;
        AtualizaVida();
    }

    public void AtualizaVida()
    {
        if (OnPerdeuVida != null && gameObject.tag.Equals("Player"))
            OnPerdeuVida(vidaAtual);

    }

    public void ChangeHealth(int valor)
    {
        if (valor > 0)
            ganhaVida(valor);      // ganhaVida já faz: if (vidaAtual > maxVida) vidaAtual = maxVida;
        else
            perdeVida(Mathf.Abs(valor));
    }
}
