
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpectateurController : NetworkBehaviour
{

    [SerializeField]
    private List<Animator> animators = new();
    private void Start()
    {
       
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectsWithTag("Clapeur").ToList().ForEach(clapeur => animators.Add(clapeur.GetComponent<Animator>()));
  
        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.SetTrigger("clapTrigger");
            } 
        }
    }
   
    public void OnTriggerExit(Collider other)
    {
        GameObject.FindGameObjectsWithTag("Clapeur").ToList().ForEach(clapeur => animators.Add(clapeur.GetComponent<Animator>()));

        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.SetTrigger("clapTrigger");
            }
        }
    }
}

