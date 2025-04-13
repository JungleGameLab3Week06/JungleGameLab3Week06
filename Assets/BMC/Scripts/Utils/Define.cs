using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        TitleScene,
        InGameScene,
        GameOverScene,
        GameClearScene
    }

    // 적 상태
    public enum  EnemyState
    {
        None,      // 없음
        Shock,     // 감전, 기절
    }

    // 원소 효과
    public enum ElementalEffect
    {
        Flame =     (1 << Elemental.Flame) | (1 << Elemental.Flame),          // 화염 = 화염 + 화염
        Water =     (1 << Elemental.Water) | (1 << Elemental.Water),          // 물 = 물 + 물
        Wall =      (1 << Elemental.Ground) | (1 << Elemental.Ground),        // 벽 = 땅 + 땅
        Lightning = (1 << Elemental.Lightning) | (1 << Elemental.Lightning),  // 번개 = 번개 + 번개
        Fog =       (1 << Elemental.Flame) | (1 << Elemental.Water),          // 안개 = 화염 물
        Ignition =  (1 << Elemental.Flame) | (1 << Elemental.Ground),         // 점화 = 화염 + 땅
        Diffusion =     (1 << Elemental.Flame) | (1 << Elemental.Lightning),  // 확산 = 화염 + 번개
        Grease =        (1 << Elemental.Water) | (1 << Elemental.Ground),     // 기름 = 물 + 땅
        ElectricShock = (1 << Elemental.Water) | (1 << Elemental.Lightning),  // 감전 = 물 + 번개
        None =          (1 << Elemental.Ground) | (1 << Elemental.Lightning), // 없음 = 땅 + 번개
    }

    // 플레이어 마법 속성
    public enum Elemental
    {
        Flame = 0,      // 화염
        Water = 1,      // 물
        Ground = 2,     // 땅
        Lightning = 3,  // 번개
        None
    }

    // 적 타입
    public enum EnemyType
    {
        Normal,    // 기본타입
        Special,   // 특수타입(강적)
        Confuse    // 혼란타입
    }

    #region Sound
    public enum BGM
    {
        None,
        Title,              // TitleScene
        Main,               // InGameScene
        Boss,               // BossScene
        GameOver            // GameOverScene
    }
    public enum Effect
    {
        BestElemental,      // 최고 공격
        BossDeath,          // 보스 사망
        BtnClick,           // 버튼 클릭
        EnemyDeath,         // 일반 적 사망
        GameStart,          // 게임 시작
        NormalElemental,    // 일반 공격
        PlayerCast,         // 플레이어 마법 시전
        PlayerDeath,        // 플레이어 사망
    }
    #endregion
}