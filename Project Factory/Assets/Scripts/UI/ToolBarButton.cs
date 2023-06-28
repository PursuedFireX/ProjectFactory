using UnityEngine;
using UnityEngine.UI;

namespace PFX
{
    public class ToolBarButton : MonoBehaviour
    {
        private Animator anim;
        public int id;
        private bool selected;


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
                UIManager.I.UpdateToolbarIndex(id, this);
                
            }
            else if(!selected)
            {
                anim.SetBool("Selected", false);
                UIManager.I.UpdateToolbarIndex(0, this);
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
