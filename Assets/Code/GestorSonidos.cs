using UnityEngine;

public class GestorSonidos : MonoBehaviour
{
    public static GestorSonidos instancia;
    private AudioSource fuente;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            fuente = gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null && fuente != null)
        {
            fuente.PlayOneShot(clip);
        }
    }
}
