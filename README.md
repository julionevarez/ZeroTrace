# ZERO TRACE
### *A 2D Top-Down Stealth Extraction Game*

> **Get in. Grab the loot. Leave no trace.**

Built in **Unity 2D** for CS 576 — Intro to 3D Game Programming (SDSU, Spring 2026).

---

## Overview

Zero Trace is a single-screen 2D top-down stealth game. You infiltrate a dark military facility, loot secured crates behind timed input minigames, and extract before the clock runs out — all while evading patrolling guards. Get spotted and you lose everything you've collected.

The core tension is simple: *do you grab one more crate, or do you run for the exit?*

Inspired by *Escape from Tarkov*, *Metal Gear Solid*, and *Splinter Cell*.

---

## Gameplay

- **Move** with `WASD`
- **Interact** with loot crates using `E`
- **Complete the minigame** — match the arrow key sequence before the timer bar runs out
- **Reach the extraction zone** and hold for **2 seconds** to exfil
- **Don't get caught** — guards have directional field of view and contact detection

### Win Condition
Carry at least one piece of loot to the extraction zone and hold position for 2 seconds before the global timer expires.

### Lose Conditions
- The 90-second countdown timer hits zero
- A guard spots you (detection wipes your carried loot and respawns you)

---

## Features

### Guard AI
- 4 guards with independent multi-waypoint patrol routes
- Directional field of view — guards only detect in front of them
- Raycast-based line-of-sight detection
- Ping-pong patrol mode for complex back-and-forth routes
- Contact detection — walking into a guard triggers an alert

### Loot Minigame
- Press `E` on any crate to trigger an arrow key sequence challenge
- Timer bar above each crate counts down independently
- Fail or time out → crate locks permanently for that run
- Success → loot added to your inventory with audio feedback

### Audio System
- Full `AudioManager` singleton with scene-persistent audio
- Iconic sound bytes sourced from *MGS* (alert), *RE4* (item pickup), *COD* (mission failed), and *GTA SA* (mission complete)
- Footstep loop, crate interaction, minigame success/fail, timer warning, and extraction sounds

### Visuals & UI
- Programmatically generated pixel art assets via **Python + PIL**
- Sci-fi tileset with wall/floor variants (Kenney assets)
- Directional player and guard sprites (4-directional)
- MGS-style global timer with color progression: green → orange → red
- `Press E` interaction prompts above loot containers
- Separate **Start Screen** scene with How To Play panel
- Win/lose screens with restart support

---

## Tech Stack

| Layer | Tools |
|---|---|
| Engine | Unity 6.3 LTS (Built-in Render Pipeline) |
| Language | C# |
| Asset Generation | Python 3, PIL (Pillow) |
| Version Control | Git / GitHub |
| Audio | Unity AudioSource + custom AudioManager |
| Art | Kenney Sci-Fi Tiles, Kenney Top-Down Shooter |

---

## Project Structure

```
ZeroTrace/
├── Assets/
│   ├── Audio/                    # Sound effects (.wav / .mp3)
│   ├── Scripts/                  # All C# game scripts
│   │   ├── PlayerController.cs   # WASD movement, interaction input
│   │   ├── GuardPatrol.cs        # Waypoint patrol, ping-pong mode
│   │   ├── GuardDetection.cs     # Raycast FOV + contact detection
│   │   ├── LootContainer.cs      # Crate state, E-prompt, loot logic
│   │   ├── LootMinigame.cs       # Arrow key sequence + timer bar
│   │   ├── ExtractionZone.cs     # 2-second hold exfil mechanic
│   │   ├── AudioManager.cs       # Singleton, scene-persistent audio
│   │   ├── GameManager.cs        # Win/lose state, scene management
│   │   ├── GameTimer.cs          # 90s countdown, color progression
│   │   └── StartScreenManager.cs # Start screen / How To Play panel
│   ├── Prefabs/
│   │   └── LootContainer         # Reusable loot crate prefab
│   ├── Sprites/                  # Pixel art sprites (PIL-generated + Kenney)
│   │   └── EmojiOne/             # Emoji sprite attribution
│   ├── Scenes/
│   │   ├── StartScreen.unity
│   │   └── GameScene.unity
│   └── Tilemaps/                 # Tilemap assets and ZeroTracePalette
│       ├── tile_floor_military
│       ├── tile_wall_military
│       ├── tile_wall_vertical
│       └── tile_wall_corner_(bl/br/tl/tr)
├── Fonts/
│   ├── NotoSans-VariableFont.wght
│   └── NotoSans2
└── changes.txt                   # Deviations from original design document
```

---

## Controls

| Key | Action |
|---|---|
| `W A S D` | Move |
| `E` | Interact with crate / start minigame |
| `← ↑ → ↓` | Arrow key minigame input |

---

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/julionevarez/ZeroTrace.git
   ```
2. Open in **Unity 6.3 LTS** via Unity Hub
3. Load `Assets/Scenes/StartScreen.unity`
4. Press **Play**

---

## Design Deviations (`changes.txt` summary)

| Original Plan | What Was Built |
|---|---|
| Simple trigger-based detection | Raycast + directional FOV system |
| 1 guard | 4 guards with independent patrol routes |
| Basic loot pickup | Full arrow key minigame with individual timers |
| Instant extraction on zone entry | 2-second hold mechanic for tension |
| Darkness/vision mask overlay | Cut — Built-in Render Pipeline limitation |
| Respawn on detection | Immediate loot wipe + respawn |

---

## Credits

- **Developer:** Julio Nevarez
- **Course:** CS 576 — Intro to 3D Game Programming, SDSU Spring 2026
- **Art Assets:** [Kenney.nl](https://kenney.nl) (CC0)
- **Sound Effects:** freesound.org / zapsplat.com
- **Iconic Audio:** *Metal Gear Solid*, *Resident Evil 4*, *Call of Duty*, *GTA San Andreas*
