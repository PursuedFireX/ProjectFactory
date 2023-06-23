using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace PFX
{
    public static class Tools
    {

        public const int sortingOrderDefault = 5000;

        /// <summary>
        /// //Smoothly rotates any gameobject.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="targetRotation"></param>
        /// <param name="spd"></param>
        public static void SmoothRotate(Transform t, Vector3 targetRotation, float spd)
        {
            t.rotation = Quaternion.Lerp(t.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * spd);
        }

        /// <summary>
        /// //Updates the fill amount of a UI element. Used for slider functuanality with custom shapes. Make sure sprite mode is set to "Filled" in editor.
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="add"></param>
        /// <param name="currentAmount"></param>
        /// <param name="updateAmount"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float UpdateUIFill(Image fill, bool add, float currentAmount, int updateAmount, int maxValue)
        {
            if (add)
            {
                currentAmount += updateAmount;

                if (currentAmount > maxValue)
                    currentAmount = maxValue;
            }
            else
            {
                currentAmount -= updateAmount;

                if (currentAmount < 0)
                    currentAmount = 0;
            }

            fill.fillAmount = (float)currentAmount / maxValue;
            return currentAmount;
        }

        /// <summary>
        /// A timer that counts up and returns true once it equals the given max time.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="maxTime"></param>
        /// <param name="updatedTime"></param>
        /// <returns></returns>
        public static bool Timer(float timer, float maxTime, out float updatedTime)
        {
            timer += Time.deltaTime;
            updatedTime = timer;
            if (timer >= maxTime)
            {
                timer -= maxTime;
                updatedTime = timer;
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// //Gets a direction based on the given angle.
        /// </summary>
        /// <param name="eularY"></param>
        /// <param name="angleToDegrees"></param>
        /// <returns></returns>
        public static Vector3 DirectionFromAngle(float eularY, float angleToDegrees)
        {
            angleToDegrees += eularY;

            return new Vector3(Mathf.Sin(angleToDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleToDegrees * Mathf.Rad2Deg));
        }

        /// <summary>
        /// //Loads scene via the given index.
        /// </summary>
        /// <param name="sceneIndex"></param>
        public static void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// //Loads scene via the scene's name.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// //Closes the game.
        /// </summary>
        public static void Quit()
        {
            Application.Quit();
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetDirToMouse(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

    }
}
