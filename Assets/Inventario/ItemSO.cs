using UnityEngine;


[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange;
    public int statChangeAmount;
    public StatToChange stat2ToChange;
    public float stat2ChangeAmount;
    public AudioClip useSound; // 👈 arrasta o som aqui no Inspector

    public void UseItem(AudioSource audioSource) // 👈 recebe o AudioSource
    {
        if (useSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(useSound); // 👈 PlayOneShot é mais fiável
        }

        if (statToChange == StatToChange.Health)
            FindFirstObjectByType<Vida>().ChangeHealth(statChangeAmount);
        else if (statToChange == StatToChange.Stamina)
            FindFirstObjectByType<StaminaController>().ChangeStamina(statChangeAmount);
    }

    public enum StatToChange { Health, Stamina }
}