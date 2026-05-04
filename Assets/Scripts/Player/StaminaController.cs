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

    private Personagem playerController;

    private void Start()
    {
        playerController = GetComponent<Personagem>();
    }

    private void Update()
    {
        if (!weAreSprinting)
        {
            if (playerStamina < maxStamina)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina); // garante que não passa do máximo
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    playerController.SetRunSpeed(normalRunSpeed);
                    sliderCanvasGroup.alpha = 0f;
                    hasRegenerated = true; // agora só ativa quando está mesmo cheio
                }
            }
            else if (!hasRegenerated) // segurança extra caso o Clamp já tenha igualado
            {
                hasRegenerated = true;
                playerController.SetRunSpeed(normalRunSpeed);
                sliderCanvasGroup.alpha = 0f;
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
        playerStamina = maxStamina; // opcional: enche a stamina ao aumentar o máximo
        playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);
        UpdateStamina(1);
    }
}