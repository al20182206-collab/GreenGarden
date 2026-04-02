using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    public GameObject uiPanel; // Arrastra aquí el objeto que tiene tu frame/marco

    void OnEnable()
    {
        // Nos suscribimos al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si la escena cargada es "Mapa", apagamos el Frame
        if (scene.name == "Mapa") 
        {
            uiPanel.SetActive(false);
        }
        else 
        {
            uiPanel.SetActive(true);
        }
    }
}
