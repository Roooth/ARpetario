using TMPro;
using UnityEngine;

public class ScanUIManager : MonoBehaviour
{
    [Header("Panel de escaneo")]
    [SerializeField] private GameObject panelTexto;
    [SerializeField] private TMP_Text nombreDeEscaneado;

    [Header("Imagen de carga")]
    [SerializeField] private RectTransform imgCargando;
    [SerializeField] private Sprite spriteCargando;
    [SerializeField] private Sprite spriteDetectado;

    [Header("Notificación")]
    [SerializeField] private GameObject notificacionIcono;

    [Header("Configuración")]
    [SerializeField] private float velocidadRotacion = 180f;

    private Quaternion rotacionInicial;

    private void Awake()
    {
        rotacionInicial = imgCargando.localRotation;

        panelTexto.SetActive(false);
        notificacionIcono.SetActive(false);

        imgCargando.GetComponent<UnityEngine.UI.Image>().sprite = spriteCargando;
    }

    private void Update()
    {
        if (panelTexto.activeSelf)
        {
            return;
        }

        imgCargando.Rotate(0f, 0f, -velocidadRotacion * Time.deltaTime);
    }

    public void MostrarEscaneo(ARSpeciesData especie)
    {
        panelTexto.SetActive(true);

        nombreDeEscaneado.text = especie.nombre;

        imgCargando.localRotation = rotacionInicial;
        imgCargando.GetComponent<UnityEngine.UI.Image>().sprite = spriteDetectado;
    }

    public void OcultarEscaneo()
    {
        panelTexto.SetActive(false);
        nombreDeEscaneado.text = "";

        imgCargando.localRotation = rotacionInicial;
        imgCargando.GetComponent<UnityEngine.UI.Image>().sprite = spriteCargando;
    }

    public void MostrarNotificacion()
    {
        notificacionIcono.SetActive(true);
    }

    public void OcultarNotificacion()
    {
        notificacionIcono.SetActive(false);
    }
}