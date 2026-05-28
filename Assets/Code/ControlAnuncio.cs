using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ControlAnuncio : MonoBehaviour
{
    public static ControlAnuncio instancia;
    private float duracionAnuncio = 10f;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void MostrarAnuncio()
    {
        StartCoroutine(Mostrar());
    }

    IEnumerator Mostrar()
    {
        // 1. Pausa el juego completamente
        Time.timeScale = 0f;

        // 2. Carga la escena de anuncios encima (Additive para no perder la UI)
        AsyncOperation cargar = SceneManager.LoadSceneAsync("Anuncios", LoadSceneMode.Additive);
        yield return cargar;

        // 3. Espera 10 segundos en tiempo real (ignora la pausa)
        yield return new WaitForSecondsRealtime(duracionAnuncio);

        // 4. Quita la escena del anuncio
        SceneManager.UnloadSceneAsync("Anuncios");

        // 5. Reactiva el tiempo
        Time.timeScale = 1f;
    }
}