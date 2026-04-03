using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    public GameObject uiPanel; 
    private static InterfaceManager instancia;

    void Awake()
    {
        // Forzamos que se salga de cualquier carpeta/padre para que DontDestroy funcione
        transform.SetParent(null); 

        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SISTEMA: UI marcada para sobrevivir entre escenas.");
        }
        else
        {
            Debug.Log("SISTEMA: Ya existe una UI, destruyendo duplicado.");
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (uiPanel != null)
        {
            // ¡IMPORTANTE! Revisa que el nombre de la escena sea EXACTO
            bool esMapa = (scene.name == "Mapa");
            uiPanel.SetActive(!esMapa); 

            Debug.Log("Escena cargada: " + scene.name + " | UI Activa: " + !esMapa);
        }
    }
}