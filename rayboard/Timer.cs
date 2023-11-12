using static Raylib_cs.Raylib;

namespace userInputraylib
{
    /*
         Docs:
            - https://www.youtube.com/watch?v=vGlvTWUctTQ
    */
    public class CTimer
    {
        private static float org_time;
        private struct timer
        {
            public float Lifetime;
        }
        timer mytimer;
        public CTimer(float lifetime)
        {
            org_time = lifetime;
            mytimer.Lifetime = lifetime;
        }
        public void UpdateTimer()
        {
            if (mytimer.Lifetime > 0)
            {
                mytimer.Lifetime -= GetFrameTime();
            }
        }
        public void StopTimer()
        {
            mytimer.Lifetime = 0;
        }
        public float GetCurrentTime()
        {
            return mytimer.Lifetime;
        }
        public float RemainTime()
        {
            return org_time - mytimer.Lifetime;
        }
        public bool TimerDone()
        {
            return mytimer.Lifetime <= 0;
        }
    }
}
