using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PFX
{
    public class ChatPopup : MonoBehaviour
    {

        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Image bg;
        private float fadeTime;
        private float fadeSpd;

        private Color textColor;
        private Color bgColor;

        public static ChatPopup Create(Vector2 pos, string text, float fadeTime, float fadeSpd, Color textColor)
        {
            Transform pop = Instantiate(AssetManager.I.chatPopup, GameManager.I.UITransform);
            pop.GetComponent<RectTransform>().localPosition = pos;
            ChatPopup popup = pop.GetComponent<ChatPopup>();
            popup.Setup(text, textColor);
            popup.fadeSpd = fadeSpd;
            popup.fadeTime = fadeTime;

            if(ChatConsoleController.I.currentChatPopup != null)
            {
                Destroy(ChatConsoleController.I.currentChatPopup.gameObject);
            }

            ChatConsoleController.I.currentChatPopup = pop.gameObject;


            return popup;
        }

        public void Setup(string text, Color c)
        {
            textMesh.text = text;
            textMesh.color = c;
            bgColor = bg.color;
        }

        private void Update()
        {
            fadeTime -= Time.deltaTime;
            if(fadeTime < 0)
            {
                textColor.a -= fadeSpd * Time.deltaTime;
                bgColor.a -= fadeSpd * Time.deltaTime;

                textMesh.color = textColor;
                bg.color = bgColor;

                if(textColor.a <= 0 && bgColor.a <= 0)
                {
                    ChatConsoleController.I.currentChatPopup = null;
                    Destroy(gameObject);
                }
            }
        }
    }
}
