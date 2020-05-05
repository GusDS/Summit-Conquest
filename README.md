# Summit-Conquest
Summit Conquest (Unity CwCL Prototype 4) Version 1.0
Unity Course: Create with Code Live
Week 5: Gameplay Mechanics | Apr 20 - 24 (2020)

## 1) Overview:
Based on the course prototype, I made a full game adding a lot of features built from the ground, learning A LOT in the process.
(from Course description):
This week, you will program an arcade-style Sumo battle with the objective of knocking increasingly difficult waves of enemies off of a floating island, using power ups to help defeat them. In creating this prototype, you will learn how to implement new gameplay mechanics into your projects, which are new rules or systems that make the game more interesting to play. On one hand, you will learn to program a powerup, which give the player a temporary advantage. On the other hand, you will learn to program increasingly difficult enemy waves, which make survival more challenging for the player. A good balance of powerups and increasing difficulty make for a much more interesting gameplay experience.  

## 2) Game Instructions:
Survive continually increasing enemies for 2 minutes. You have 3 lives to get the highest score.

Pick up Powerup X (multiply): Super Push for 10 seconds.
Pick up Powerup Flame: Charged Bomb. No time limit, detonate with space while jumping.
+1 life with each 20000 points mark.

Score:
        Small Enemy             100
        Big Enemy              2000
        Cannon Ball              50
        Super Push collect      200
        Charged Bomb collect    300
        Level win              5000 (+ 10 points x seconds)

Move Forward:		(keyboard) W or keyUp			/ (mouse) LeftClick
Move Backward:		(keyboard) S or keyDown
Turn Camera:		(keyboard) A+D or keyLeft+keyRight	/ (mouse) LeftRight Axis
Jump:			(keyboard) Space			/ (mouse) RightClick
Keyboard On/Off:	(keyboard) F1
Mouse On/Off:		(keyboard) F2

statistics:
Player                                  : HIST: hiscore / total score added / levels passed / seconds survived / lives lost
Enemy beaten (big, small, cannonball)   : Current game / total
Powerup collected                       : idem

## 3) Project work (features, modifications, settings):
- Project Settings: Graphics High (Tier 3) unchecked Cascade Shadows (recurring issue detected with objects' visible border).
- Environment: Lighting changed. FX_Mist particle system color and other modifications.
- UI: Added keyboard+mouse icons, input devices can be both set active/inactive through keys F1 and F2.
- UI: Complete information added: level, time, score + high score, lives. Save data locally.
- Statistics: collect and read/write from local system. Shows info in menu.
- Game Manager: built a "Control" idea before learning about a "Game Manager" :)
- Player: respawn when fall down, lives control + extra lives
- Game: time survive gameplay
- Enemy: Added enemy spawn frequency. Added different enemies (Small: fast, weak, pushable, heavy spawn / Big: slow, strong, few spawn, fire Cannon Balls).
- Enemy: Design, Textures and Animations finished.
- Powerups: Two powerups 
- Game: physics tuning for different masses and forces applied. Completed for player, enemies, powerups actions including explosions.
- Sound: Background music and Sound effects for almost every player and enemy actions.
- Menu: full options control with graphic design, shows full statistics, save data locally. Restart/Exit.
- Confirmation Dialog: hard work that can be improved, but a useful tool for all projects
- Title screen with Game Instructions.
- Documentation, version control, public release.

## 4) To Do:
- Code: Use Object Pooler for instantiate prefabs.
- Game: Difficulty progression (more time to survive, less powerups?, IA?).
- IA: Bigger enemies try to avoid falling down.
- Sound: some missing sounds (player/enemy entry, player fall, high score, victory, game over)
- Powerups: timer float counter.
- Instructions + Statistics: add icons and better design
- Control: tweak for gamepad control (assign rotate view to right stick)
- Fix:
        .Enemy: too much aceleration sometimes.
        .Audio slider not always plays sample sound right.
- Known Issues:
        .Physical forces in screen sometimes produces weird movements.
        .Enemies spotlights strange effect (covering a large area) at some angles
        .Statistics panel slow mouse wheel scroll
        .Code: some twisted code, product of learning on the go (mainly with parallel "Update" routines)

## 5) Code/Screenshots:

## 6) Resources and Links:
        ### Font
        https://www.1001freefonts.com/data-control.font
        ### Sounds and Music:
        https://freesound.org/people/montblanccandies/sounds/271623/            -- Music Background
        https://freesound.org/people/Raclure/sounds/483602/                     -- Bump
        https://freesound.org/people/Cman634/sounds/198784/                     -- Powerup Collect
        https://freesound.org/people/evan.schad/sounds/463214/                  -- Jump
        https://freesound.org/people/kennysvoice/sounds/158372/                 -- Powerup Push
        https://freesound.org/people/pumodi/sounds/150205/                      -- Powerup Explode
        https://freesound.org/people/ProjectsU012/sounds/340960/                -- Enemy Big Fire
        https://freesound.org/people/JohanDeecke/sounds/369529/                 -- Cannon Ball Explode
        https://freesound.org/people/KeyKrusher/sounds/154953/                  -- Beep
        ### Particle Effects:
        (Explosion) Dust, Shockwave and Shower, from Unity Standard Assets
        ### Mouse and Keyboard Icons
        https://www.pngfuel.com/free-png/guvkk
        https://www.clipartmax.com/png/full/114-1148459_%C2%A0-keyboard-icon-png-blue.png
        ### Kenney Assets (https://kenney.nl/). Icons:
        audioOn/audioOff
        musicOn/MusicOff
        grey_sliderUp/grey_sliderHorizontal
        grey_box
        grey_button03
        grey_checkmarkWhite
        red_cross

YouTube:
GitHub: https://github.com/GusDS/Summit-Conquest
