using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PFX
{
    public class ContentButton : MonoBehaviour
    {
        private Animator anim;
        private bool selected;
        public ItemData item;


        private void Awake()
        {
            anim = GetComponent<Animator>();
        }



        public void Selected()
        {
            selected = !selected;

            if (selected)
            {
                anim.SetBool("Selected", true);
                BuildingManager.I.UpdateSelectedPart(item);
            }
            else if (!selected)
            {
                anim.SetBool("Selected", false);
                BuildingManager.I.UpdateSelectedPart(item);
            }
        }

        public void Deselect()
        {
            anim.SetBool("Selected", false);
        }


        public void Deselected()
        {
            //anim.SetBool("Selected", false);
            //UIManager.I.UpdateToolbarIndex(0);
        }

        public void HoverEnter()
        {
            anim.SetBool("Hover", true);
            UIManager.I.mouseOverUI = true;
        }

        public void HoverExit()
        {
            anim.SetBool("Hover", false);
            UIManager.I.mouseOverUI = false;
        }
    }
}
