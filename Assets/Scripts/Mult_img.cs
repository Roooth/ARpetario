
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Mult_img : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private UnlockManager unlockManager;
    [SerializeField] private ScanUIManager scanUIManager;
    [SerializeField, Range(0.1f, 1f)] private float proporcionMarcador = 0.8f;
    [SerializeField] private float desplazamientoFrontal = 0.02f;

    private ARTrackedImageManager _aRTrackedImageManager;
    private Dictionary<string, GameObject> _arObjects;
    private Dictionary<string, Vector3> _escalasOriginales;
    private Dictionary<string, Vector3> _dimensionesOriginales;

    private TrackableId _idImagenActiva;
    private bool _hayImagenActiva;
    private string _imagenActiva;

    private void Awake()
    {
        _aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();
        _escalasOriginales = new Dictionary<string, Vector3>();
        _dimensionesOriginales = new Dictionary<string, Vector3>();
    }

    private void OnEnable()
    {
        if (_aRTrackedImageManager != null)
        {
            _aRTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    private void Start()
    {
        foreach (GameObject prefab in prefabsToSpawn)
        {
            if (prefab == null || _arObjects.ContainsKey(prefab.name))
            {
                continue;
            }

            GameObject modelo = Instantiate(prefab);
            modelo.name = prefab.name;

            _escalasOriginales.Add(prefab.name, modelo.transform.localScale);

            Renderer[] renderizadores =
                modelo.GetComponentsInChildren<Renderer>(true);

            bool encontrado = false;
            Bounds limites = new Bounds();

            foreach (Renderer renderizador in renderizadores)
            {
                if (!encontrado)
                {
                    limites = renderizador.bounds;
                    encontrado = true;
                }
                else
                {
                    limites.Encapsulate(renderizador.bounds);
                }
            }

            _dimensionesOriginales.Add(
                prefab.name,
                encontrado ? limites.size : Vector3.zero
            );

            modelo.SetActive(false);
            _arObjects.Add(prefab.name, modelo);
        }

        ActualizarSeleccion();
    }

    private void OnDisable()
    {
        if (_aRTrackedImageManager != null)
        {
            _aRTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(
        ARTrackedImagesChangedEventArgs eventArgs)
    {
        ActualizarSeleccion();
    }

    private void ActualizarSeleccion()
    {
        ARTrackedImage seleccionada = null;

        if (_hayImagenActiva)
        {
            foreach (ARTrackedImage imagen in _aRTrackedImageManager.trackables)
            {
                if (imagen.trackableId == _idImagenActiva &&
                    imagen.trackingState == TrackingState.Tracking &&
                    _arObjects.ContainsKey(imagen.referenceImage.name))
                {
                    seleccionada = imagen;
                    break;
                }
            }
        }

        if (seleccionada == null)
        {
            foreach (ARTrackedImage imagen in _aRTrackedImageManager.trackables)
            {
                if (imagen.trackingState == TrackingState.Tracking &&
                    _arObjects.ContainsKey(imagen.referenceImage.name))
                {
                    seleccionada = imagen;
                    break;
                }
            }
        }

        if (seleccionada == null)
        {
            OcultarTodosLosModelos();
            _hayImagenActiva = false;
            _imagenActiva = null;
            scanUIManager.OcultarEscaneo();
            return;
        }

        string nombreImagen = seleccionada.referenceImage.name;
        bool cambioImagen = !_hayImagenActiva ||
            seleccionada.trackableId != _idImagenActiva;

        if (!cambioImagen)
        {
            return;
        }

        OcultarTodosLosModelos();

        GameObject modeloActivo = _arObjects[nombreImagen];

        modeloActivo.transform.SetParent(seleccionada.transform, false);
        modeloActivo.transform.localPosition =
            new Vector3(0f, 0f, desplazamientoFrontal);
        modeloActivo.transform.localRotation = Quaternion.identity;

        AjustarEscala(modeloActivo, nombreImagen, seleccionada.size);

        modeloActivo.SetActive(true);

        _idImagenActiva = seleccionada.trackableId;
        _hayImagenActiva = true;
        _imagenActiva = nombreImagen;

        ARSpeciesData especie = unlockManager.ObtenerEspecie(nombreImagen);

        if (especie == null)
        {
            scanUIManager.OcultarEscaneo();
            Debug.LogWarning(
                "No existe ARSpeciesData para el Image Target: " + nombreImagen
            );
            return;
        }

        bool esNueva = unlockManager.DesbloquearEspecie(especie.id);

        scanUIManager.MostrarEscaneo(especie);

        if (esNueva)
        {
            scanUIManager.MostrarNotificacion();
        }
    }

    private void AjustarEscala(
        GameObject modelo,
        string nombre,
        Vector2 tamanoMarcador)
    {
        Vector3 dimensiones = _dimensionesOriginales[nombre];
        Vector3 escalaOriginal = _escalasOriginales[nombre];

        if (dimensiones.x <= 0f || dimensiones.y <= 0f)
        {
            modelo.transform.localScale = escalaOriginal;
            return;
        }

        float factorX =
            tamanoMarcador.x * proporcionMarcador / dimensiones.x;

        float factorY =
            tamanoMarcador.y * proporcionMarcador / dimensiones.y;

        float factor = Mathf.Min(factorX, factorY);

        modelo.transform.localScale = escalaOriginal * factor;
    }

    private void OcultarTodosLosModelos()
    {
        foreach (GameObject modelo in _arObjects.Values)
        {
            modelo.SetActive(false);
        }
    }
}
