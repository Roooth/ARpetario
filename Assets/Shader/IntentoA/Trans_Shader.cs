using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Trans_Shader : MonoBehaviour
{
    public Image laImagen;
    public float velTrans;

    private string escenaDestino;

    private enum Estado { Ninguno, Revelando, Ocultando }
    private Estado estado = Estado.Revelando;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip sonidoTransicion;

    void Awake()
    {
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (laImagen == null)
            laImagen = GetComponentInChildren<Image>();
    }

    void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, volume);
    }

    void Start()
    {
        if (laImagen == null)
        {
            Debug.LogError("Trans_Shader: no encontró Image", this);
            enabled = false;
            return;
        }

        if (laImagen.material == null)
        {
            Debug.LogError("Trans_Shader: la imagen no tiene material", this);
            enabled = false;
            return;
        }

        laImagen.material.SetFloat("_CutOff", -0.1f);
        estado = Estado.Revelando;
    }

    void Update()
    {
        float smooth = laImagen.material.GetFloat("_Smooth");

        if (estado == Estado.Revelando)
        {
            float target = 1.1f;

            laImagen.material.SetFloat("_CutOff",
                Mathf.MoveTowards(
                    laImagen.material.GetFloat("_CutOff"),
                    target,
                    velTrans * Time.deltaTime));
             
            if (laImagen.material.GetFloat("_CutOff") >= target - 0.001f)
            {
                estado = Estado.Ninguno;
            }
        }
        else if (estado == Estado.Ocultando)
        {
            float target = -0.1f - smooth;

            laImagen.material.SetFloat("_CutOff",
                Mathf.MoveTowards(
                    laImagen.material.GetFloat("_CutOff"),
                    target,
                    velTrans * Time.deltaTime));


            if (laImagen.material.GetFloat("_CutOff") <= target + 0.001f)
            {
                Debug.Log("Ya terminó la transición.");
                SceneManager.LoadScene(escenaDestino);
                estado = Estado.Revelando;
            }
        }
    }

    public void TransicionarA(string nombreEscena)
    {
        Debug.Log("Transicionando a: " + nombreEscena);
        escenaDestino = nombreEscena;

        Play(sonidoTransicion, 1f, Random.Range(0.95f, 1.05f));

        estado = Estado.Ocultando;
    }
}
