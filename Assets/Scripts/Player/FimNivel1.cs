using UnityEngine;

public class FimNivel1 : MonoBehaviour
{   
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<TelaDerrota>().MostrarTelaVitoria();
        }
    }
}
