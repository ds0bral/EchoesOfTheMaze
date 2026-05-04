using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Camera _camera;
    [SerializeField] float _distanciaInteracao = 5f;
    PlayerControols controls;

    IInteract objetoAtual = null;
    ILerNota notaAtual = null;
    bool notaAberta = false;

    void Awake()
    {
        controls = new PlayerControols();
    }

    void OnEnable()
    { 
        if (controls == null)
            controls = new PlayerControols();
        controls.Gameplay.Enable(); 
    }
    void OnDisable() { controls.Gameplay.Disable(); }

    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // Se a nota está aberta, não faz raycast — mantém a referência
        if (!notaAberta)
        {
            Vector3 origem = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(origem, _camera.transform.forward, out hit, _distanciaInteracao))
            {
                var novoObjeto = hit.collider.GetComponent<IInteract>();
                var novaNota = hit.collider.GetComponent<ILerNota>();

                if (novaNota != null && novaNota != notaAtual)
                    SistemaMensagem.instance.MostrarMensagem("Prima X para ler a nota");

                objetoAtual = novoObjeto;
                notaAtual = novaNota;
            }
            else
            {
                objetoAtual = null;
                notaAtual = null;
            }
        }

        if (controls.Gameplay.Interacao.WasPressedThisFrame())
            objetoAtual?.Acao();

        if (controls.Gameplay.LerNota.WasPressedThisFrame())
        {
            notaAtual?.LerNota();
            if (notaAtual != null)
                notaAberta = !notaAberta; // sincroniza com o estado do painel
        }
    }
}