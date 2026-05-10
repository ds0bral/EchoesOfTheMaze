using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaDerrota : MonoBehaviour
{
    [SerializeField] private GameObject painelMorte;
    [SerializeField] private GameObject painelVitoria;
    [SerializeField] private int sceneMenuIndex = 0;

    void Start()
    {
        painelMorte.SetActive(false);
        if (painelVitoria != null)
            painelVitoria.SetActive(false);

        var vida = GameObject.FindWithTag("Player")?.GetComponent<Vida>();
        if (vida != null)
            vida.OnMorreu += MostrarTelaMorte;
    }

    void MostrarTelaMorte()
    {
        painelMorte.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MostrarTelaVitoria()
    {
        painelVitoria.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneMenuIndex);
    }

}