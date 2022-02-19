using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TransitionLight : MonoBehaviour
{
    [SerializeField]
    private Material _transitionIn;
    void Start()
    {
        StartCoroutine(BeginTransition());
    }
    void Update()
    {
    }
    IEnumerator BeginTransition()
    {
        yield return Animate(_transitionIn, 1);
    }
    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Animate(Material material, float time)
    {
        float current = 0;
        GetComponent<Image>().material = material;
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
}

