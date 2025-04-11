using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        TitleScene,
        InGameScene
    }

    // 적 상태 <- ElementalEffect와 순서 및 내용 동일
    public enum  EnemyState
    {
        None,      // 없음
        Ignite,    // 점화
        Shock,     // 감전
    }

    // 원소 효과 <- EnemyState와 순서 및 내용 동일
    public enum ElementalEffect
    {
        None,      // 없음
        Ignite,    // 점화
        Shock,     // 감전
    }

    // 플레이어 마법 속성
    public enum Elemental
    {
        Flame,      // 화염
        Lightning,  // 번개
        Oil,        // 기름
        Frost       // 냉기
    }

    #region Sound
    public enum BGM
    {
        None,
        Main,
        Boss,
        GameOver
    }

    public enum Effect
    {

    }
    #endregion
}