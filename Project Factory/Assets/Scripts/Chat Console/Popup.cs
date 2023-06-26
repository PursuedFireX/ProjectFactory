using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PFX
{
    public class Popup : MonoBehaviour
    {
        private bool isChat;
        private bool moveUp;
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Image bg;
        private float fadeTime;
        private float fadeSpd;

        private Color textColor;
        private Color bgColor;

        public static Popup Create(Vector3 pos, string text, float fadeTime, float fadeSpd, Color textColor, bool moveUp = false, bool isChat = false)
        {
            Transform pop = null;

            if (isChat)
                pop = Instantiate(AssetManager.I.chatPopup, GameManager.I.UITransform);
            else
                pop = Instantiate(AssetManager.I.textPopup).transform;

            pop.GetComponent<RectTransform>().localPosition = pos;
            Popup popup = pop.GetComponent<Popup>();
            popup.Setup(text, textColor);
            popup.fadeSpd = fadeSpd;
            popup.fadeTime = fadeTime;
            popup.isChat = isChat;
            popup.moveUp = moveUp;

            if (isChat)
            {
                if (ChatConsoleController.I.currentChatPopup != null)
                {
                    Destroy(ChatConsoleController.I.currentChatPopup.gameObject);
                }

                ChatConsoleController.I.currentChatPopup = pop.gameObject;
            }

            return popup;
        }

        public void Setup(string text, Color c, bool isChat = false)
        {
            textMesh.text = text;
            textMesh.color = c;

            if(isChat)
                bgColor = bg.color;
        }


        private void Update()
        {
            fadeTime -= Time.deltaTime;
            float spd = 5f;
            if(moveUp)
            {
                transform.position += new Vector3(0, spd) * Time.deltaTime;
            }

            if(!isChat)
            {
                transform.LookAt(Camera.main.transform);
                transform.localEulerAngles += new Vector3(0, 180, 0);
            }


            if(fadeTime < 0)
            {
                textColor.a -= fadeSpd * Time.deltaTime;
                textMesh.color = textColor;

                if (isChat)
                {
                    bgColor.a -= fadeSpd * Time.deltaTime;
                    bg.color = bgColor;

                    if (textColor.a <= 0 && bgColor.a <= 0)
                    {
                        ChatConsoleController.I.currentChatPopup = null;
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (textColor.a <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
