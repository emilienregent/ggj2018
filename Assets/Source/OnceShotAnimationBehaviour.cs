using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceShotAnimationBehaviour : MonoBehaviour 
{
    public Animator animator = null;

    private void Start () 
    {
        StartCoroutine("OnCompleteAnimation");
	}
	
    IEnumerator OnCompleteAnimation()
    {
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        GameObject.Destroy(gameObject);
    }
}
