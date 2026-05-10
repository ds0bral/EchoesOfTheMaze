using UnityEngine;

public class SomAndar : MonoBehaviour
{
    [SerializeField] AudioClip[] _somPassos;
    [SerializeField] AudioClip[] _somPassosCorrer;
    AudioSource _audioSource;
    [SerializeField]  float volume = 1f;
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
        _audioSource.volume = volume;
    }

    public void SomPassos()
    {
        TocarSom(_somPassos);
    }

    public void SomPassosCorrer()
    {
        TocarSom(_somPassosCorrer);
    }

    void TocarSom(AudioClip[] clips)
    {
        if (_audioSource == null || clips == null || clips.Length == 0) return;
        if (Time.time - _ultimoSom < _cooldown) return;

        _ultimoSom = Time.time;
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    //public void PararSom()
    //{
    //    if (_audioSource != null && _audioSource.isPlaying)
    //        _audioSource.Stop();
    //}
}