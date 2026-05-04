using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SistemaMensagem : MonoBehaviour
{
    public static SistemaMensagem instance;

    TMP_Text txt_mensagem;
    [SerializeField] float tempoVisivel = 2f;
    [SerializeField] float tempoFade = 1f;

    Dictionary<string, int> _itensAtivos = new Dictionary<string, int>();
    Coroutine _coroutineAtual;
    bool _mensagemSimples = false;  // flag para saber qual fluxo está ativo

    void Awake() { instance = this; }

    void Start()
    {
        txt_mensagem = GetComponent<TMP_Text>();
        txt_mensagem.alpha = 0f;
    }

    // Mensagem com item + quantidade (inventário)
    public void MostrarMensagem(string nomeItem, int quantidade)
    {
        if (_itensAtivos.ContainsKey(nomeItem))
            _itensAtivos[nomeItem] += quantidade;
        else
            _itensAtivos[nomeItem] = quantidade;

        _mensagemSimples = false;

        if (_coroutineAtual != null)
            StopCoroutine(_coroutineAtual);

        _coroutineAtual = StartCoroutine(MostrarEDesaparecer());
    }

    // Mensagem simples (prompt, avisos, etc.)
    public void MostrarMensagem(string mensagem)
    {
        _mensagemSimples = true;
        txt_mensagem.text = mensagem;

        if (_coroutineAtual != null)
            StopCoroutine(_coroutineAtual);

        _coroutineAtual = StartCoroutine(MostrarEDesaparecer());
    }

    IEnumerator MostrarEDesaparecer()
    {
        // Só atualiza texto de itens se não for mensagem simples
        if (!_mensagemSimples)
            AtualizarTexto();

        txt_mensagem.alpha = 1f;

        yield return new WaitForSeconds(tempoVisivel);

        float t = 0f;
        while (t < tempoFade)
        {
            t += Time.deltaTime;
            txt_mensagem.alpha = 1f - (t / tempoFade);
            yield return null;
        }

        txt_mensagem.alpha = 0f;

        if (!_mensagemSimples)
            _itensAtivos.Clear();
    }

    void AtualizarTexto()
    {
        string texto = "";
        foreach (var item in _itensAtivos)
            texto += $"{item.Key} {item.Value}x\n";

        txt_mensagem.text = texto.TrimEnd('\n');
    }
}