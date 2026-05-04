using UnityEngine;
using TMPro;

public class HudVida : MonoBehaviour
{
    TMP_Text txt_vida;

    void Awake()  // <-- mudança aqui
    {
        txt_vida = GetComponent<TMP_Text>();

        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.LogError("Player não encontrado!");
        else if (player.GetComponent<Vida>() == null)
            Debug.LogError("Vida não encontrado no Player!");
        else if (txt_vida == null)
            Debug.LogError("TMP_Text não encontrado!");
        else
            player.GetComponent<Vida>().OnPerdeuVida += HudVida_OnPerdeuVida;
    }

    private void HudVida_OnPerdeuVida(int obj)
    {
        txt_vida.text = obj.ToString();
    }

    void Update() 
    { 
        
    }
}
