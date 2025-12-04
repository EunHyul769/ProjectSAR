using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        Enemy.OnBossDiedGlobal += HandleBossDied;
    }

    private void OnDisable()
    {
        Enemy.OnBossDiedGlobal -= HandleBossDied;
    }

    private void HandleBossDied(Enemy diedBoss)
    {
        if (diedBoss.EnemyData.enemyType == EnemyType.Boss)
        {
            SceneLoader.Load(SceneType.ResultScene);
        }
    }
}
