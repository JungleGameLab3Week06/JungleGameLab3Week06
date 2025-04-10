using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public RhythmManager rhythmManager;
    public Transform spawnPoint;
    public Enemy enemyPrefab; // 인스펙터에서 연결할 적 오브젝트
    public Sprite[] tagSprites; // 태그별 스프라이트 배열 (인스펙터에서 설정)

    public string[] tags = { "화염", "기름", "냉기", "번개" };
    public TextMeshProUGUI judgeText; // TMP 사용 시
    public float judgeDisplayTime = 0.5f;
    Coroutine judgeCoroutine;
    Dictionary<(string, string), string> tagInteractions = new Dictionary<(string, string), string>
    {
        { ("기름", "화염"), "점화" },
        // { ("화염", "기름"), "점화" }, // 반대 순서 추가
        // { ("냉기", "번개"), "감전" },
        { ("번개", "냉기"), "감전" } // 반대 순서 추가
    };
    string allyTag;
    string playerTag;
    Enemy currentEnemy;
    bool isJudging = false;
    double judgeWindowStart;
    double judgeWindowEnd;
    double currentBeatTime = -1; // 현재 처리 중인 비트
    bool hasInputThisBeat = false; // 현재 비트에서 입력 여부

    void Awake()
    {
        if (rhythmManager == null)
            rhythmManager = FindAnyObjectByType<RhythmManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rhythmManager.OnBeat += HandleBeat;
        // 첫 적은 바로 스폰하지 않고 비트에서 처리
    }

    void HandleBeat()
    {
        if (currentEnemy == null)
        {
            SpawnEnemy();
        }
        else
        {
            currentEnemy.Move();

            double beatTime = rhythmManager.GetLastBeatTime();
            if (beatTime == currentBeatTime) return; // 동일 박자 중복 처리 방지

            float beatInterval = rhythmManager.GetBeatInterval(); // 큰 박자 간격 (BPM 기반)
            float window = beatInterval * 0.3f; // 성공 구간 (±0.3초, BPM 60 기준 0.6초)
            judgeWindowStart = beatTime - window;
            judgeWindowEnd = beatTime + window;
            Debug.Log($"[큰 박자] 비트: {beatTime:F4}, 간격: {beatInterval:F4}, 성공 구간: {judgeWindowStart:F4}~{judgeWindowEnd:F4}");

            isJudging = true;
            hasInputThisBeat = false; // 새 큰 박자 시작 시 입력 초기화
            currentBeatTime = beatTime;

            // 입력이 없으면 박자 끝에서 Miss
            if (!hasInputThisBeat && AudioSettings.dspTime >= judgeWindowEnd)
            {
                ShowJudge("Miss");
                Debug.Log("입력 없이 큰 박자 종료 - Miss");
                isJudging = false;
            }
        }

    }

    string GetTimingJudgement(double timeDiff)
    {
        float beatInterval = rhythmManager.GetBeatInterval();
        float perfectWindow = beatInterval * 0.15f; // ±0.15초 (BPM 60 기준 0.3초)
        float goodWindow = beatInterval * 0.3f;     // ±0.3초 (BPM 60 기준 0.6초)

        if (timeDiff <= perfectWindow) return "Perfect";
        if (timeDiff <= goodWindow) return "Good";
        return "Miss";
    }
    void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        allyTag = tags[Random.Range(0, tags.Length)];
        currentEnemy.SetPreviewTag(allyTag, tagSprites, tags); // 이번 태그만 표시
        Debug.Log($"적 등장! 동료 예고: {allyTag}");
    }
    public void SelectTag(string tag)
    {
        double now = AudioSettings.dspTime;

        if (!isJudging)
        {
            Debug.Log("[무시됨] 현재 큰 박자 아님");
            return;
        }

        if (hasInputThisBeat)
        {
            ShowJudge("Miss");
            Debug.Log("[즉시 Miss] 큰 박자 내 연속 입력");
            return;
        }

        playerTag = tag;
        double beatTime = rhythmManager.GetLastBeatTime();
        double diff = System.Math.Abs(now - beatTime);
        string judge = GetTimingJudgement(diff);

        ShowJudge(judge);
        if (judge != "Miss")
        {
            ExecuteTags();
        }
        else
        {
            Debug.Log($"[즉시 Miss] now: {now:F4} (성공 구간: {judgeWindowStart:F4}~{judgeWindowEnd:F4})");
        }

        hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }

    void ProcessInput(double beatTime, double inputTime, string tag)
    {
        playerTag = tag;
        double diff = System.Math.Abs(inputTime - beatTime);
        string judge = GetTimingJudgement(diff);

        ShowJudge(judge);
        if (judge != "Miss")
        {
            ExecuteTags();
        }
        else
        {
            Debug.Log($"[즉시 Miss] now: {inputTime:F4} (윈도우: {judgeWindowStart:F4}~{judgeWindowEnd:F4})");
        }
    }


    void ExecuteTags()
    {
        // 동료의 실제 태그 (70% 확률로 예고와 다름)
        string actualAllyTag = Random.value > 1f ? tags[Random.Range(0, tags.Length)] : allyTag;
        Debug.Log($"조합 체크: (동료: {actualAllyTag}, 플레이어: {playerTag})");
        string interaction = GetInteraction(actualAllyTag, playerTag);

        if (actualAllyTag != allyTag)
        {
            Debug.Log($"동료가 속였다! 예고: {allyTag}, 실제: {actualAllyTag}");
        }
        else
        {
            Debug.Log($"동료가 '{actualAllyTag}'를 발사했다!");
        }
        if (interaction != null)
        {
            Debug.Log($"반응 발생: {interaction}");
            ApplyInteraction(interaction);
        }
        else
        {
            Debug.Log($"조합 실패: {actualAllyTag} + {playerTag}는 정의된 반응 없음");
            currentEnemy.TakeDamage(10);
        }
        UpdateAllyPreviewTag();
    }
    private string GetInteraction(string tag1, string tag2)
    {
        if (tagInteractions.TryGetValue((tag1, tag2), out string interaction))
            return interaction;
        if (tagInteractions.TryGetValue((tag2, tag1), out interaction))
            return interaction;
        return null;
    }

    void UpdateAllyPreviewTag()
    {
        // 50% 확률로 예고 태그 변경
        if (Random.value > 0.5f)
        {
            allyTag = tags[Random.Range(0, tags.Length)]; // 새 태그로 업데이트
            string newAllyTag = tags[Random.Range(0, tags.Length)];
            if (newAllyTag != allyTag) // 이전과 다를 때만 업데이트
            {
                allyTag = newAllyTag;
                currentEnemy.SetPreviewTag(allyTag, tagSprites, tags); // 올바른 매개변수 전달
                Debug.Log($"동료 예고 변경: {allyTag}");
            }
        }
    }
    void ApplyInteraction(string interaction)
    {
        switch (interaction)
        {
            case "점화":
                currentEnemy.ApplyState("화상");
                currentEnemy.TakeDamage(70);
                break;
            case "감전":
                currentEnemy.ApplyState("감전");
                currentEnemy.TakeDamage(50);
                Debug.Log("감전감전50");
                break;
        }
    }

    void ShowJudge(string result)
    {
        if (judgeCoroutine != null)
            StopCoroutine(judgeCoroutine);

        judgeCoroutine = StartCoroutine(ShowJudgeCoroutine(result));
    }

    IEnumerator ShowJudgeCoroutine(string result)
    {
        judgeText.text = result;
        judgeText.gameObject.SetActive(true);

        // 색상 또는 효과 설정 (선택)
        switch (result)
        {
            case "Perfect":
                judgeText.color = Color.yellow;
                break;
            case "Good":
                judgeText.color = Color.green;
                break;
            case "Miss":
                judgeText.color = Color.red;
                break;
        }

        yield return new WaitForSeconds(judgeDisplayTime);
        judgeText.gameObject.SetActive(false);
    }


}