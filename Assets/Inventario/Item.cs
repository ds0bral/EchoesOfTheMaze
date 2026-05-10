using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] public Sprite itemIcon;
    [TextArea] [SerializeField] private string itemDescription;
    [SerializeField] private AudioClip somApanhar;
    private Inventario inventario; 

    void Start()
    {
        inventario = GameObject.Find("Player").GetComponent<Inventario>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (somApanhar != null)
            {
                GameObject somObj = new GameObject("SomApanhar");
                AudioSource audio = somObj.AddComponent<AudioSource>();
                audio.ignoreListenerPause = true; // ignora o timeScale
                audio.clip = somApanhar;
                audio.Play();
                Destroy(somObj, somApanhar.length); // destroi depois do som acabar
            }

            inventario.AddItem(itemName, quantity, itemIcon, itemDescription);
            Destroy(gameObject);
        }
    }
}