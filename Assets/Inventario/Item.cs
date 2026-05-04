using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] public Sprite itemIcon;
    [TextArea] [SerializeField] private string itemDescription;
    private Inventario inventario; 

    void Start()
    {
        inventario = GameObject.Find("Player").GetComponent<Inventario>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventario.AddItem(itemName, quantity, itemIcon, itemDescription);
            Destroy(gameObject);
        }
    }
}