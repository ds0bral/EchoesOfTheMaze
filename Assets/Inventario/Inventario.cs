using UnityEngine;
using UnityEngine.InputSystem;

public class Inventario : MonoBehaviour
{
    [SerializeField] GameObject panelInventario;
    private bool inventarioAberto;
    public itemSlot[] itemSlot;
    PlayerControols controls;
    [SerializeField]public ItemSO[] itensSO; // ItemSO com I maiúsculo
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        controls = new PlayerControols();
    }

    void OnEnable()
    {
        if (controls != null)
            controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Gameplay.Disable();
    }

    void Update()
    {
        if (controls.Gameplay.AbreINV.triggered && inventarioAberto)
        {
            Time.timeScale = 1f;
            panelInventario.SetActive(false);
            inventarioAberto = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (controls.Gameplay.AbreINV.triggered && !inventarioAberto)
        {
            Time.timeScale = 0f;
            panelInventario.SetActive(true);
            inventarioAberto = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UseItem(string itemName)
    {
        bool existe = false;
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName && itemSlot[i].quantity > 0)
            {
                existe = true;
                break;
            }
        }
        if (existe)
        {
            for (int i = 0; i < itensSO.Length; i++)
            {
                if (itensSO[i].itemName == itemName)
                {
                    itensSO[i].UseItem(audioSource); // 👈 passa o audioSource
                    RemoveItem(itemName, 1);
                    break;
                }
            }
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].quantity -= quantity;

                if (itemSlot[i].quantity <= 0)
                {
                    itemSlot[i].quantity = 0;
                    itemSlot[i].ClearSlot(); // limpa o slot completamente
                }
                else
                {
                    itemSlot[i].QuantityText.text = itemSlot[i].quantity.ToString();
                }
                return;
            }
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemIcon, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].quantity += quantity;
                itemSlot[i].QuantityText.text = itemSlot[i].quantity.ToString();
                return;
            }
        }

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (string.IsNullOrEmpty(itemSlot[i].itemName))
            {
                itemSlot[i].AddItem(itemName, quantity, itemIcon, itemDescription);
                return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].isSelected = false;
        }
    }
}