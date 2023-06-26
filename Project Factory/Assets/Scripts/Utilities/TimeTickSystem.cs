using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public static class TimeTickSystem
    {
        public class OnTickEventArgs : EventArgs
        {
            public int tick;
        }

        public static event EventHandler<OnTickEventArgs> OnTick;

        private const float TICK_TIMER_MAX = .2f; //5 ticks per second.

        private static GameObject timeTickSystemGameObject;
        private static int tick;


        public static void Create()
        {
            if(timeTickSystemGameObject == null)
            {
                timeTickSystemGameObject = new GameObject("TimeTickSystem");
                timeTickSystemGameObject.AddComponent<TimeTickSystemObject>();

            }
        }

        public static int GetTick()
        {
            return tick;
        }

        private class TimeTickSystemObject : MonoBehaviour
        {
            private float tickTimer;

            private void Awake()
            {
                tick = 0;
            }

            private void Update()
            {
                tickTimer += Time.deltaTime;
                if (tickTimer >= TICK_TIMER_MAX)
                {
                    tickTimer -= TICK_TIMER_MAX;
                    tick++;
                    if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
                }
            }

        }


    }
}
