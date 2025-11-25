# FSMTest - Unity Finite State Machine Study Project

Unity에서 적 AI를 위한 FSM(Finite State Machine) 패턴을 연구하고 구현한 프로젝트입니다.

## 프로젝트 개요

이 프로젝트는 적(Enemy) AI의 행동을 FSM 패턴으로 제어하는 시스템을 구현합니다. 적은 플레이어를 감지하고 추적하며 공격하는 세 가지 상태를 전환하면서 동작합니다.

## 아키텍처

### 상태 머신 구조

```
EnemyStateMachine
    ├── EnemyState (Base Class)
    │   ├── EnemyIdleState
    │   ├── EnemyChaseState
    │   └── EnemyAttackState
    └── Enemy (Context)
```

### 핵심 컴포넌트

#### 1. State Machine Core

**EnemyStateMachine** (`EnemyStateMachine.cs`)
- 현재 상태를 추적하고 관리
- 상태 초기화 및 전환 기능 제공
- `Initialize()`: 초기 상태 설정
- `ChangeState()`: 상태 전환 시 ExitState → EnterState 호출

**EnemyState** (`EnemyState.cs`)
- 모든 상태의 베이스 클래스
- 상태별로 오버라이드할 수 있는 메서드 제공:
  - `EnterState()`: 상태 진입 시 호출
  - `ExitState()`: 상태 종료 시 호출
  - `FrameUpdate()`: 매 프레임 업데이트
  - `PhysicsUpdate()`: 물리 업데이트
  - `AnimationTriggerEvent()`: 애니메이션 이벤트 처리

#### 2. Enemy System

**Enemy** (`Enemy.cs`)
- 적 캐릭터의 기본 클래스
- 인터페이스 구현:
  - `IDamageable`: 체력 및 피해 처리
  - `IEnemyMoveable`: 이동 및 방향 전환
- 상태 머신 인스턴스 보유
- 플레이어 감지 상태 관리:
  - `IsAggroed`: 플레이어 인식 여부
  - `IsWithinStrikingDistance`: 공격 범위 내 여부

**Ghost** (`Ghost.cs`)
- Enemy 클래스를 상속받는 구체적인 적 타입
- 확장 가능한 구조 제공

#### 3. Concrete States

**EnemyIdleState** (`EnemyIdleState.cs`)
- 적이 대기 중일 때의 상태
- 무작위 위치로 배회
- `GetRandomPointInCircle()`: 범위 내 무작위 지점 생성
- 플레이어 감지 시 Chase 상태로 전환

**EnemyChaseState** (`EnemyChaseState.cs`)
- 플레이어를 추적하는 상태
- 플레이어 방향으로 이동
- 조건별 상태 전환:
  - 플레이어를 놓치면 → Idle 상태
  - 공격 범위에 도달하면 → Attack 상태

**EnemyAttackState** (`EnemyAttackState.cs`)
- 플레이어를 공격하는 상태
- 일정 간격으로 발사체 발사 (`_timeBetweenShots`)
- 플레이어가 멀어지면 Chase 상태로 복귀

#### 4. Interfaces

**IDamageable** (`IDamageable.cs`)
```csharp
- Damage(float damageAmount): 피해 처리
- Die(): 사망 처리
- MaxHealth, CurrentHealth: 체력 관리
```

**IEnemyMoveable** (`IEnemyMoveable.cs`)
```csharp
- MoveEnemy(Vector2 velocity): 적 이동
- CheckForLeftOrRightFacing(Vector2 velocity): 방향 전환
- RB, IsFacingRight: 이동 관련 프로퍼티
```

#### 5. Player

**Player** (`Player.cs`)
- Unity Input System 사용
- 기본 2D 이동 기능 구현
- Rigidbody2D 기반 물리 이동

## 상태 전환 다이어그램

```
     ┌─────────┐
     │  Idle   │◄──────────┐
     └────┬────┘           │
          │                │
          │ IsAggroed      │ !IsAggroed
          ▼                │
     ┌─────────┐           │
     │  Chase  │───────────┘
     └────┬────┘
          │
          │ IsWithinStrikingDistance
          ▼
     ┌─────────┐
     │ Attack  │◄───┐
     └────┬────┘    │
          │         │
          └─────────┘
       플레이어가 너무 멀어지면 Chase로 복귀
```

## 주요 기능

### 1. 상태 기반 AI
- 각 상태는 독립적인 로직을 가짐
- 명확한 전환 조건
- 확장 가능한 구조

### 2. 이동 및 방향 전환
- Rigidbody2D 기반 물리 이동
- 자동 좌우 반전 (Y축 회전)

### 3. 전투 시스템
- 체력 시스템 (IDamageable)
- 발사체 기반 공격
- 공격 범위 감지

### 4. 무작위 배회
- Idle 상태에서 범위 내 무작위 이동
- 부드러운 이동 패턴

## 코드 구조 특징

### 장점
1. **확장성**: 새로운 상태를 쉽게 추가 가능
2. **유지보수성**: 각 상태의 로직이 분리됨
3. **재사용성**: Interface 기반 설계
4. **Unity 친화적**: MonoBehaviour와 잘 통합됨

### 개선 가능한 점
1. 상태 전환 조건을 ScriptableObject로 관리
2. 애니메이션 시스템 완전 통합
3. Die() 메서드 구현
4. 객체 풀링 적용 (발사체)

## 파일 구조

```
Assets/Scripts/
├── Enemy/
│   ├── Base/
│   │   └── Enemy.cs
│   ├── Enemy Type/
│   │   └── Ghost.cs
│   ├── Interfaces/
│   │   ├── IDamageable.cs
│   │   └── IEnemyMoveable.cs
│   └── State Machine/
│       ├── EnemyState.cs
│       ├── EnemyStateMachine.cs
│       └── ConcreteStates/
│           ├── EnemyIdleState.cs
│           ├── EnemyChaseState.cs
│           └── EnemyAttackState.cs
└── Player/
    └── Player.cs
```

## 사용 방법

1. Enemy 게임오브젝트에 `Enemy` 또는 `Ghost` 컴포넌트 추가
2. Rigidbody2D 컴포넌트 필수
3. BulletPrefab 할당 (발사체)
4. 플레이어 태그를 "Player"로 설정
5. Trigger를 통해 `SetAggroStatus()` 및 `SetStrikingDistanceBool()` 호출

## 학습 포인트

이 프로젝트를 통해 다음을 학습할 수 있습니다:
- FSM 패턴의 실제 구현
- 객체지향 설계 원칙 (상속, 인터페이스)
- Unity의 Update/FixedUpdate 사이클
- 상태 기반 AI 로직 설계
- 코드 재사용성과 확장성 고려

## 요구사항

- Unity 2021.3 이상
- Unity Input System 패키지
