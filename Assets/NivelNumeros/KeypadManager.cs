using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadManager : MonoBehaviour
{
    [Header("Porta")]
    public Animator portaAnimator;

    [Header("Display")]
    public TextMeshProUGUI codigoEscritoText;

    [Header("Botões Números")]
    public Button btn0;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Button btn5;
    public Button btn6;
    public Button btn7;
    public Button btn8;
    public Button btn9;

    [Header("Botões Ação")]
    public Button btnApagar;
    public Button btnAplicar;
    public Button btnSair;

    [Header("Painel")]
    public GameObject keypadPanel;

    [Header("Sons")]
    public AudioSource audioSource;
    public AudioClip somBotao;    // Som ao carregar num número
    public AudioClip somCorreto;  // Som quando o código está certo
    public AudioClip somErrado;   // Som quando o código está errado

    // Cores da ordem
    private readonly Color colorRed    = new Color(0.94f, 0.18f, 0.18f);
    private readonly Color colorGreen  = new Color(0.18f, 0.85f, 0.28f);
    private readonly Color colorYellow = new Color(1.00f, 0.88f, 0.10f);
    private readonly Color colorBlue   = new Color(0.18f, 0.50f, 0.95f);

    private string inputAtual = "";
    private bool doorOpen = false;

    void Start()
    {
        btn0.onClick.AddListener(() => PressionarNumero("0"));
        btn1.onClick.AddListener(() => PressionarNumero("1"));
        btn2.onClick.AddListener(() => PressionarNumero("2"));
        btn3.onClick.AddListener(() => PressionarNumero("3"));
        btn4.onClick.AddListener(() => PressionarNumero("4"));
        btn5.onClick.AddListener(() => PressionarNumero("5"));
        btn6.onClick.AddListener(() => PressionarNumero("6"));
        btn7.onClick.AddListener(() => PressionarNumero("7"));
        btn8.onClick.AddListener(() => PressionarNumero("8"));
        btn9.onClick.AddListener(() => PressionarNumero("9"));

        btnApagar.onClick.AddListener(Apagar);
        btnAplicar.onClick.AddListener(Aplicar);
        btnSair.onClick.AddListener(Sair);

        AtualizarDisplay();
    }

    void TocarSom(AudioClip clip, float volume = 1f)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);
    }


    void PressionarNumero(string numero)
    {
        if (inputAtual.Length >= 4) return;

        TocarSom(somBotao, 0.4f); // <-- som de clique
        inputAtual += numero;
        AtualizarDisplay();
    }

    void Apagar()
    {
        if (inputAtual.Length == 0) return;

        TocarSom(somBotao, 0.4f); // <-- som de clique também no apagar
        inputAtual = inputAtual.Substring(0, inputAtual.Length - 1);
        AtualizarDisplay();
    }

    void Aplicar()
    {
        if (inputAtual.Length < 4)
        {
            TocarSom(somErrado, 0.8f); // <-- código incompleto = erro
            MostrarTexto("ERRO", Color.red);
            return;
        }

        int[] correct = new int[]
        {
            NumerosNivel.RedCode,
            NumerosNivel.GreenCode,
            NumerosNivel.YellowCode,
            NumerosNivel.BlueCode
        };

        bool success = true;
        for (int i = 0; i < 4; i++)
        {
            if (int.Parse(inputAtual[i].ToString()) != correct[i])
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            TocarSom(somCorreto); // <-- código correto!
            MostrarTexto("ABERTO", colorGreen);
            doorOpen = true;
            Time.timeScale = 1f;
            Invoke(nameof(AbrirPorta), 1f);
        }
        else
        {
            TocarSom(somErrado); // <-- código errado!
            MostrarTexto("ERRO", colorRed);
            inputAtual = "";
            Invoke(nameof(AtualizarDisplay), 1.5f);
        }
    }

    void AbrirPorta()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (portaAnimator != null)
            portaAnimator.SetTrigger("Abrir");

        Time.timeScale = 1f;
    }

    void Sair()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void AtualizarDisplay()
    {
        if (codigoEscritoText == null) return;

        Color[] cores = { colorRed, colorGreen, colorYellow, colorBlue };
        string linha = "";

        for (int i = 0; i < 4; i++)
        {
            string hex = ColorUtility.ToHtmlStringRGB(cores[i]);
            if (i < inputAtual.Length)
                linha += $"<color=#{hex}>{inputAtual[i]}</color>";
            else
                linha += $"<color=#{hex}>_</color>";

            if (i < 3) linha += "  ";
        }

        codigoEscritoText.text = linha;
        codigoEscritoText.color = Color.white;
    }

    void MostrarTexto(string mensagem, Color cor)
    {
        if (codigoEscritoText == null) return;
        codigoEscritoText.text = mensagem;
        codigoEscritoText.color = cor;
    }

    public void AbrirPainel()
    {
        if (doorOpen) return;
        keypadPanel.SetActive(true);
        inputAtual = "";
        AtualizarDisplay();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}