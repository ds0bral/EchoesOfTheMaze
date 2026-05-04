using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Parameters")]
    [SerializeField] private int slowedRunSpeed = 4;
    [SerializeField] private int normalRunSpeed = 8;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    [Header("Som Cansaço")]
    [SerializeField] private AudioClip somCansado;
    [SerializeField] private AudioSource audioSourceCansado;

    private Personagem playerController;

    private void Start()
    {
        playerController = GetComponent<Personagem>();

        if (audioSourceCansado == null)
            audioSourceCansado = gameObject.AddComponent<AudioSource>();

        audioSourceCansado.clip = null; // não atribui o clip logo
        audioSourceCansado.loop = true;
        audioSourceCansado.playOnAwake = false;
        audioSourceCansado.Stop(); // garante que não toca nada
    }

    private void Update()
    {
        if (!weAreSprinting)
        {
            if (playerStamina < maxStamina)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    playerController.SetRunSpeed(normalRunSpeed);
                    sliderCanvasGroup.alpha = 0f;
                    hasRegenerated = true;
                    PararSomCansado(); // stamina cheia, para o som
                }
            }
            else if (!hasRegenerated)
            {
                hasRegenerated = true;
                playerController.SetRunSpeed(normalRunSpeed);
                sliderCanvasGroup.alpha = 0f;
                PararSomCansado();
            }
        }
    }

    private void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0)
            sliderCanvasGroup.alpha = 0f;
        else
            sliderCanvasGroup.alpha = 1f;
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                hasRegenerated = false;
                playerController.SetRunSpeed(slowedRunSpeed);
                sliderCanvasGroup.alpha = 0f;
                TocarSomCansado(); // stamina zerou, começa o som
            }
        }
        else
        {
            weAreSprinting = false;
            playerController.SetRunSpeed(normalRunSpeed);
            UpdateStamina(0);
        }
    }

    public void ChangeStamina(float valor)
    {
        maxStamina += valor;
        playerStamina = maxStamina;
        playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);
        UpdateStamina(1);
    }

    private void TocarSomCansado()
    {
        if (somCansado == null || audioSourceCansado == null) return;
        if (!audioSourceCansado.isPlaying)
        {
            audioSourceCansado.clip = somCansado;
            audioSourceCansado.Play();
        }
    }

    private void PararSomCansado()
    {
        if (audioSourceCansado != null && audioSourceCansado.isPlaying)
            audioSourceCansado.Stop();
    }
}