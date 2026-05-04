using UnityEngine;


[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int statChangeAmount;

    public void UseItem()
    {
        if (statToChange == StatToChange.Health)
        {
            FindFirstObjectByType<Vida>().ChangeHealth(statChangeAmount);
        }
        else if (statToChange == StatToChange.Stamina)
        {
            FindFirstObjectByType<StaminaController>().ChangeStamina(statChangeAmount);
        }
    }


    public enum StatToChange
    {
        Health,
        Stamina,
    }

      
}
