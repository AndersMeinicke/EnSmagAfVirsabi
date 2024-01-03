using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClasses
{
    [Serializable]
    public class TopScoreEntry : AzureTableStorageConnection.TableEntry
    {
        public string ChallengeID;
        public string UserName;
        public float SecondsUsed;
        public string HumainzedTime;
        public int Score;

        public TopScoreEntry(string userName, float secondsUsed, int score, string challengeID)
        {
            UserName = userName;
            SecondsUsed = secondsUsed;
           // HumainzedTime = VirsabiUtility.HumanizeTime(secondsUsed, VirsabiUtility.TimeFormat.MMSSmm);
            Score = score;
            ChallengeID = challengeID;
        }
    }
}
