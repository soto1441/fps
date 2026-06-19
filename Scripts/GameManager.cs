// =======================
// GameManager.cs
// =======================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int roundsToWin = 13;
    public int teamAScore = 0;
    public int teamBScore = 0;
    public float buyPhaseTime = 20f;
    public UIManager ui;
    public List<PlayerController> players = new List<PlayerController>();
    public int respawnTime = 5;
    public Transform[] teamASpawnPoints;
    public Transform[] teamBSpawnPoints;

    private bool gameRunning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // UIManager 찾기
        if (ui == null) ui = UIManager.Instance;
        if (ui == null) ui = FindObjectOfType<UIManager>();

        // 플레이어 자동으로 찾기
        if (players.Count == 0)
        {
            players.AddRange(FindObjectsOfType<PlayerController>());
        }

        // 스폰 포인트 자동으로 찾기
        if (teamASpawnPoints == null || teamASpawnPoints.Length == 0)
        {
            teamASpawnPoints = GameObject.Find("TeamASpawns")?.GetComponentsInChildren<Transform>();
        }
        if (teamBSpawnPoints == null || teamBSpawnPoints.Length == 0)
        {
            teamBSpawnPoints = GameObject.Find("TeamBSpawns")?.GetComponentsInChildren<Transform>();
        }

        Debug.Log($"게임 매니저 시작: {players.Count}명의 플레이어 감지");
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        gameRunning = true;
        int round = 1;

        while (teamAScore < roundsToWin && teamBScore < roundsToWin)
        {
            Debug.Log($"=== Round {round} 구매 페이즈 시작 ===");
            if (ui != null) ui.ShowRoundStart(round);
            if (ui != null) ui.ShowBuyPhase(true, buyPhaseTime);
            
            yield return new WaitForSeconds(buyPhaseTime);
            
            if (ui != null) ui.ShowBuyPhase(false, 0);
            Debug.Log($"=== Round {round} 시작 ===");

            // 모든 플레이어 부활
            RespawnAll();

            bool roundEnded = false;
            float roundTimer = 0f;
            float maxRoundTime = 300f; // 5분

            while (!roundEnded && roundTimer < maxRoundTime)
            {
                roundTimer += Time.deltaTime;

                if (CheckTeamEliminated(out int winnerTeam))
                {
                    if (winnerTeam == 0)
                    {
                        teamAScore++;
                        Debug.Log("Team A 승리!");
                    }
                    else
                    {
                        teamBScore++;
                        Debug.Log("Team B 승리!");
                    }

                    if (ui != null) ui.UpdateScore(teamAScore, teamBScore);
                    roundEnded = true;
                }
                yield return null;
            }

            if (!roundEnded)
            {
                Debug.Log("라운드 시간 초과 - 동점");
            }

            yield return new WaitForSeconds(3f);
            round++;
        }

        gameRunning = false;
        string winner = teamAScore > teamBScore ? "Team A" : "Team B";
        Debug.Log($"경기 종료: {winner} 승리! ({teamAScore} : {teamBScore})");
        if (ui != null) ui.ShowMatchEnd(winner);
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
                if (p.CompareTag("TeamA")) aliveA++;
                else if (p.CompareTag("TeamB")) aliveB++;
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
            if (dmg != null)
            {
                dmg.currentHP = dmg.maxHP;
                dmg.armor = 0;
            }

            // 무기 초기화
            var wm = p.GetComponentInChildren<WeaponManager>();
            if (wm != null)
            {
                foreach (var w in wm.slots)
                {
                    if (w != null) w.Init();
                }
                if (wm.ui != null) wm.ui.UpdateWeaponUI(wm.slots);
            }

            // 스폰 포인트로 이동
            if (p.CompareTag("TeamA") && teamASpawnPoints != null && teamASpawnPoints.Length > 0)
            {
                Transform spawn = teamASpawnPoints[Random.Range(0, teamASpawnPoints.Length)];
                p.transform.position = spawn.position;
                p.transform.rotation = spawn.rotation;
            }
            else if (p.CompareTag("TeamB") && teamBSpawnPoints != null && teamBSpawnPoints.Length > 0)
            {
                Transform spawn = teamBSpawnPoints[Random.Range(0, teamBSpawnPoints.Length)];
                p.transform.position = spawn.position;
                p.transform.rotation = spawn.rotation;
            }
        }
    }

    public bool IsGameRunning() => gameRunning;
}