using UnityEngine;
using UnityEngine.InputSystem;

public class Pruebas : MonoBehaviour
{
    [SerializeField] private ScanUIManager scanUIManager;
    [SerializeField] private UnlockManager unlockManager;
    [SerializeField] private ARSpeciesData especiePrueba;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            bool esNueva = unlockManager.DesbloquearEspecie(especiePrueba.id);

            scanUIManager.MostrarEscaneo(especiePrueba);

            if (esNueva)
            {
                scanUIManager.MostrarNotificacion();
            }
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            scanUIManager.OcultarEscaneo();
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            scanUIManager.OcultarNotificacion();
        }
    }
}