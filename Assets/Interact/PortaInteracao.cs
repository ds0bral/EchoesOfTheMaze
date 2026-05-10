using UnityEngine;

public class PortaInteracao : MonoBehaviour, IInteract, IInteractMessage
{
    [SerializeField] private KeypadManager keypad;

    public void Acao()
    {
        keypad.AbrirPainel();
    }

    public string Mensagem()
    {
        return "Prima E para interagir com a porta";
    }
}