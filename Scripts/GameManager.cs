// =======================
// GameManager.cs
// =======================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int roundsToWin = 13;
    public int teamAScore = 0;
    public int teamBScore = 0;
    public float buyPhaseTime = 20f;
    public UIManager ui;
    public List<PlayerController> players;
    public int respawnTime = 5;

    void Start()
    {
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        while (teamAScore < roundsToWin && teamBScore < roundsToWin)
        {
            if (ui != null) ui.ShowBuyPhase(true, buyPhaseTime);
            yield return new WaitForSeconds(buyPhaseTime);
            if (ui != null) ui.ShowBuyPhase(false, 0);

            bool roundEnded = false;
            while (!roundEnded)
            {
                if (CheckTeamEliminated(out int winnerTeam))
                {
                    if (winnerTeam == 0) teamAScore++; else teamBScore++;
                    if (ui != null) ui.UpdateScore(teamAScore, teamBScore);
                    roundEnded = true;
                }
                yield return null;
            }

            yield return new WaitForSeconds(3f);
            RespawnAll();
        }

        if (ui != null) ui.ShowMatchEnd(teamAScore > teamBScore ? "Team A" : "Team B");
    }

    bool CheckTeamEliminated(out int winnerTeam)
    {
        winnerTeam = -1;
        int aliveA = 0, aliveB = 0;
        foreach (var p in players)
        {
            if (p == null) continue;
            var dmg = p.GetComponent<Damageable>();
            if (dmg != null && dmg.currentHP > 0)
            {
                if (p.CompareTag("TeamA")) aliveA++; else if (p.CompareTag("TeamB")) aliveB++;
            }
        }
        if (aliveA == 0 && aliveB > 0) { winnerTeam = 1; return true; }
        if (aliveB == 0 && aliveA > 0) { winnerTeam = 0; return true; }
        return false;
    }

    void RespawnAll()
    {
        foreach (var p in players)
        {
            if (p == null) continue;
            var dmg = p.GetComponent<Damageable>();
            if (dmg != null) { dmg.currentHP = dmg.maxHP; dmg.armor = 0; }
            var wm = p.GetComponentInChildren<WeaponManager>();
            if (wm != null)
            {
                foreach (var w in wm.slots) if (w != null) w.Init();
                if (wm.ui != null) wm.ui.UpdateWeaponUI(wm.slots);
            }
            // 스폰 포인트로 이동시키는 로직을 여기에 추가
        }
    }
}