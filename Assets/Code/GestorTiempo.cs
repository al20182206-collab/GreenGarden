using UnityEngine;
using UnityEngine.SceneManagement;

public class CicloDiaNoche : MonoBehaviour
{
    public static CicloDiaNoche instancia;

    [Header("Configuración de Tiempo")]
    public float tiempoPorFase = 10f; // 5 minutos = 300 segundos
    public float tiempoActual = 0f;

    public enum FaseDia { Dia, Atardecer, Noche }
    public static FaseDia faseActual = FaseDia.Dia;

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
        }
    }

    void Update()
    {
        tiempoActual += Time.deltaTime;

        // Si pasan 5 minutos, cambiamos de fase
        if (tiempoActual >= tiempoPorFase)
        {
            CambiarFase();
            tiempoActual = 0f;
        }
    }

    void CambiarFase()
    {
        // Ciclo: Dia -> Atardecer -> Noche -> Dia...
        if (faseActual == FaseDia.Dia) faseActual = FaseDia.Atardecer;
        else if (faseActual == FaseDia.Atardecer) faseActual = FaseDia.Noche;
        else faseActual = FaseDia.Dia;

        Debug.Log("Cambiando a: " + faseActual);

        // Avisar a todos los fondos de la escena actual que se actualicen
        ActualizarFondosEnEscena();
    }

    // Buscamos los fondos en la escena cargada y les pedimos que cambien
    public void ActualizarFondosEnEscena()
    {
        ControladorFondo[] fondos = Object.FindObjectsByType<ControladorFondo>(FindObjectsSortMode.None);
        foreach (ControladorFondo f in fondos)
        {
            f.CambiarSpriteSegunFase();
        }
    }
    
    void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }
    
    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        ActualizarFondosEnEscena();
    }
}