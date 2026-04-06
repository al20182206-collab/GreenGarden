using UnityEngine;
using UnityEngine.UI;

public class LogicaPlanta : MonoBehaviour 
{
    public Sprite[] etapas; 
    public Slider barra;
    public float armonia = 50f; 
    private int etapaActual = 0;
    private float tiempoRecibiendoAgua = 0f; 
    private SpriteRenderer render;

    void Start() 
    {
        render = GetComponent<SpriteRenderer>();
        
        // 1. Cargamos datos
            armonia = EstadoPlanta.armoniaActual;
            tiempoRecibiendoAgua = EstadoPlanta.progresoCrecimiento;
        
            // 2. EL TRUCO DEL TIEMPO:
            float segundosAusente = EstadoPlanta.ObtenerTiempoTranscurrido();
            // Bajamos 1.5 de armonía por cada segundo que no estuviste
            armonia -= segundosAusente * 1.5f; 
            armonia = Mathf.Clamp(armonia, 0, 100);
        
            // ... el resto de tu código de etapas ...
     
        
        // Esto se ejecuta JUSTO cuando cambias de escena (cuando la planta desaparece)
        
       
        // 1. SINCRONIZACIÓN AL ENTRAR (Cargar datos del Cerebro)
        armonia = EstadoPlanta.armoniaActual;
        tiempoRecibiendoAgua = EstadoPlanta.progresoCrecimiento;

        // Recuperar la etapa visual basada en el tiempo acumulado
        // (Calculamos en qué etapa debería estar según el tiempo que guardamos)
        // Si cada etapa requiere 5 seg, dividimos el tiempo entre 5
        etapaActual = Mathf.FloorToInt(tiempoRecibiendoAgua / 5f);
        etapaActual = Mathf.Clamp(etapaActual, 0, etapas.Length - 1);

        if (etapas.Length > 0) render.sprite = etapas[etapaActual];

        Debug.Log("Planta sincronizada: Armonía " + armonia + " | Etapa " + etapaActual);
        
         void OnDisable() 
         {
                    // Guardamos la hora actual para la próxima vez
                    EstadoPlanta.ultimaVezVisto = System.DateTime.Now.ToString();
                    // Aseguramos que el cerebro tenga el valor actualizado antes de irnos
                    EstadoPlanta.armoniaActual = armonia;
          }
     
    }

void Update() 
    {
        // 2. La armonía baja por descuido
        if (armonia > 0) {
           armonia = EstadoPlanta.armoniaActual;
        }

        if (barra != null) barra.value = armonia;

        // 3. Efecto visual de muerte/vida
        if (armonia <= 0) {
            render.color = Color.gray;
            EstadoPlanta.estaViva = false;
        } else {
            render.color = Color.white;
            EstadoPlanta.estaViva = true;
        }

        // 4. GUARDAR DATOS EN EL CEREBRO (Sincronización constante)
        EstadoPlanta.armoniaActual = armonia;
        EstadoPlanta.progresoCrecimiento = tiempoRecibiendoAgua;
    }

   void OnTriggerStay2D(Collider2D otro) {
       // Si el agua toca la planta y el "Cerebro" dice que está viva
       if (otro.CompareTag("Agua") && EstadoPlanta.estaViva) {
   
           // --- CAMBIO CLAVE ---
           // No subimos "armonia", subimos directamente la del EstadoPlanta
           EstadoPlanta.armoniaActual += 20f * Time.deltaTime; 
   
           // Limitamos para que no pase de 100
           EstadoPlanta.armoniaActual = Mathf.Clamp(EstadoPlanta.armoniaActual, 0, 100);
   
           // Actualizamos nuestra variable local para que el resto del script funcione
           armonia = EstadoPlanta.armoniaActual;
   
           // Lógica de crecimiento (la que ya tenías)
           if (armonia > 60) {
               tiempoRecibiendoAgua += Time.deltaTime;
   
               int nuevaEtapa = Mathf.FloorToInt(tiempoRecibiendoAgua / 5f);
               if (nuevaEtapa > etapaActual && nuevaEtapa < etapas.Length) {
                   etapaActual = nuevaEtapa;
                   render.sprite = etapas[etapaActual];
                   Debug.Log("¡Evolución!");
               }
           }
       }
   }
}