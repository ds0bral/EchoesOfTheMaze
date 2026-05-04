using UnityEngine;

public class Nota : MonoBehaviour, IInteract, ILerNota
{
    [SerializeField] GameObject panelNota; // arrasta o PanelNota aqui no Inspector

    void Awake()
    {
        panelNota.SetActive(false);
    }

    public void Acao()
    {
        // Se quiseres fazer algo com E, senão deixa vazio
    }

    public void LerNota()
    {
        bool estaAberto = panelNota.activeSelf;
        panelNota.SetActive(!estaAberto); // abre se fechado, fecha se aberto

        Time.timeScale = estaAberto ? 1f : 0f;
    }
}
