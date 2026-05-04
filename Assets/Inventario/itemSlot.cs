using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemSlot : MonoBehaviour,IPointerClickHandler
{
    //item data
    public string itemName;
    public int quantity;
    public Sprite itemIcon;
    public string itemDescription;
    
    
    //slot UI
    [SerializeField] public TMP_Text QuantityText; 
    [SerializeField] private Image IconImage;

    //Description slot

    public Image itemDescriptionIcon;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    public GameObject selectedShader;
    public bool isSelected;

    private Inventario inventario;

    private void Awake()
    {
        inventario = GameObject.Find("Player").GetComponent<Inventario>();
    }

    private void Start()
    {

    }

    public void AddItem(string name, int quantity, Sprite icon, string description)
    {
        this.itemName = name;
        this.quantity = quantity;
        this.itemIcon = icon;
        this.itemDescription = description;

        QuantityText.text = quantity.ToString();
        QuantityText.enabled = true; // Esconde o texto se a quantidade for 1
        IconImage.sprite = itemIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }


    public void OnLeftClick()
    {
        inventario.DeselectAllSlots();
        selectedShader.SetActive(true);
        isSelected = true;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionIcon.sprite = itemIcon;
    }

    public void UsarItem()
    {
        inventario.UseItem(itemDescriptionNameText.text);
    }
    
    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemIcon = null;
        itemDescription = "";

        QuantityText.text = "";
        QuantityText.enabled = false;
        IconImage.sprite = null;

        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionIcon.sprite = null;
        selectedShader.SetActive(false);
        isSelected = false;
    
    }
}
