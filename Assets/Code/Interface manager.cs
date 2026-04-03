using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PersistenciaUI : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreDelPanel = "PanelMarco"; // Asegúrate de que tu objeto se llame así
    public List<string> escenasSinUI = new List<string> { "Mapa", "Almacen" };

    private static PersistenciaUI instancia;
    private GameObject uiPanelMarco;

    void Awake()
    {
        // 1. Singleton para que no se duplique
        if (instancia == null)
        {
            instancia = this;
            transform.SetParent(null); 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 2. Buscamos el panel la primera vez
        BuscarPanel();
    }

    void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

    void BuscarPanel()
    {
        // Buscamos el objeto por nombre dentro de los hijos del Canvas
        if (uiPanelMarco == null)
        {
            Transform t = transform.Find(nombreDelPanel);
            if (t != null) uiPanelMarco = t.gameObject;
        }
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        // Siempre nos aseguramos de tener la referencia al despertar en escena nueva
        BuscarPanel();

        if (uiPanelMarco != null)
        {
            bool debeOcultarse = false;
            foreach (string s in escenasSinUI)
            {
                if (escena.name.Trim().ToLower() == s.Trim().ToLower())
                {
                    debeOcultarse = true;
                    break;
                }
            }

            uiPanelMarco.SetActive(!debeOcultarse);
            Debug.Log("Escena: " + escena.name + " | UI Activa: " + !debeOcultarse);
        }
    }
}
