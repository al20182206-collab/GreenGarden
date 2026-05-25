using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogicaPlantaUniversal : MonoBehaviour 
{
    [Header("Configuración de esta Planta")]
    public int indicePlanta; 

    public Sprite[] etapas; 
    public Slider barra;
    public Sprite spriteMuerta;

    private SpriteRenderer render;
    private int etapaActual = 0;
    
    [Header("Ajustes de Sonido")]
    public AudioClip sonidoAgua; 
    public AudioClip sonidoPala; 
    private AudioSource bocinaPlanta;

    private bool esperandoAnuncio = false;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        
        bocinaPlanta = gameObject.AddComponent<AudioSource>();
        bocinaPlanta.playOnAwake = false; 
        
        float segundos = EstadoPlanta.ObtenerTiempoTranscurrido();
        EstadoPlanta.armonias[indicePlanta] -= segundos * 1.5f;

        SincronizarVisual();
    }

    void Update() {
        float armonia = EstadoPlanta.armonias[indicePlanta];
        if (barra != null) barra.value = armonia;

        if (armonia <= 0) {
            if (spriteMuerta != null) render.sprite = spriteMuerta;
            render.color = Color.gray;
            EstadoPlanta.vivas[indicePlanta] = false;
        } else {
            render.color = Color.white;
            EstadoPlanta.vivas[indicePlanta] = true;
            gameObject.tag = "Untagged"; 
            SincronizarVisual();
        }
    }
    
    void SincronizarVisual() 
    {
        if (EstadoPlanta.vivas[indicePlanta]) {
            etapaActual = Mathf.FloorToInt(EstadoPlanta.progresos[indicePlanta] / 5f);
            etapaActual = Mathf.Clamp(etapaActual, 0, etapas.Length - 1);
            if (etapas.Length > 0) render.sprite = etapas[etapaActual];
        }
    }

    // --- LÓGICA DE RIEGO (AGUA) ---
    void OnTriggerStay2D(Collider2D otro) {
        // 🛡️ SEGURIDAD: Si el juego está pausado (anuncio activo), la planta se vuelve intocable
        if (Time.timeScale == 0f) return;

        if (otro.CompareTag("Agua") && EstadoPlanta.vivas[indicePlanta]) {
            if (!bocinaPlanta.isPlaying)
            {
                bocinaPlanta.clip = sonidoAgua;
                bocinaPlanta.loop = true;
                bocinaPlanta.Play();
            }

            EstadoPlanta.armonias[indicePlanta] += 20f * Time.deltaTime;
            EstadoPlanta.armonias[indicePlanta] = Mathf.Clamp(EstadoPlanta.armonias[indicePlanta], 0, 100);

            if (EstadoPlanta.armonias[indicePlanta] > 60) {
                EstadoPlanta.progresos[indicePlanta] += Time.deltaTime;
            }
        }
    }

    void OnTriggerExit2D(Collider2D otro) {
        if (otro.CompareTag("Agua")) {
            if (bocinaPlanta.isPlaying) {
                bocinaPlanta.Stop();
            }
        }
    }

    // --- LÓGICA DE LA PALA ---
    void OnTriggerEnter2D(Collider2D otro) {
        // 🛡️ SEGURIDAD: Si el tiempo está pausado, ignoramos los choques "fantasmas" de la pala
        if (Time.timeScale == 0f) return;

        if (otro.CompareTag("Pala") && !EstadoPlanta.vivas[indicePlanta] && !esperandoAnuncio) {
            
            esperandoAnuncio = true; 

            if (ControlAnuncio.instancia != null) {
                ControlAnuncio.instancia.MostrarAnuncio();
                StartCoroutine(EsperarAnuncioYRevivir());
            } else {
                RevivirPlantaMecanica();
            }
        }
    }

    IEnumerator EsperarAnuncioYRevivir() {
        // 1. Esperamos un frame para que el ControlAnuncio alcance a hacer Time.timeScale = 0f
        yield return null; 

        // 2. 🧠 MAGIA: En lugar de contar a ciegas, esperamos inteligentemente a que el anuncio termine y despause el juego
        yield return new WaitUntil(() => Time.timeScale > 0f); 

        // 3. Una vez que el tiempo corre normal, aplicamos la vida y el sonido de golpe
        RevivirPlantaMecanica();
    }

    void RevivirPlantaMecanica() {
        EstadoPlanta.armonias[indicePlanta] = 100f;
        EstadoPlanta.progresos[indicePlanta] = 0f;
        EstadoPlanta.vivas[indicePlanta] = true;
        gameObject.tag = "Untagged";
        
        etapaActual = 0;
        if (etapas.Length > 0) {
            render.sprite = etapas[0];
            render.color = Color.white;
        }

        // 💥 SUENA LA PALA UNA SOLA VEZ AL REVIVIR 💥
        if (sonidoPala != null && bocinaPlanta != null) {
            bocinaPlanta.PlayOneShot(sonidoPala);
        }

        esperandoAnuncio = false; 

        Debug.Log("Planta " + indicePlanta + " limpiada y revivida con éxito.");
    }
}