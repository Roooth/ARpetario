using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class ControladorAR : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private float velocidadEscala = 0.005f;
    [SerializeField] private float escalaMinima = 0.1f;
    [SerializeField] private float escalaMaxima = 2f;
    [SerializeField] private float velocidadRotacion = 1f;

    [SerializeField] private ARPlaneManager planeManager;

    private GameObject modeloActual;
    private List<ARRaycastHit> impactos = new List<ARRaycastHit>();

    private bool modeloColocado;
    private float distanciaAnterior;
    private float anguloAnterior;

    private void Start()
    {
        ARSpeciesData especie = DatosAR.Instancia.especieSeleccionada;

        if (especie == null)
        {
            Debug.LogError("No hay una especie seleccionada.");
            return;
        }

        if (especie.prefab == null)
        {
            Debug.LogError("La especie no tiene un prefab asignado.");
            return;
        }

        modeloActual = Instantiate(especie.prefab);
        modeloActual.SetActive(false);
    }

    private void Update()
    {
        if (modeloActual == null)
        {
            return;
        }

        if (Touchscreen.current == null)
        {
            return;
        }

        var toquePrincipal = Touchscreen.current.primaryTouch;

        if (Touchscreen.current.touches.Count >= 2)
        {
            var toque1 = Touchscreen.current.touches[0];
            var toque2 = Touchscreen.current.touches[1];

            if (toque1.press.isPressed && toque2.press.isPressed)
            {
                Vector2 posicion1 = toque1.position.ReadValue();
                Vector2 posicion2 = toque2.position.ReadValue();

                float distanciaActual = Vector2.Distance(posicion1, posicion2);
                float anguloActual = Mathf.Atan2(
                    posicion2.y - posicion1.y,
                    posicion2.x - posicion1.x
                ) * Mathf.Rad2Deg;

                if (distanciaAnterior > 0)
                {
                    float diferenciaEscala = distanciaActual - distanciaAnterior;
                    EscalarModelo(diferenciaEscala);
                }

                if (anguloAnterior != 0)
                {
                    float diferenciaAngulo = Mathf.DeltaAngle(
                        anguloAnterior,
                        anguloActual
                    );

                    RotarModelo(diferenciaAngulo);
                }

                distanciaAnterior = distanciaActual;
                anguloAnterior = anguloActual;

                return;
            }
        }

        distanciaAnterior = 0;
        anguloAnterior = 0;

        if (toquePrincipal.press.wasPressedThisFrame)
        {
            Vector2 posicionToque = toquePrincipal.position.ReadValue();

            if (!modeloColocado)
            {
                if (raycastManager.Raycast(
                    posicionToque,
                    impactos,
                    TrackableType.PlaneWithinPolygon))
                {
                    Pose pose = impactos[0].pose;

                    modeloActual.transform.SetPositionAndRotation(
                        pose.position,
                        pose.rotation
                    );

                    modeloActual.SetActive(true);
                    modeloColocado = true;
                    OcultarPlanos();
                }
            }
        }

        if (modeloColocado && toquePrincipal.press.isPressed)
        {
            Vector2 posicionToque = toquePrincipal.position.ReadValue();

            if (raycastManager.Raycast(
                posicionToque,
                impactos,
                TrackableType.PlaneWithinPolygon))
            {
                modeloActual.transform.position = impactos[0].pose.position;
            }
        }
    }

    private void EscalarModelo(float diferencia)
    {
        float escala = modeloActual.transform.localScale.x;

        escala += diferencia * velocidadEscala;
        escala = Mathf.Clamp(escala, escalaMinima, escalaMaxima);

        modeloActual.transform.localScale = Vector3.one * escala;
    }


    private void RotarModelo(float diferencia)
    {
        modeloActual.transform.Rotate(
            Vector3.up,
            -diferencia * velocidadRotacion,
            Space.World
        );
    }

    private void OcultarPlanos()
    {
        foreach (ARPlane plano in planeManager.trackables)
        {
            plano.gameObject.SetActive(false);
        }
    }
}