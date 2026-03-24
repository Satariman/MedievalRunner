# Medieval Runner — Architecture

Infinite runner in a medieval setting. Unity 6 (6000.3.9f1), HDRP, New Input System. Event-driven architecture with loose coupling between modules.

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/          # Game state management, performance init
│   ├── Input/         # Input abstraction layer
│   ├── Player/        # Movement, jump, health, animation
│   ├── World/         # Infinite track generation
│   ├── Obstacles/     # Obstacle types and spawning
│   ├── Bonuses/       # Bonus types and spawning
│   ├── Score/         # Score tracking
│   ├── Save/          # High score persistence
│   └── UI/            # HUD, menus, panels
├── Animations/        # PlayerAnimator.controller + clips
├── ScriptableObjects/
│   ├── Obstacles/     # BoulderData.asset, LogData.asset
│   └── Bonuses/       # HealthBonusData.asset, InvulnerabilityBonusData.asset
├── Prefabs/
│   ├── Boulder.prefab, Log.prefab
│   └── Bonuses/       # HealthBonus.prefab, InvulnerabilityBonus.prefab
├── Input/             # PlayerInputActions.inputactions
└── OutdoorsScene.unity
```

## Modules

### Core
- **GameManager** — Central coordinator. Manages `GameState` transitions (Menu → Playing → Paused → GameOver). Controls `Time.timeScale`: `0f` for Paused and GameOver, `1f` for Playing/Menu. Public API: `StartGame()`, `ResumeGame()`, `GoToMenu()`.
- **GameState** — Enum: `Menu`, `Playing`, `Paused`, `GameOver`.
- **PerformanceInitializer** — Sets Quality Level (Performant), VSync=0, targetFPS=60 in `Awake()`.

### Input
- **PlayerInputHandler** — Wraps New Input System (`Assets/Input/PlayerInputActions.inputactions`, Gameplay map). Exposes events: `OnMove(Vector2)`, `OnJumpPressed()`, `OnBoostStateChanged(bool)`, `OnPausePressed()` (ESC). Pause action is always active regardless of `SetInputEnabled`.

### Player
- **PlayerRoot** — Wiring component. Subscribes to PlayerInputHandler events and forwards to PlayerMovement and PlayerJumpController.
- **PlayerMovement** — Uses `CharacterController` for physics. Free horizontal movement. Handles gravity, air control multiplier.
- **PlayerJumpController** — Single jump with ground check. Extra gravity on descent. Event: `OnLanded`.
- **PlayerHealth** — HP with configurable max. `TakeDamage(int)` triggers i-frames. `Heal(int)` restores HP. `GrantInvulnerability(float)` grants timed invulnerability (stacks duration). Events: `OnHealthChanged`, `OnDamageTaken`, `OnDeath`, `OnHealed`, `OnInvulnerabilityGranted`.
- **PlayerFallDetector** — Monitors `transform.position.y` each frame. If Y drops below `fallThresholdY` (default `−2`), calls `PlayerHealth.TakeDamage(MaxHealth)` — instant death. Threshold is configurable via Inspector.
- **PlayerAnimationController** — Drives Animator (Damage/Bonus triggers, IsInvulnerable bool) and MaterialPropertyBlock color: red flash on damage, yellow flash on bonus, blue pulse while invulnerable.

### World
- **WorldMover** — Controls world scroll speed. `SetBoostActive(bool)` applies multiplier. `SetMovementEnabled(bool)`.
- **RunGenerator** — Moves segments and recycles in O(n) — tracks maxZ in main loop, batches recycles.
- **RunSegment** — Stores segment `Length`.

### Obstacles
- **ObstacleData** (ScriptableObject) — Config: Prefab, SpawnHeight, Speed, RotationSpeed, Damage, Lifetime.
- **ObstacleSpawner** — Timer-based spawning with per-type object pool. `SetSpawningEnabled(bool)`.
- **ObstacleBase** (abstract) — Lifecycle: Initialize → Update (move+rotate+lifetime) → Despawn (SetActive false). Collision via cached root GameObject reference. Events: `OnDespawn`.
- **BoulderObstacle** / **LogObstacle** — Concrete obstacle implementations.

### Bonuses
- **BonusData** (ScriptableObject) — Config: Prefab, SpawnHeight, Speed, Lifetime, GlowColor, PulseSpeed, EffectDuration, HealAmount.
- **BonusSpawner** — Timer-based spawning with per-type object pool. `SetSpawningEnabled(bool)`. Event: `OnBonusSpawned`.
- **BonusBase** (abstract) — Pickup via `OnTriggerEnter`. Pulsing translucent visual via MaterialPropertyBlock. Events: `OnCollected`.
- **HealthBonus** — Calls `PlayerHealth.Heal(HealAmount)`. Red translucent pulsing sphere.
- **InvulnerabilityBonus** — Calls `PlayerHealth.GrantInvulnerability(EffectDuration)`. Blue translucent pulsing sphere.

### Score
- **ScoreManager** — Accumulates score per second with boost multiplier. `ResetScore()` on restart. Property: `CurrentScore`. Event: `OnScoreChanged(int)`.

### Save
- **SaveManager** — Persists high score in `PlayerPrefs`. API: `TryUpdateHighScore(int)`, property `HighScore`.

### UI
- **UIManager** — Switches panels based on `GameManager.OnStateChanged`. On GameOver: updates high score via SaveManager.
- **MainMenuUI** — Title, developer name, high score, Play/Quit buttons.
- **PauseMenuUI** — Resume/Main Menu buttons. Shown when `GameState.Paused` (Time.timeScale=0).
- **GameOverUI** — Final score, high score, Play Again/Main Menu buttons.
- **UIScoreView** — Zero-alloc score update (TryFormat + SetCharArray).
- **UIHealthView** — Health display as "HP: current/max".

## Event Flows

```
Movement:     InputHandler.OnMove → PlayerRoot → PlayerMovement.SetMoveInput
Jump:         InputHandler.OnJumpPressed → PlayerRoot → JumpController.HandleJumpPressed
Boost:        InputHandler.OnBoostStateChanged → WorldMover + ScoreManager (2x)
Pause:        InputHandler.OnPausePressed → GameManager (toggle Playing↔Paused, timeScale)
Damage:       Obstacle.OnCollisionEnter → PlayerHealth.TakeDamage → OnDamageTaken → PlayerAnimationController
Heal:         Bonus.OnTriggerEnter → HealthBonus.ApplyEffect → PlayerHealth.Heal → OnHealed → PlayerAnimationController
Invulnerable: Bonus.OnTriggerEnter → InvulnerabilityBonus.ApplyEffect → PlayerHealth.GrantInvulnerability → OnInvulnerabilityGranted → PlayerAnimationController
Fall:         PlayerFallDetector (Y < -2) → PlayerHealth.TakeDamage(MaxHealth)
Death:        PlayerHealth.OnDeath → GameManager (GameOver, timeScale=0) → UIManager → SaveManager.TryUpdateHighScore
Score:        ScoreManager (per frame) → OnScoreChanged → UIScoreView
```

## Key Patterns
- **No singletons/static hubs** — standard C# `event Action` delegates, references wired via Inspector.
- **Inspector injection** — dependencies assigned in Unity Inspector, not service locators.
- **ScriptableObject configs** — all gameplay parameters are data-driven assets.
- **Component composition** — Player is split into Root/Movement/Jump/Health/AnimationController.
- **Object pooling** — ObstacleSpawner and BonusSpawner use per-type Queue pools; despawn = SetActive(false).
- **Extensibility** — new bonus types: inherit BonusBase, override ApplyEffect(), create BonusData asset; same pattern for obstacles.
