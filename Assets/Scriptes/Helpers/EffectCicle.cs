using System.Collections;
using UnityEngine;

public class EffectCicle : MonoBehaviour
{
    private ParticleSystem _particle;
    private void OnEnable()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        _particle.Play();
        StartCoroutine(DeleteEffect());
  
    }

    private IEnumerator DeleteEffect()
    {
        while(_particle.isPlaying)
        {
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
