using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PersistenciaUI : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreDelPanel = "PanelMarco"; 
    public List<string> escenasSinUI = new List<string> { "Mapa", "Almacen" };

    [Header("Ajustes de Planta (Global)")]
    public float velocidadDescuidado = 1.5f;

    private static PersistenciaUI instancia;
    private GameObject uiPanelMarco;

    void Awake()
    {
        // 1. Singleton para que no se duplique y sea eterno
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

        BuscarPanel();
    }

    // --- CORRECCIÓN CLAVE: El Update del Canvas ---
    void Update()
    {
        // Recorremos las 3 plantas
        for (int i = 0; i < EstadoPlanta.armonias.Length; i++)
        {
            if (EstadoPlanta.vivas[i] && EstadoPlanta.armonias[i] > 0)
            {
                EstadoPlanta.armonias[i] -= velocidadDescuidado * Time.deltaTime;
            }
            else if (EstadoPlanta.armonias[i] <= 0)
            {
                EstadoPlanta.vivas[i] = false;
            }
        }
    }
void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

    void BuscarPanel()
    {
        if (uiPanelMarco == null)
        {
            Transform t = transform.Find(nombreDelPanel);
            if (t != null) uiPanelMarco = t.gameObject;
        }
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
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

            // Esto solo oculta lo visual, pero el Update de arriba sigue bajando la armonía
            uiPanelMarco.SetActive(!debeOcultarse);
            Debug.Log("Escena: " + escena.name + " | UI Activa: " + !debeOcultarse + " | Armonía Actual: " + EstadoPlanta.armoniaActual);
        }
    }
}
