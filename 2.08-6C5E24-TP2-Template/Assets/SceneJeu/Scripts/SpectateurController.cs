using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpectateurController : NetworkBehaviour
{

    [SerializeField]
    private List<Animator> animators;
    private void Start()
    {
        GameObject.FindGameObjectsWithTag("Clapeur").ToList().ForEach(clapeur => animators.Add(clapeur.GetComponent<Animator>()));
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            foreach (Animator animator in animators)
            {
                if (animator != null)
                {
                    animator.SetTrigger("clapTrigger");
                }
            }
        }
    }    
}

