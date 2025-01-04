using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        private bool open=false;
        public Transform Player;

        private bool isTriggerPressed = false;

        void Start()
        {
            open = false;
        }

        void Update()
        {

            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 1.5 )
                {
					Debug.Log("Distanța între jucător și ușă: " + dist + " | Stare ușă: " + open);
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
