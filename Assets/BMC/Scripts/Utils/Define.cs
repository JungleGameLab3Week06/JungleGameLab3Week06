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
        Shock,     // 감전, 기절
    }

    // 원소 효과 <- EnemyState와 순서 및 내용 동일
    public enum ElementalEffect
    {
        None,      // 없음
        ElectricShock, // 감전
        Fire,      // 화염
        Water,     // 물
        Ground,    // 땅
        Lightning, // 번개
        Wall,      // 벽
        Diffusion, // 확산
        Ignition,  // 점화
        Fog,       // 안개
        Grease,    // 기름
    }

    // 플레이어 마법 속성
    public enum Elemental
    {
        Flame,      // 화염
        Water,      // 물
        Ground,     // 땅
        Lightning,  // 번개
        None
    }
    public enum EnemyType
    {
        None,    //기본타입
        Special, //특수타입(강적)
        Confuse  //혼란타입
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