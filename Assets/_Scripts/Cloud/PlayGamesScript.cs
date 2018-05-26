﻿using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //Enabling Google services
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
	}
	

    void SignIn()
    {
        //log in
        Social.localUser.Authenticate(success => { });
    }

    #region Achievements

    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievement (string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }

    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }


    #endregion /Achievements



    #region Leaderboards

    public static void AddScoreToLeaderBoard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public static void ShowLeaderBoardsUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboards

}






