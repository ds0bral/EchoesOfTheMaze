using UnityEngine;
using TMPro;

/// <summary>
/// Coloca este script num GameObject vazio chamado "ColorCodeManager" no teu nível.
/// Arrasta os 4 TextMeshPro para os campos no Inspector.
/// </summary>
public class NumerosNivel : MonoBehaviour
{
    [Header("Text Mesh Pro - Números no Nível")]
    [Tooltip("TextMeshPro com a cor VERMELHA")]
    public TextMeshPro redNumberText;

    [Tooltip("TextMeshPro com a cor VERDE")]
    public TextMeshPro greenNumberText;

    [Tooltip("TextMeshPro com a cor AMARELA")]
    public TextMeshPro yellowNumberText;

    [Tooltip("TextMeshPro com a cor AZUL")]
    public TextMeshPro blueNumberText;

    // Os 4 números gerados (acessíveis por outros scripts)
    public static int RedCode   { get; private set; }
    public static int GreenCode { get; private set; }
    public static int YellowCode{ get; private set; }
    public static int BlueCode  { get; private set; }

    // Cores dos textos
    private readonly Color colorRed    = new Color(0.94f, 0.18f, 0.18f);
    private readonly Color colorGreen  = new Color(0.18f, 0.85f, 0.28f);
    private readonly Color colorYellow = new Color(1.00f, 0.88f, 0.10f);
    private readonly Color colorBlue   = new Color(0.18f, 0.50f, 0.95f);

    void Awake()
    {
        GenerateCodes();
    }

    void GenerateCodes()
    {
        RedCode    = Random.Range(0, 10);
        GreenCode  = Random.Range(0, 10);
        YellowCode = Random.Range(0, 10);
        BlueCode   = Random.Range(0, 10);

        ApplyToText(redNumberText,    RedCode,    colorRed);
        ApplyToText(greenNumberText,  GreenCode,  colorGreen);
        ApplyToText(yellowNumberText, YellowCode, colorYellow);
        ApplyToText(blueNumberText,   BlueCode,   colorBlue);

        // Debug para veres os códigos no Console durante o desenvolvimento
        Debug.Log($"[ColorCode] Vermelho={RedCode} | Verde={GreenCode} | Amarelo={YellowCode} | Azul={BlueCode}");
    }

    void ApplyToText(TextMeshPro tmp, int number, Color color)
    {
        if (tmp == null)
        {
            Debug.LogWarning("[ColorCodeManager] Um TextMeshPro não foi atribuído no Inspector!");
            return;
        }
        tmp.text  = number.ToString();
        tmp.color = color;
    }
}
