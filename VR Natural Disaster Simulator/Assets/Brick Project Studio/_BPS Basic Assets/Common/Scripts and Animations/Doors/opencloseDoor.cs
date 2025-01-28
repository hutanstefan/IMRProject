using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        private Animator openandclose;
        private bool open=false;
        public Transform Player;

        private bool isTriggerPressed = false;

        void Start()
        {
            open = false;
            //Debug.Log($"Door {gameObject.name} is active");
            openandclose = GetComponent<Animator>();
            if (openandclose == null)
            {
                Debug.LogError($"[{gameObject.name}] Nu s-a găsit componenta Animator pe acest GameObject!");
            }
            else
            {
               // Debug.Log($"[{gameObject.name}] Animator detectat: {openandclose.runtimeAnimatorController.name}");
            }
        }

        void Update()
        {

            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 2.3 )
                {
					//Debug.Log("Distanța între jucător și ușă: " + dist + " | Stare ușă: " + open + " usa " + gameObject.name );
                    //Debug.Log($"[{gameObject.name}] Animator detectat: {openandclose.runtimeAnimatorController.name}");
                    if (open == false)
                    {
                        StartCoroutine(opening());
                    }
                }
				  else if (dist >= 1.5 && open == true) 
                {
                    StartCoroutine(closing()); 
					open=false;
                }
   
            }
        }


		
        IEnumerator opening()
        {
            print("You are opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing()
        {
            print("You are closing the door");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
