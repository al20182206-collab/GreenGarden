using UnityEngine;
using UnityEngine.UI;

public class LogicaPlanta : MonoBehaviour {
    public Sprite[] etapas; 
    public Slider barra;
    public float armonia = 50f; // Empezamos a la mitad
    private int etapaActual = 0;
    private float tiempoRecibiendoAgua = 0f; // Solo sube con agua
    private SpriteRenderer render;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        if (etapas.Length > 0) render.sprite = etapas[0];
    }

    void Update() {
        // 1. La armonía SIEMPRE baja (el descuido)
        if (armonia > 0) {
            armonia -= 1.5f * Time.deltaTime; 
        }

        barra.value = armonia;

        // 2. Si la armonía llega a 0, la planta se pone gris (muere)
        if (armonia <= 0) {
            render.color = Color.gray;
        } else {
            render.color = Color.white;
        }
    }

    // 3. ESTA ES LA CLAVE: Solo crece si el agua la está tocando
    void OnTriggerStay2D(Collider2D otro) {
        if (otro.CompareTag("Agua")) {
            // Sube la armonía
            armonia += 15f * Time.deltaTime; 
            armonia = Mathf.Clamp(armonia, 0, 100);

            // Solo si tiene buena armonía Y le está cayendo agua, crece
            if (armonia > 60) {
                tiempoRecibiendoAgua += Time.deltaTime;

                // Si completas 5 segundos acumulados de riego, evoluciona
                if (tiempoRecibiendoAgua > 5f && etapaActual < etapas.Length - 1) { 
                    etapaActual++;
                    render.sprite = etapas[etapaActual];
                    tiempoRecibiendoAgua = 0; // Reiniciar para la siguiente etapa
                    Debug.Log("¡Evolución!");
                }
            }
        }
    }
}