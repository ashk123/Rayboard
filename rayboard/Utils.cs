namespace tamrinSnakemy
{
    /*
     * Docs:
     * - https://www.youtube.com/watch?v=_JjPo8rE8a8
     * - https://www.youtube.com/watch?v=KMK3ZOWxQTc
     * - https://github.com/ChrisDill/Raylib-cs/blob/master/Examples/Audio/SoundLoading.cs
    */
    internal class Utils
    {
        public struct PStatus
        {
            public string play_name;
            public float end_time;
            public string text;

        }
        public static List<PStatus> RuntimeStatusList = new List<PStatus>();
        private static string random_cash; // for cach the previes random sentence
        public static string GetRandomString(params string[] words)
        {
            while (true)
            {
                List<string> org = new List<string>();
                foreach (var word in words)
                {
                    org.Add(word);
                }
                Random rnd = new Random();
                int number = rnd.Next(0, org.Count);
                if (random_cash == org[number])
                {
                    continue;
                }
                else
                {
                    random_cash = org[number];
                    return org[number];

                }
            }
        }
        public static void SavePlayerStatus(string player_name, float end_time, string text)
        {
            PStatus new_status;
            new_status.text = text;
            new_status.play_name = player_name;
            new_status.end_time = end_time;
            RuntimeStatusList.Add(new_status);
        }
    }
}
