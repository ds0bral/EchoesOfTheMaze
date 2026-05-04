using UnityEngine;

public class SomAndar : MonoBehaviour
{
    [SerializeField] AudioClip _somPassos;
    [SerializeField] AudioClip _somPassosCorrer;
    AudioSource _audioSource;
    float _ultimoSom = 0f;
    [SerializeField] float _cooldown = 0.15f;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = GetComponentInParent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;
    }

    public void SomPassos()
    {
        TocarSom(_somPassos);
    }

    public void SomPassosCorrer()
    {
        TocarSom(_somPassosCorrer);
    }

    void TocarSom(AudioClip clip)
    {
        if (_audioSource == null || clip == null) return;
        if (Time.time - _ultimoSom < _cooldown) return;

        _ultimoSom = Time.time;
        _audioSource.pitch = Random.Range(0.9f, 1.1f); // varia ligeiramente o tom
        _audioSource.PlayOneShot(clip);
    }

    public void PararSom()
    {
        if (_audioSource != null && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}