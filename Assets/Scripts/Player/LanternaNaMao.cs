using UnityEngine;
using System.Collections;

public class LanternaMao : MonoBehaviour
{
    PlayerControols controls;
    Light luzLanterna;
    bool lanternaAtiva = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip somLigar;
    [SerializeField] AudioClip somDesligar;

    void Awake()
    {
        controls = new PlayerControols();
        controls.Gameplay.Enable();
    }

    void OnEnable()  { controls?.Gameplay.Enable(); }
    void OnDisable() { controls?.Gameplay.Disable(); }

    void Start()
    {
        luzLanterna = GetComponentInChildren<Light>();

        if (luzLanterna != null)
            luzLanterna.enabled = false;

        SetVisivel(false);
    }

    void Update()
    {
        if (controls.Gameplay.Flashlight.WasPressedThisFrame())
        {
            lanternaAtiva = !lanternaAtiva;
            StartCoroutine(AtivarLanterna(lanternaAtiva));
        }
    }

    IEnumerator AtivarLanterna(bool ativar)
    {
        // Toca o som primeiro
        if (ativar && somLigar != null)
        {
            audioSource.PlayOneShot(somLigar);
            yield return new WaitForSeconds(somLigar.length); // espera o som acabar
        }
        else if (!ativar && somDesligar != null)
        {
            audioSource.PlayOneShot(somDesligar);
            yield return new WaitForSeconds(somDesligar.length); // espera o som acabar
        }

        // Só depois liga/desliga a luz e o modelo
        if (luzLanterna != null)
            luzLanterna.enabled = ativar;

        SetVisivel(ativar);
    }

    void SetVisivel(bool visivel)
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = visivel;
    }
}