using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorMusica : MonoBehaviour
{
    public static GestorMusica instancia; 
    public AudioSource fuenteMusica;
    public AudioClip musicaFondo;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene escena, LoadSceneMode modo)
    {
        VerificarMusica(escena.name);
    }

    void VerificarMusica(string nombreEscena)
    {
        // Si estamos en la Intro, paramos. Si no, reproducimos.
        if (nombreEscena == "Intro") 
        {
            if (fuenteMusica.isPlaying) fuenteMusica.Stop();
        }
        else
        {
            if (!fuenteMusica.isPlaying) 
            {
                fuenteMusica.clip = musicaFondo;
                fuenteMusica.loop = true;
                fuenteMusica.Play();
            }
        }
    }

    // 💥 NUEVO: Métodos para que el anuncio controle la música
    public void PausarMusica() { fuenteMusica.Pause(); }
    public void ReanudarMusica() { fuenteMusica.UnPause(); }
}