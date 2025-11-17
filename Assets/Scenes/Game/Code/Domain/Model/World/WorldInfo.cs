
using System.Collections.Generic;

[System.Serializable]
public class WorldInfo
{
    public DetailInfo Library;

    [System.Serializable]
    public class DetailInfo
    {
        public string title;

        public string description;

        public List<LevelSummaryInfo> levels;

        [System.Serializable]
        public class LevelSummaryInfo
        {
            public int pick;

            public List<string> choices;
        }
    }
}
