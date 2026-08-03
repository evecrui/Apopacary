using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockingCameraDetector : MonoBehaviour
{
    public Transform cam;
    public GameObject currentlyTransparenting;
    public LayerMask obstructionMask;
    private Coroutine fadeRoutine;
    private float endFadeResultIntended;

    public float fadedAlpha = 0.1f;
    public float fadeDuration = 0.5f;
    string BaseColorID = "_BaseColor";


    private void LateUpdate()
    {
        Vector3 dir = transform.position - cam.position;

        if (Physics.Raycast(cam.position, dir.normalized, out RaycastHit hit, dir.magnitude, obstructionMask))
        {
            GameObject fade = hit.transform.gameObject;
            if (fade != currentlyTransparenting)
            {
                if (currentlyTransparenting != null)
                    StartFade(currentlyTransparenting, 1); // fading in
                StartFade(fade, fadedAlpha); // fading out
            }
        } else
        {
            if (currentlyTransparenting != null)
            {
                StartFade(currentlyTransparenting, 1); // fading in
                currentlyTransparenting = null;
            }
        }
    }

    private void StartFade(GameObject fading, float targetAlpha)
    {
        if (fadeRoutine != null && currentlyTransparenting != null)
        {
            Material m = currentlyTransparenting.GetComponent<MeshRenderer>().material;
            Color oldBaseColor = m.GetColor(BaseColorID);
            oldBaseColor.a = endFadeResultIntended;
            m.SetColor(BaseColorID, oldBaseColor);
            StopCoroutine(fadeRoutine);
        }

        currentlyTransparenting = fading;
        endFadeResultIntended = targetAlpha;
        fadeRoutine = StartCoroutine(FadeCoroutine(fading, targetAlpha));
    }

    private IEnumerator FadeCoroutine(GameObject fading, float targetAlpha)
    {
        Material m = currentlyTransparenting.GetComponent<MeshRenderer>().material;
        Color c = m.GetColor(BaseColorID);
        float startAlpha = c.a;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t);
            c.a = a;
            m.SetColor(BaseColorID, c);
            yield return null;
        }

        c.a = targetAlpha;
        m.SetColor(BaseColorID, c);
        fadeRoutine = null;
    }

}
