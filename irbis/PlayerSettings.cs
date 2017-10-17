using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

[Serializable]
public struct PlayerSettings
{   //start at line 11 to make it easier to count, just subtract 10 from final line
    public Point colliderOffset;
    public Point colliderSize;
    public float terminalVelocity;
    public float maxHealth;
    public float maxShield;
    public float maxEnergy;
    public float invulnerableMaxTime;
    public float energyUsableMargin;
    public float minSqrDetectDistance;
    public Vector2 initialPosition;
    public Rectangle boundingBox;
    public bool cameraLerpSetting;
    public float cameraLerpSpeed;
    public float shieldRechargeRate;
    public float energyRechargeRate;
    public float healthRechargeRate;
    public float potionRechargeRate;
    public float potionRechargeTime;
    public int maxNumberOfPotions;
    public float shieldHealingPercentage;
    public float shockwaveEffectiveDistance;
    public float shockwaveMaxEffectDistance;
    public float shockwaveStunTime;
    public Vector2 shockwaveKnockback;
    public float speed;
    public float jumpTimeMax;
    public float idleTimeMax;
    public float[] animationSpeed;
    public int[] animationFrames;
    public int[] characterWidth;
    public float shieldAnimationSpeed;
    public float superShockwaveHoldtime;
    public float walljumpHoldtime;
    public float attack1Damage;
    public float attack2Damage;
    public string timerAccuracy;
    public bool cameraShakeSetting;
    public bool cameraSwingSetting;
    public float swingDuration;
    public float swingMagnitude;
    public int attackColliderWidth;
    public int attackColliderHeight;
    public bool fullscreen;
    public float screenScale;
    public Point resolution;
    public bool vSync;
    public float masterAudioLevel;
    public float musicLevel;
    public float soundEffectsLevel;
    public Keys attackKey;
    public Keys altAttackKey;
    public Keys shockwaveKey;
    public Keys altShockwaveKey;
    public Keys shieldKey;
    public Keys altShieldKey;
    public Keys jumpKey;
    public Keys altJumpKey;
    public Keys upKey;
    public Keys altUpKey;
    public Keys downKey;
    public Keys altDownKey;
    public Keys leftKey;
    public Keys altLeftKey;
    public Keys rightKey;
    public Keys altRightKey;
    public Keys rollKey;
    public Keys altRollKey;
    public Keys potionKey;
    public Keys altPotionKey;
    public Keys useKey;
    public Keys altUseKey;
    public int characterHeight;
    public int debug;

    //start at line 11 to make it easier to count, just subtract 10 from final line
    static int numberOfVariables = 73;

    public PlayerSettings(bool useDefaults)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.PlayerSettings"); }
        if (useDefaults)
        {
            //lines beginning with ; are ignored
            //delete this file and relaunch to return everything to defaults

            //SETTINGS

            //KEY BINDS
            //a list of all available keys can be found here:
            // https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
            attackKey = Keys.Enter;
            altAttackKey = Keys.Enter;
            shockwaveKey = Keys.E;
            altShockwaveKey = Keys.E;
            shieldKey = Keys.Q;
            altShieldKey = Keys.Q;
            jumpKey = Keys.Space;
            altJumpKey = Keys.Space;
            upKey = Keys.W;
            altUpKey = Keys.Up;
            downKey = Keys.S;
            altDownKey = Keys.Down;
            leftKey = Keys.A;
            altLeftKey = Keys.Left;
            rightKey = Keys.D;
            altRightKey = Keys.Right;
            rollKey = Keys.LeftShift;
            altRollKey = Keys.LeftShift;
            potionKey = Keys.F;
            altPotionKey = Keys.F;
            useKey = Keys.R;
            altUseKey = Keys.R;

            //CAMERA SETTINGS
            //The camera will move when the player leaves this area on the screen
            //{int X location, int Y location, int Width, int Height}
            //where location refers to the center of the rectangle
            boundingBox = Rectangle.Empty;

            //Do you want the camera to smoothly trail the player?
            cameraLerpSetting = true;

            //How fast should the camera lerp?
            cameraLerpSpeed = 15f;

            //Turn off camera "swing" (the motion the camera makes when you attack and miss)
            cameraSwingSetting = false;
            
            //How far should the camera swing?
            swingMagnitude = 10f;

            //How long should that swing take (this is the time it takes to travel the above pixels,
            //the time it takes to return to the normal camera position is double this number) (seconds)
            swingDuration = 0.1f;

            //Turn off camera shake
            cameraShakeSetting = true;


            //VIDEO SETTINGS
            //toggles windowed/fullscreen mode
            fullscreen = false;

            //the screenScale of the window
            screenScale = 0;

            //how much will be drawn
            resolution = Point.Zero;
            //the actual size of the window is a combination of resolution and scale
            //for example, a 960x540 resolution at 2x scale gives you a 1920x1080window
            //with each in-game pixel using 2x2 pixels on your screen

            vSync = false;


            //AUDIO SETTINGS
             masterAudioLevel = 100f;
                   musicLevel = 100f;
            soundEffectsLevel = 100f;


            //MISC SETTINGS
            //How accurate is the timer (digits after the seconds' decimal)
            timerAccuracy = "00.00";

            //how long before the player is considered idle (seconds)
            idleTimeMax = 30f;

            //This is the time it takes to "charge" the super shockwave (seconds)
            //Basically, how long you have to hold the shockwaveKey to use
            superShockwaveHoldtime = 0.15f;

            //The amount of time the player can hold down the left or right movement key
            //during a wall jump before they drift away from the wall (seconds)
            walljumpHoldtime = 0.1f;

            //minimum distance(squared) for an enemy health bar to appear(bosses override this)
            minSqrDetectDistance = 1000000f;


            //CHEATS
            //self-explanatory
            attack1Damage = 45f;
            attack2Damage = 45f;

            //run&jump speed
            speed = 275f;
            //The maximum speed you can travel in any given direction
            terminalVelocity = 5000f;
            //how long your jumps will maintain jump velocity (seconds)
            jumpTimeMax = 0.25f;


            //highly unrecommended to not mess with these unless you really know what you're doing
            //collider size and placement relative to the 128x128 player sprite
            colliderOffset = new Point(54, 63);
            colliderSize = new Point(20, 48);

            //this is the size of the rectangle used as the main attack hitbox
            attackColliderWidth = 40;
            attackColliderHeight = 30;

            //self-explanatory
            maxHealth = 100f;
            maxShield = 50f;
            maxEnergy = 50f;

            //the distance at which the shockwave stops gaining power (otherwise it would be insanely strong close-up)
            shockwaveMaxEffectDistance = 50f;
            //distance at which shockwave has no power
            shockwaveEffectiveDistance = 100f;
            //shockwave multipliers
            shockwaveStunTime = 50f;
            shockwaveKnockback = new Vector2(5f, -2.5f);

            //this how the player won't take damage after previously taking damage (seconds)
            invulnerableMaxTime = 0.1f;

            //self-explanatory (per second)
            shieldRechargeRate = 5f;        //2f //4f
            energyRechargeRate = 10f;       //5f //10f
            healthRechargeRate = 0.5f;      //0f
            potionRechargeRate = 15f;
            potionRechargeTime = 3f;
            maxNumberOfPotions = 3;

            //this is the percentage of the damage absorbed by the shield that is converted back into health
            //0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)
            shieldHealingPercentage = 0.25f;

            //how full the energy bar has to be before allowing you to use (95f == 95%)
            energyUsableMargin = 0.95f;

            //ANIMATION SETTINGS
            //the amount of time that is allowed to pass before the animator displays the next frame (seconds)
            //(for each animation listed below)
            animationSpeed = new float[30];
            for (int i = 0; i < animationSpeed.Length; i++)
            {
                animationSpeed[i] = 0.1f;
            }
            //animationSpeed[19] = 0.3f;
            //animationSpeed[20] = 0.3f;

            // 0 is 1 frame, 1 is 2 frames, etc
            //the number of frames in each animation, only edit this if you are remaking the default spritesheet
            animationFrames = new int[30];
            animationFrames[00] = 3;            //
            animationFrames[01] = 15;           //
            animationFrames[02] = 3;            //
            animationFrames[03] = 15;            //
            animationFrames[04] = 15;           //
            animationFrames[05] = 15;            //
            animationFrames[06] = 15;           //
            animationFrames[07] = 9;            //
            animationFrames[08] = 9;            //
            animationFrames[09] = 5;            //
            animationFrames[10] = 5;            //
            animationFrames[11] = 0;            //
            animationFrames[12] = 0;            //
            animationFrames[13] = 1;            //
            animationFrames[14] = 1;            //
            animationFrames[15] = 0;            //
            animationFrames[16] = 0;            //
            animationFrames[17] = 0;            //
            animationFrames[18] = 0;            //
            animationFrames[19] = 2;            //
            animationFrames[20] = 2;            //
            for (int i = 21; i < animationFrames.Length; i++)
            {
                animationFrames[i] = 0;
            }

            //the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)
            //NOTE: there is no variable for the number of frames in the shield animation, as the shield animator
            //uses the width of the shield sprite to determine when to loop.
            shieldAnimationSpeed = 0.05f;

            //FONT SETTINGS
            //the height of each character in the font spritesheet (pixels)
            characterHeight = 12;
            //the width of each character in the font spritesheet (pixels)
            characterWidth = new int[100];
            int[] twosCol = { 18, 44, 62, 82, 83, 86 };
            int[] threesCol = { 47, 84, 85, 87, 90, 91, 93 };
            int[] foursCol = { 1, 45, 70, 71, 80, 81, 94 };
            int[] fivesCol = { 41, 88, 92 };
            int[] sixesCol = { 5, 14, 15, 19, 21, 28, 54, 55, 57, 59, 60, 63, 65, 67, 72, 73, 75, 76, 77 };
            int[] sevensCol = { 0, 2, 3, 4, 6, 7, 8, 9, 11, 12, 13, 16, 17, 20, 23, 24, 25, 26, 27, 30, 35, 36, 37, 38, 39, 40, 42, 43, 46, 49, 50, 51, 52, 53, 56, 61, 74, 78 };
            int[] eightsCol = { 10, 29, 31, 32, 33, 34, 48, 68, 69, 99 };
            int[] ninesCol = { 64, 79 };
            int[] tensCol = { 22, 58, 89, 95 };
            int[] elevenCol = { 66 };
            int[] twelveCol = { 32 };
            foreach (int i in characterWidth)
            {
                characterWidth[i] = -1;
            }
            foreach (int i in twosCol)
            {
                characterWidth[i] = 2;
            }
            foreach (int i in threesCol)
            {
                characterWidth[i] = 3;
            }
            foreach (int i in foursCol)
            {
                characterWidth[i] = 4;
            }
            foreach (int i in fivesCol)
            {
                characterWidth[i] = 5;
            }
            foreach (int i in sixesCol)
            {
                characterWidth[i] = 6;
            }
            foreach (int i in sevensCol)
            {
                characterWidth[i] = 7;
            }
            foreach (int i in eightsCol)
            {
                characterWidth[i] = 8;
            }
            foreach (int i in ninesCol)
            {
                characterWidth[i] = 9;
            }
            foreach (int i in tensCol)
            {
                characterWidth[i] = 10;
            }
            foreach (int i in elevenCol)
            {
                characterWidth[i] = 11;
            }
            foreach (int i in twelveCol)
            {
                characterWidth[i] = 12;
            }


            //ETC SETTINGS
            //player starting position(world space)
            //only used when one isn't given by the fight
            initialPosition = new Vector2(64f, 64f);

            //DEBUG MODE
            debug = 0;
        }
        else
        {
            //lines beginning with ; are ignored
            //delete this file and relaunch to return everything to defaults

            //KEY BINDS
            //a list of all available keys can be found here:
            // https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
            attackKey = Keys.Enter;
            altAttackKey = Keys.Enter;
            shockwaveKey = Keys.E;
            altShockwaveKey = Keys.E;
            shieldKey = Keys.Q;
            altShieldKey = Keys.Q;
            jumpKey = Keys.Space;
            altJumpKey = Keys.Space;
            upKey = Keys.W;
            altUpKey = Keys.Up;
            downKey = Keys.S;
            altDownKey = Keys.Down;
            leftKey = Keys.A;
            altLeftKey = Keys.Left;
            rightKey = Keys.D;
            altRightKey = Keys.Right;
            rollKey = Keys.LeftShift;
            altRollKey = Keys.LeftShift;
            potionKey = Keys.F;
            altPotionKey = Keys.F;
            useKey = Keys.R;
            altUseKey = Keys.R;

            //CAMERA SETTINGS
            //The camera will move when the player leaves this area on the screen
            //{int X location, int Y location, int Width, int Height}
            //where location refers to the center of the rectangle
            boundingBox = Rectangle.Empty;

            //Do you want the camera to smoothly trail the player?
            cameraLerpSetting = false;

            //How fast should the camera lerp?
            cameraLerpSpeed = 0f;

            //Turn off camera "swing" (the motion the camera makes when you attack and miss)
            cameraSwingSetting = false;

            //How far should the camera swing?
            swingMagnitude = 0f;

            //How long should that swing take (this is the time it takes to travel the above pixels,
            //the time it takes to return to the normal camera position is double this number) (seconds)
            swingDuration = 0f;

            //Turn off camera shake
            cameraShakeSetting = false;


            //VIDEO SETTINGS
            //toggles windowed/fullscreen mode
            fullscreen = false;

            //the scale of the window
            screenScale = 0;

            //how much will be drawn
            resolution = Point.Zero;
            //the actual size of the window is a combination of resolution and scale
            //for example, a 960x540 resolution at 2x scale gives you a 1920x1080window
            //with each in-game pixel using 2x2 pixels on your screen

            vSync = false;


            //AUDIO SETTINGS
             masterAudioLevel = 0f;
                   musicLevel = 0f;
            soundEffectsLevel = 0f;


            //MISC SETTINGS
            //How accurate is the timer (digits after the seconds' decimal)
            timerAccuracy = "00.";

            //how long before the player is considered idle (seconds)
            idleTimeMax = 0f;

            //This is the time it takes to "charge" the super shockwave (seconds)
            //Basically, how long you have to hold the shockwaveKey to use
            superShockwaveHoldtime = 0f;

            //The amount of time the player can hold down the left or right movement key
            //during a wall jump before they drift away from the wall (seconds)
            walljumpHoldtime = 0f;

            //minimum distance(squared) for an enemy health bar to appear(bosses override this)
            minSqrDetectDistance = 0f;


            //CHEATS
            //self-explanatory
            attack1Damage = 0f;
            attack2Damage = 0f;

            //run&jump speed
            speed = 0f;
            //The maximum speed you can travel in any given direction
            terminalVelocity = 0f;
            //how long your jumps will maintain jump velocity (seconds)
            jumpTimeMax = 0f;


            //highly unrecommended to not mess with these unless you really know what you're doing
            //collider size and placement relative to the 128x128 player sprite
            colliderSize = colliderOffset = Point.Zero;

            //this is the size of the rectangle used as the main attack hitbox
            attackColliderWidth = 0;
            attackColliderHeight = 0;

            //self-explanatory
            maxHealth = 0f;
            maxShield = 0f;
            maxEnergy = 0f;

            //the distance at which the shockwave stops gaining power (otherwise it would be insanely strong close-up)
            shockwaveMaxEffectDistance = 0f;
            //distance at which shockwave has no power
            shockwaveEffectiveDistance = 0f;
            //shockwave multipliers
            shockwaveStunTime = 0f;
            shockwaveKnockback = new Vector2(0f, 0f);

            //this how the player won't take damage after previously taking damage (seconds)
            invulnerableMaxTime = 0f;

            //self-explanatory (per second)
            shieldRechargeRate = 0f;        //2f //4f
            energyRechargeRate = 0f;       //5f //10f
            healthRechargeRate = 0f;      //0f
            potionRechargeRate = 0f;
            potionRechargeTime = 0f;
            maxNumberOfPotions = 0;

            //this is the percentage of the damage absorbed by the shield that is converted back into health
            //0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)
            shieldHealingPercentage = 0f;

            //how full the energy bar has to be before allowing you to use (95f == 95%)
            energyUsableMargin = 0f;





            //ANIMATION SETTINGS
            //the amount of time that is allowed to pass before the animator displays the next frame (seconds)
            //(for each animation listed below)
            animationSpeed = new float[0];
            for (int i = 0; i < animationSpeed.Length; i++)
            {
                animationSpeed[i] = 0f;
            }

            // 0 is 1 frame, 1 is 2 frames, etc
            //the number of frames in each animation, only edit this if you are remaking the default spritesheet
            animationFrames = new int[0];
            for (int i = 0; i < animationFrames.Length; i++)
            {
                animationFrames[i] = 0;
            }

            //the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)
            //NOTE: there is no variable for the number of frames in the shield animation, as the shield animator
            //uses the width of the shield sprite to determine when to loop.
            shieldAnimationSpeed = 0.05f;

            //FONT SETTINGS
            //the height of each character in the font spritesheet (pixels)
            characterHeight = 12;
            //the width of each character in the font spritesheet (pixels)
            characterWidth = new int[0];
            for (int i = 0; i < characterWidth.Length; i++)
            {
                characterWidth[i] = 0;
            }

            //ETC SETTINGS
            //player starting position(world space)
            //only used when one isn't given by the fight
            initialPosition = new Vector2(0f, 0f);

            //DEBUG MODE
            debug = 1;
        }
    }

    public PlayerSettings(PlayerSettings settings)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.PlayerSettings"); }
        //lines beginning with ; are ignored
        //delete this file and relaunch to return everything to defaults

        //key binds
        //a list of all available keys can be found here:
        // https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
        attackKey = settings.attackKey;
        altAttackKey = settings.altAttackKey;
        shockwaveKey = settings.shockwaveKey;
        altShockwaveKey = settings.altShockwaveKey;
        shieldKey = settings.shieldKey;
        altShieldKey = settings.altShieldKey;
        jumpKey = settings.jumpKey;
        altJumpKey = settings.altJumpKey;
        upKey = settings.upKey;
        altUpKey = settings.altUpKey;
        downKey = settings.downKey;
        altDownKey = settings.altDownKey;
        leftKey = settings.leftKey;
        altLeftKey = settings.altLeftKey;
        rightKey = settings.rightKey;
        altRightKey = settings.altRightKey;
        rollKey = settings.rollKey;
        altRollKey = settings.altRollKey;
        potionKey = settings.potionKey;
        altPotionKey = settings.altPotionKey;
        useKey = settings.useKey;
        altUseKey = settings.altUseKey;

        //SETTINGS
        //CAMERA SETTINGS
        //The camera will move when the player leaves this area on the screen
        //{int X location, int Y location, int Width, int Height}
        //where location refers to the center of the rectangle
        boundingBox = settings.boundingBox;

        //Do you want the camera to smoothly trail the player?
        cameraLerpSetting = settings.cameraLerpSetting;

        //How fast should the camera lerp?
        cameraLerpSpeed = settings.cameraLerpSpeed;

        //Turn off camera "swing" (the motion the camera makes when you attack and miss)
        cameraSwingSetting = settings.cameraSwingSetting;

        //How far should the camera swing?
        swingMagnitude = settings.swingMagnitude;

        //How long should that swing take (this is the time it takes to travel the above pixels,
        //the time it takes to return to the normal camera position is double this number) (seconds)
        swingDuration = settings.swingDuration;

        //Turn off camera shake
        cameraShakeSetting = settings.cameraShakeSetting;


        //VIDEO SETTINGS
        //toggles windowed/fullscreen mode
        fullscreen = settings.fullscreen;

        //the screenScale of the window
        screenScale = settings.screenScale;

        //how much will be drawn
        resolution = settings.resolution;
        //the actual size of the window is a combination of resolution and scale
        //for example, a 960x540 resolution at 2x scale gives you a 1920x1080window
        //with each in-game pixel using 2x2 pixels on your screen

        vSync = settings.vSync;


        //AUDIO SETTINGS
        masterAudioLevel = settings.masterAudioLevel;
        musicLevel = settings.musicLevel;
        soundEffectsLevel = settings.soundEffectsLevel;


        //MISC SETTINGS
        //How accurate is the timer (digits after the seconds' decimal)
        timerAccuracy = settings.timerAccuracy;

        //how long before the player is considered idle (seconds)
        idleTimeMax = settings.idleTimeMax;

        //This is the time it takes to "charge" the super shockwave (seconds)
        //Basically, how long you have to hold the shockwaveKey to use
        superShockwaveHoldtime = settings.superShockwaveHoldtime;

        //The amount of time the player can hold down the left or right movement key
        //during a wall jump before they drift away from the wall (seconds)
        walljumpHoldtime = settings.walljumpHoldtime;

        //minimum distance(squared) for an enemy health bar to appear(bosses override this)
        minSqrDetectDistance = settings.minSqrDetectDistance;


        //CHEATS
        //self-explanatory
        attack1Damage = settings.attack1Damage;
        attack2Damage = settings.attack2Damage;

        //run&jump speed
        speed = settings.speed;
        //The maximum speed you can travel in any given direction
        terminalVelocity = settings.terminalVelocity;
        //how long your jumps will maintain jump velocity (seconds)
        jumpTimeMax = settings.jumpTimeMax;

        //highly unrecommended to not mess with these unless you really know what you're doing
        //collider size and placement relative to the 128x128 player sprite
        colliderOffset = settings.colliderOffset;
        colliderSize = settings.colliderSize;

        //this is the size of the rectangle used as the main attack hitbox
        attackColliderWidth = settings.attackColliderWidth;
        attackColliderHeight = settings.attackColliderHeight;

        //self-explanatory
        maxHealth = settings.maxHealth;
        maxShield = settings.maxShield;
        maxEnergy = settings.maxEnergy;

        //the distance at which the shockwave stops gaining power (otherwise it would be insanely strong close-up)
        shockwaveMaxEffectDistance = settings.shockwaveMaxEffectDistance;
        //distance at which shockwave has no power
        shockwaveEffectiveDistance = settings.shockwaveEffectiveDistance;
        //shockwave multipliers
        shockwaveStunTime = settings.shockwaveStunTime;
        shockwaveKnockback = settings.shockwaveKnockback;

        //this how the player won't take damage after previously taking damage (seconds)
        invulnerableMaxTime = settings.invulnerableMaxTime;

        //self-explanatory (per second)
        shieldRechargeRate = settings.shieldRechargeRate;
        energyRechargeRate = settings.energyRechargeRate;
        healthRechargeRate = settings.healthRechargeRate;
        potionRechargeRate = settings.potionRechargeRate;
        potionRechargeTime = settings.potionRechargeTime;
        maxNumberOfPotions = settings.maxNumberOfPotions;


        //this is the percentage of the damage absorbed by the shield that is converted back into health
        //0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)
        shieldHealingPercentage = settings.shieldHealingPercentage;

        //how full the energy bar has to be before allowing you to use (95f == 95%)
        energyUsableMargin = settings.energyUsableMargin;
        
        //player starting position(world space)
        //only used when one isn't given by the fight
        initialPosition = settings.initialPosition;
        
        //ANIMATION SETTINGS
        //the amount of time that is allowed to pass before the animator displays the next frame (seconds)
        //(for each animation listed below)
        animationSpeed = settings.animationSpeed;

        // 0 is 1 frame, 1 is 2 frames, etc
        //the number of frames in each animation, only edit this if you are remaking the default spritesheet
        animationFrames = settings.animationFrames;

        //the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)
        //NOTE: there is no variable for the number of frames in the shield animation, as the shield animator
        //uses the width of the shield sprite to determine when to loop.
        shieldAnimationSpeed = settings.shieldAnimationSpeed;

        //FONT SETTINGS
        //the height of each character in the font spritesheet (pixels)
        characterHeight = settings.characterHeight;
        //the width of each character in the font spritesheet (pixels)
        characterWidth = settings.characterWidth;

        //DEBUG MODE
        debug = settings.debug;
    }

    public static void Save(PlayerSettings settings, string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.Save"); }
        Irbis.Irbis.WriteLine("saving " + filename + "...");
        StreamWriter writer = new StreamWriter(filename);

        //do writing here
        //;lines beginning with ; are ignored
        writer.WriteLine(";lines beginning with \";\" and empty lines are ignored");
        writer.WriteLine(";delete this file and relaunch to return everything to defaults");
        writer.WriteLine("");
        writer.WriteLine("");


        writer.WriteLine(";SETTINGS");
        writer.WriteLine(";KEY BINDS");
        writer.WriteLine(";a list of all available keys can be found here:");
        writer.WriteLine("; https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx");
        writer.WriteLine("attackKey=" + settings.attackKey);
        writer.WriteLine("altAttackKey=" + settings.altAttackKey);
        writer.WriteLine("");
        writer.WriteLine("shockwaveKey=" + settings.shockwaveKey);
        writer.WriteLine("altShockwaveKey=" + settings.altShockwaveKey);
        writer.WriteLine("");
        writer.WriteLine("shieldKey=" + settings.shieldKey);
        writer.WriteLine("altShieldKey=" + settings.altShieldKey);
        writer.WriteLine("");
        writer.WriteLine("jumpKey=" + settings.jumpKey);
        writer.WriteLine("altJumpKey=" + settings.altJumpKey);
        writer.WriteLine("");
        writer.WriteLine("upKey=" + settings.upKey);
        writer.WriteLine("altUpKey=" + settings.altUpKey);
        writer.WriteLine("");
        writer.WriteLine("downKey=" + settings.downKey);
        writer.WriteLine("altDownKey=" + settings.altDownKey);
        writer.WriteLine("");
        writer.WriteLine("leftKey=" + settings.leftKey);
        writer.WriteLine("altLeftKey=" + settings.altLeftKey);
        writer.WriteLine("");
        writer.WriteLine("rightKey=" + settings.rightKey);
        writer.WriteLine("altRightKey=" + settings.altRightKey);
        writer.WriteLine("");
        writer.WriteLine("rollKey=" + settings.rollKey);
        writer.WriteLine("altRollKey=" + settings.altRollKey);
        writer.WriteLine("");
        writer.WriteLine("potionKey=" + settings.potionKey);
        writer.WriteLine("altPotionKey=" + settings.altPotionKey);
        writer.WriteLine("");
        writer.WriteLine("useKey=" + settings.useKey);
        writer.WriteLine("altUseKey=" + settings.altUseKey);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CAMERA SETTINGS");
        writer.WriteLine(";The camera will move when the player leaves this area on the screen");
        writer.WriteLine(";{int X location, int Y location, int Width, int Height})");
        writer.WriteLine(";where location refers to the center of the rectangle");
        writer.WriteLine("boundingBox={X:" + settings.boundingBox.Center.X + " Y:" + settings.boundingBox.Center.Y + " Width:" + settings.boundingBox.Width + " Height:" + settings.boundingBox.Height + "}");
        writer.WriteLine("");
        writer.WriteLine(";Do you want the camera to smoothly trail the player?");
        writer.WriteLine("cameraLerpSetting=" + settings.cameraLerpSetting);
        writer.WriteLine("");
        writer.WriteLine(";How fast should the camera lerp?");
        writer.WriteLine("cameraLerpSpeed=" + settings.cameraLerpSpeed);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera \"swing\" (the motion the camera makes when you attack and miss)");
        writer.WriteLine("cameraSwingSetting=" + settings.cameraSwingSetting);
        writer.WriteLine("");
        writer.WriteLine(";How far should the camera swing? (pixels)");
        writer.WriteLine("swingMagnitude=" + settings.swingMagnitude);
        writer.WriteLine("");
        writer.WriteLine(";How long should that swing take (this is the time it takes to travel the above pixels,");
        writer.WriteLine(";the time it takes to return to the normal camera position is double this number) (seconds)");
        writer.WriteLine("swingDuration=" + settings.swingDuration);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera shake");
        writer.WriteLine("cameraShakeSetting=" + settings.cameraShakeSetting);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";VIDEO SETTINGS");
        writer.WriteLine(";toggles windowed/fullscreen mode");
        writer.WriteLine("fullscreen=" + settings.fullscreen);
        writer.WriteLine("");
        writer.WriteLine(";the scale of the window");
        writer.WriteLine("screenScale=" + settings.screenScale);
        writer.WriteLine("");
        writer.WriteLine(";how much of the world will be drawn");
        writer.WriteLine("resolution=" + settings.resolution);
        writer.WriteLine(";the actual size of the window is a combination of resolution and scale");
        writer.WriteLine(";for example, a 960x540 resolution at 2x scale gives you a 1920x1080 window");
        writer.WriteLine(";with each in-game pixel using 2x2 pixels on your screen");
        writer.WriteLine("");
        writer.WriteLine("vSync=" + settings.vSync);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";AUDIO SETTINGS");
        writer.WriteLine("masterAudioLevel=" + settings.masterAudioLevel);
        writer.WriteLine("musicLevel=" + settings.musicLevel);
        writer.WriteLine("soundEffectsLevel=" + settings.soundEffectsLevel);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";MISC SETTINGS");
        writer.WriteLine(";How accurate is the timer (digits after the seconds' decimal)");
        writer.WriteLine("timerAccuracy=" + (settings.timerAccuracy.Length - 3));
        writer.WriteLine("");
        writer.WriteLine(";How long before the player is considered idle (seconds)");
        writer.WriteLine("idleTimeMax=" + settings.idleTimeMax);
        writer.WriteLine("");
        writer.WriteLine(";This is the time it takes to \"charge\" the super shockwave (seconds)");
        writer.WriteLine(";Basically, how long you have to hold the shockwaveKey to use");
        writer.WriteLine("superShockwaveHoldtime=" + settings.superShockwaveHoldtime);
        writer.WriteLine("");
        writer.WriteLine(";The amount of time the player can hold down the left or right movement key");
        writer.WriteLine(";during a wall jump before they drift away from the wall (seconds)");
        writer.WriteLine("walljumpHoldtime=" + settings.walljumpHoldtime);
        writer.WriteLine("");
        writer.WriteLine(";Minimum distance(squared) for an enemy health bar to appear(bosses override this)");
        writer.WriteLine("minSqrDetectDistance=" + settings.minSqrDetectDistance);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CHEATS");
        writer.WriteLine(";self-explanatory");
        writer.WriteLine("attack1Damage=" + settings.attack1Damage);
        writer.WriteLine("attack2Damage=" + settings.attack2Damage);
        writer.WriteLine("");

        writer.WriteLine(";run&jump speed");
        writer.WriteLine("speed=" + settings.speed);
        writer.WriteLine(";The maximum speed you can travel in any given direction");
        writer.WriteLine("terminalVelocity=" + settings.terminalVelocity);
        writer.WriteLine(";how long your jumps will maintain jump velocity (seconds)");
        writer.WriteLine("jumpTimeMax=" + settings.jumpTimeMax);
        writer.WriteLine("");

        writer.WriteLine(";highly unrecommended to not mess with these unless you really know what you're doing");
        writer.WriteLine(";collider size and placement relative to the 128x128 player sprite");
        writer.WriteLine("colliderOffset=" + settings.colliderOffset);
        writer.WriteLine("colliderSize=" + settings.colliderSize);
        writer.WriteLine("");

        writer.WriteLine(";this is the size of the rectangle used as the main attack hitbox");
        writer.WriteLine("attackColliderWidth=" + settings.attackColliderWidth);
        writer.WriteLine("attackColliderHeight=" + settings.attackColliderHeight);
        writer.WriteLine("");

        writer.WriteLine(";self-explanatory");
        writer.WriteLine("maxHealth=" + settings.maxHealth);
        writer.WriteLine("maxShield=" + settings.maxShield);
        writer.WriteLine("maxEnergy=" + settings.maxEnergy);
        writer.WriteLine("");



        writer.WriteLine(";the distance at which the shockwave stops gaining power (otherwise it would be insanely strong close-up)");
        writer.WriteLine("shockwaveMaxEffectDistance=" + settings.shockwaveMaxEffectDistance);
        writer.WriteLine(";distance at which shockwave has no power");
        writer.WriteLine("shockwaveEffectiveDistance=" + settings.shockwaveEffectiveDistance);
        writer.WriteLine(";shockwave multipliers");
        writer.WriteLine("shockwaveStunTime=" + settings.shockwaveStunTime);
        writer.WriteLine("shockwaveKnockback=" + settings.shockwaveKnockback);
        writer.WriteLine("");

        writer.WriteLine(";this how the player won't take damage after previously taking damage (seconds)");
        writer.WriteLine("invulnerableMaxTime=" + settings.invulnerableMaxTime);
        writer.WriteLine("");

        writer.WriteLine(";these are only the base. they may be modified in-game by potions and other ways (per second)");
        writer.WriteLine("shieldRechargeRate=" + settings.shieldRechargeRate);
        writer.WriteLine("energyRechargeRate=" + settings.energyRechargeRate);
        writer.WriteLine("healthRechargeRate=" + settings.healthRechargeRate);
        writer.WriteLine("potionRechargeRate=" + settings.potionRechargeRate);
        writer.WriteLine("potionRechargeTime=" + settings.potionRechargeTime);
        writer.WriteLine("maxNumberOfPotions=" + settings.maxNumberOfPotions);
        writer.WriteLine("");

        writer.WriteLine(";this is the percentage of the damage absorbed by the shield that is converted back into health");
        writer.WriteLine(";0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)");
        writer.WriteLine("shieldHealingPercentage=" + settings.shieldHealingPercentage);
        writer.WriteLine("");

        writer.WriteLine(";how full the energy bar has to be before allowing you to use (95 == 95%)");
        writer.WriteLine("energyUsableMargin=" + settings.energyUsableMargin);
        writer.WriteLine("");
        //writer.WriteLine("");

        writer.WriteLine(";ANIMATION SETTINGS");
        writer.WriteLine(";the amount of time that is allowed to pass before the animator displays the next frame (seconds)");
        writer.WriteLine(";(for each animation listed below)");
        for (int i = 00; i < settings.animationSpeed.Length; i++)
        {
            writer.WriteLine("animationSpeed[" + i + "]=" + settings.animationSpeed[i]);
        }
        writer.WriteLine("");


        writer.WriteLine(";0 is 1 frame, 1 is 2 frames, etc");
        writer.WriteLine(";the number of frames in each animation, only edit this if you are remaking the default spritesheet");
        for (int i = 00; i < settings.animationFrames.Length; i++)
        {
            writer.WriteLine("animationFrames[" + i + "]=" + settings.animationFrames[i]);
        }
        writer.WriteLine("");


        writer.WriteLine(";the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)");
        writer.WriteLine(";NOTE: there is no variable for the number of frames in the shield animation, as the shield animator");
        writer.WriteLine(";uses the width of the shield sprite to determine when to loop.");
        writer.WriteLine("shieldAnimationSpeed=" + settings.shieldAnimationSpeed);


        writer.WriteLine(";FONT SETTINGS");
        writer.WriteLine(";the height of each character in the font spritesheet (pixels)");
        writer.WriteLine("characterHeight=" + settings.characterHeight);
        writer.WriteLine(";the width of each character in the font spritesheet (pixels)");
        for (int i = 00; i < 100; i++)
        {
            writer.WriteLine("characterWidth[" + i + "]=" + settings.characterWidth[i]);
        }
        writer.WriteLine(";characterWidth[99] is used as SPACE, so it should remain blank on the spritesheet (it is the final character)");
        writer.WriteLine("");
        writer.WriteLine("");



        writer.WriteLine(";ETC SETTINGS");
        writer.WriteLine(";player starting position (world space)");
        writer.WriteLine(";only used when one isn't given by the fight");
        writer.WriteLine("initialPosition=" + settings.initialPosition);
        writer.WriteLine("");

        writer.WriteLine(";DEBUG MODE");
        writer.WriteLine("debug=" + settings.debug);
        writer.WriteLine("");
        writer.WriteLine(";//END");




        writer.Close();
        Irbis.Irbis.WriteLine("save successful.");
        Irbis.Irbis.WriteLine();
    }

    public static void Save(Irbis.Irbis game, string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.Save"); }
        Irbis.Irbis.WriteLine("saving " + filename + "...");
        StreamWriter writer = new StreamWriter(filename);

        //do writing here
        //;lines beginning with ; are ignored
        writer.WriteLine(";lines beginning with \";\" and empty lines are ignored");
        writer.WriteLine(";delete this file and relaunch to return everything to defaults");
        writer.WriteLine("");
        writer.WriteLine("");


        writer.WriteLine(";SETTINGS");
        writer.WriteLine(";KEY BINDS");
        writer.WriteLine(";a list of all available keys can be found here:");
        writer.WriteLine("; https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx");
        writer.WriteLine("attackKey=" + Irbis.Irbis.attackKey);
        writer.WriteLine("altAttackKey=" + Irbis.Irbis.altAttackKey);
        writer.WriteLine("");
        writer.WriteLine("shockwaveKey=" + Irbis.Irbis.shockwaveKey);
        writer.WriteLine("altShockwaveKey=" + Irbis.Irbis.altShockwaveKey);
        writer.WriteLine("");
        writer.WriteLine("shieldKey=" + Irbis.Irbis.shieldKey);
        writer.WriteLine("altShieldKey=" + Irbis.Irbis.altShieldKey);
        writer.WriteLine("");
        writer.WriteLine("jumpKey=" + Irbis.Irbis.jumpKey);
        writer.WriteLine("altJumpKey=" + Irbis.Irbis.altJumpKey);
        writer.WriteLine("");
        writer.WriteLine("upKey=" + Irbis.Irbis.upKey);
        writer.WriteLine("altUpKey=" + Irbis.Irbis.altUpKey);
        writer.WriteLine("");
        writer.WriteLine("downKey=" + Irbis.Irbis.downKey);
        writer.WriteLine("altDownKey=" + Irbis.Irbis.altDownKey);
        writer.WriteLine("");
        writer.WriteLine("leftKey=" + Irbis.Irbis.leftKey);
        writer.WriteLine("altLeftKey=" + Irbis.Irbis.altLeftKey);
        writer.WriteLine("");
        writer.WriteLine("rightKey=" + Irbis.Irbis.rightKey);
        writer.WriteLine("altRightKey=" + Irbis.Irbis.altRightKey);
        writer.WriteLine("");
        writer.WriteLine("rollKey=" + Irbis.Irbis.rollKey);
        writer.WriteLine("altRollKey=" + Irbis.Irbis.altRollKey);
        writer.WriteLine("");
        writer.WriteLine("potionKey=" + Irbis.Irbis.potionKey);
        writer.WriteLine("altPotionKey=" + Irbis.Irbis.altPotionKey);
        writer.WriteLine("");
        writer.WriteLine("useKey=" + Irbis.Irbis.useKey);
        writer.WriteLine("altUseKey=" + Irbis.Irbis.altUseKey);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CAMERA SETTINGS");
        writer.WriteLine(";The camera will move when the player leaves this area on the screen");
        writer.WriteLine(";XNA rectangle sctructure: (int X location, int Y location, int Width, int Height)");
        writer.WriteLine(";where location refers to the center of the rectangle");
        writer.WriteLine("boundingBox={X:" + Irbis.Irbis.boundingBox.Center.X + " Y:" + Irbis.Irbis.boundingBox.Center.Y + " Width:" + Irbis.Irbis.boundingBox.Width + " Height:" + Irbis.Irbis.boundingBox.Height + "}");
        writer.WriteLine("");
        writer.WriteLine(";Do you want the camera to smoothly trail the player?");
        writer.WriteLine("cameraLerpSetting=" + Irbis.Irbis.cameraLerpSetting);
        writer.WriteLine("");
        writer.WriteLine(";How fast should the camera lerp?");
        writer.WriteLine("cameraLerpSpeed=" + Irbis.Irbis.cameraLerpSpeed);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera \"swing\" (the motion the camera makes when you attack and miss)");
        writer.WriteLine("cameraSwingSetting=" + Irbis.Irbis.cameraSwingSetting);
        writer.WriteLine("");
        writer.WriteLine(";How far should the camera swing? (pixels)");
        writer.WriteLine("swingMagnitude=" + Irbis.Irbis.swingMagnitude);
        writer.WriteLine("");
        writer.WriteLine(";How long should that swing take (this is the time it takes to travel the above pixels,");
        writer.WriteLine(";the time it takes to return to the normal camera position is double this number) (seconds)");
        writer.WriteLine("swingDuration=" + Irbis.Irbis.swingDuration);
        writer.WriteLine("");
        writer.WriteLine(";Turn off camera shake");
        writer.WriteLine("cameraShakeSetting=" + Irbis.Irbis.cameraShakeSetting);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";VIDEO SETTINGS");
        writer.WriteLine(";toggles windowed/fullscreen mode");
        writer.WriteLine("fullscreen=" + Irbis.Irbis.graphics.IsFullScreen);
        writer.WriteLine("");
        writer.WriteLine(";the scale of the window");
        if (Irbis.Irbis.screenScale != Irbis.Irbis.resolution.X / 480f)
        { writer.WriteLine("screenScale=" + Irbis.Irbis.screenScale); }
        else
        { writer.WriteLine("screenScale=" + 0); }
        writer.WriteLine("");
        writer.WriteLine(";how much of the world will be drawn");
        writer.WriteLine("resolution=" + Irbis.Irbis.tempResolution);
        writer.WriteLine(";the actual size of the window is a combination of resolution and scale");
        writer.WriteLine(";for example, a 960x540 resolution at 2x scale gives you a 1920x1080 window");
        writer.WriteLine(";with each in-game pixel using 2x2 pixels on your screen");
        writer.WriteLine("");
        writer.WriteLine("vSync=" + game.IsFixedTimeStep);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";AUDIO SETTINGS");
        writer.WriteLine("masterAudioLevel=" + Irbis.Irbis.masterAudioLevel);
        writer.WriteLine("musicLevel=" + Irbis.Irbis.musicLevel);
        writer.WriteLine("soundEffectsLevel=" + Irbis.Irbis.soundEffectsLevel);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";MISC SETTINGS");
        writer.WriteLine(";How accurate is the timer (digits after the seconds' decimal)");
        writer.WriteLine("timerAccuracy=" + (Irbis.Irbis.timerAccuracy.Length - 3));
        writer.WriteLine("");
        writer.WriteLine(";How long before the player is considered idle (seconds)");
        writer.WriteLine("idleTimeMax=" + Irbis.Irbis.jamie.idleTimeMax);
        writer.WriteLine("");
        writer.WriteLine(";This is the time it takes to \"charge\" the super shockwave (seconds)");
        writer.WriteLine(";Basically, how long you have to hold the shockwaveKey to use");
        writer.WriteLine("superShockwaveHoldtime=" + Irbis.Irbis.jamie.superShockwaveHoldtime);
        writer.WriteLine("");
        writer.WriteLine(";The amount of time the player can hold down the left or right movement key");
        writer.WriteLine(";during a wall jump before they drift away from the wall (seconds)");
        writer.WriteLine("walljumpHoldtime=" + Irbis.Irbis.jamie.walljumpHoldtime);
        writer.WriteLine("");
        writer.WriteLine(";Minimum distance(squared) for an enemy health bar to appear(bosses override this)");
        writer.WriteLine("minSqrDetectDistance=" + Irbis.Irbis.minSqrDetectDistance);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine(";CHEATS");
        writer.WriteLine(";self-explanatory");
        writer.WriteLine("attack1Damage=" + Irbis.Irbis.jamie.attack1Damage);
        writer.WriteLine("attack2Damage=" + Irbis.Irbis.jamie.attack2Damage);
        writer.WriteLine("");

        writer.WriteLine(";run&jump speed");
        writer.WriteLine("speed=" + Irbis.Irbis.jamie.speed);
        writer.WriteLine(";The maximum speed you can travel in any given direction");
        writer.WriteLine("terminalVelocity=" + Irbis.Irbis.jamie.terminalVelocity);
        writer.WriteLine(";how long your jumps will maintain jump velocity (seconds)");
        writer.WriteLine("jumpTimeMax=" + Irbis.Irbis.jamie.jumpTimeMax);
        writer.WriteLine("");

        writer.WriteLine(";highly unrecommended to not mess with these unless you really know what you're doing");
        writer.WriteLine(";collider size and placement relative to the 128x128 player sprite");
        writer.WriteLine("colliderOffset=" + Irbis.Irbis.jamie.colliderOffset);
        writer.WriteLine("colliderSize=" + Irbis.Irbis.jamie.colliderSize);
        writer.WriteLine("");

        writer.WriteLine(";this is the size of the rectangle used as the main attack hitbox");
        writer.WriteLine("attackColliderWidth=" + Irbis.Irbis.jamie.attackColliderWidth);
        writer.WriteLine("attackColliderHeight=" + Irbis.Irbis.jamie.attackColliderHeight);
        writer.WriteLine("");

        writer.WriteLine(";self-explanatory");
        writer.WriteLine("maxHealth=" + Irbis.Irbis.jamie.maxHealth);
        writer.WriteLine("maxShield=" + Irbis.Irbis.jamie.maxShield);
        writer.WriteLine("maxEnergy=" + Irbis.Irbis.jamie.maxEnergy);
        writer.WriteLine("");



        writer.WriteLine(";the distance at which the shockwave stops gaining power (otherwise it would be insanely strong close-up)");
        writer.WriteLine("shockwaveMaxEffectDistance=" + Irbis.Irbis.jamie.shockwaveMaxEffectDistance);
        writer.WriteLine(";distance at which shockwave has no power");
        writer.WriteLine("shockwaveEffectiveDistance=" + Irbis.Irbis.jamie.shockwaveEffectiveDistance);
        writer.WriteLine(";shockwave multipliers");
        writer.WriteLine("shockwaveStunTime=" + Irbis.Irbis.jamie.shockwaveStunTime);
        writer.WriteLine("shockwaveKnockback=" + Irbis.Irbis.jamie.shockwaveKnockback);
        writer.WriteLine("");

        writer.WriteLine(";this how the player won't take damage after previously taking damage (seconds)");
        writer.WriteLine("invulnerableMaxTime=" + Irbis.Irbis.jamie.invulnerableMaxTime);
        writer.WriteLine("");

        writer.WriteLine(";these are only the base. they may be modified in-game by potions and other ways (per second)");
        writer.WriteLine("shieldRechargeRate=" + Irbis.Irbis.jamie.shieldRechargeRate);
        writer.WriteLine("energyRechargeRate=" + Irbis.Irbis.jamie.energyRechargeRate);
        writer.WriteLine("healthRechargeRate=" + Irbis.Irbis.jamie.baseHealing);
        writer.WriteLine("potionRechargeRate=" + Irbis.Irbis.jamie.potionRechargeRate);
        writer.WriteLine("potionRechargeTime=" + Irbis.Irbis.jamie.potionRechargeTime);
        writer.WriteLine("maxNumberOfPotions=" + Irbis.Irbis.jamie.maxNumberOfPotions);
        writer.WriteLine("");

        writer.WriteLine(";this is the percentage of the damage absorbed by the shield that is converted back into health");
        writer.WriteLine(";0.25 means 25% of damage absorbed by the shield is converted to health (if 20 damage is absorbed, 5 damage is healed)");
        writer.WriteLine("shieldHealingPercentage=" + Irbis.Irbis.jamie.shieldHealingPercentage);
        writer.WriteLine("");

        writer.WriteLine(";how full the energy bar has to be before allowing you to use (95 == 95%)");
        writer.WriteLine("energyUsableMargin=" + Irbis.Irbis.jamie.energyUsableMargin);
        writer.WriteLine("");
        //writer.WriteLine("");

        writer.WriteLine(";ANIMATION SETTINGS");
        writer.WriteLine(";the amount of time that is allowed to pass before the animator displays the next frame (seconds)");
        writer.WriteLine(";(for each animation listed below)");
        for (int i = 00; i < Irbis.Irbis.jamie.animationSpeed.Length; i++)
        {
            writer.WriteLine("animationSpeed[" + i + "]=" + Irbis.Irbis.jamie.animationSpeed[i]);
        }
        writer.WriteLine("");


        writer.WriteLine(";0 is 1 frame, 1 is 2 frames, etc");
        writer.WriteLine(";the number of frames in each animation, only edit this if you are remaking the default spritesheet");
        for (int i = 00; i < Irbis.Irbis.jamie.animationFrames.Length; i++)
        {
            writer.WriteLine("animationFrames[" + i + "]=" + Irbis.Irbis.jamie.animationFrames[i]);
        }
        writer.WriteLine("");


        writer.WriteLine(";the amount of time that is allowed to pass before the shield animator displays the next frame (seconds)");
        writer.WriteLine(";NOTE: there is no variable for the number of frames in the shield animation, as the shield animator");
        writer.WriteLine(";uses the width of the shield sprite to determine when to loop.");
        writer.WriteLine("shieldAnimationSpeed=" + Irbis.Irbis.jamie.shieldAnimationSpeed);


        writer.WriteLine(";FONT SETTINGS");
        writer.WriteLine(";the height of each character in the font spritesheet (pixels)");
        writer.WriteLine("characterHeight=" + Irbis.Irbis.font.charHeight);
        writer.WriteLine(";the width of each character in the font spritesheet (pixels)");
        for (int i = 00; i < 100; i++)
        {
            writer.WriteLine("characterWidth[" + i + "]=" + Irbis.Irbis.font.charWidth[i]);
        }
        writer.WriteLine(";characterWidth[99] is used as SPACE, so it should remain blank on the spritesheet (it is the final character)");
        writer.WriteLine("");
        writer.WriteLine("");



        writer.WriteLine(";ETC SETTINGS");
        writer.WriteLine(";player starting position (world space)");
        writer.WriteLine(";only used when one isn't given by the fight");
        writer.WriteLine("initialPosition=" + Irbis.Irbis.initialPos);
        writer.WriteLine("");

        writer.WriteLine(";DEBUG MODE");
        writer.WriteLine("debug=" + Irbis.Irbis.debug);
        writer.WriteLine("");
        writer.WriteLine(";//END");




        writer.Close();
        Irbis.Irbis.WriteLine("save successful.");
        Irbis.Irbis.WriteLine();
    }

    public static PlayerSettings Load(string filename)                                          //CHANGE THIS BEFORE SHIPPING 
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.Load"); }
        Irbis.Irbis.WriteLine("loading " + filename + "...");

        List<bool> checker = new List<bool>();
        //filename = ".\\Content\\" + filename;
        PlayerSettings playerSettings = new PlayerSettings(true);                              //CHANGE THIS TO TRUE BEFORE SHIPPING

        Stream stream = TitleContainer.OpenStream(filename);
        StreamReader reader = new StreamReader(stream);

        string line = string.Empty;
        string errorVars = string.Empty;
        List<int> animationFrames = new List<int>();
        List<float> animationSpeed = new List<float>();
        List<int> characterWidth = new List<int>();

        while ((line = reader.ReadLine()) != null)
        {
            line.Trim();
            //line = line.ToLower();
            //if (debug0) { Console.WriteLine(" trimline: " + line); }
            if (line.Length >= 1 && !line[0].Equals('\u003b'))
            {
                string variable = string.Empty;             //'\u003d'://=
                string extra = string.Empty;
                string value = string.Empty;                //'\u003b'://;
                string statement = string.Empty;

                foreach (char c in line)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        statement += c;
                    }
                }

                int stage = 0;
                //stage 0  = START
                //stage 1  = encountered [ pulling contents of brackets
                //stage 2  = encountered ] dump info until reaching = (should always be next character)
                //stage 3  = encountered = pulling value
                //stage -1 = encountered ; FULL STOP
                //the below IF statements should be in reverse stage order (highest stage first)

                int intResult;
                float floatResult;
                Keys keyResult;
                bool boolResult;
                int extraResult;        //for the index of arrays

                foreach (char c in statement)               //'\u005b':[ //'\u005d':]
                {
                    if (stage == 3)
                    {
                        if (c.Equals('\u003b'))
                        {
                            stage = -1;
                        }
                        if (stage > 0)
                        {
                            value += c;
                        }
                    }
                    if (stage == 2)
                    {
                        if (c.Equals('\u003d'))
                        {
                            stage = 3;
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    if (stage == 1)
                    {
                        if (c.Equals('\u005d'))
                        {
                            stage = 2;
                        }
                        else
                        {
                            extra += c;
                        }
                    }
                    if (stage == 0)
                    {
                        if (c.Equals('\u003d'))
                        {
                            stage = 3;
                        }
                        else
                        {
                            if (c.Equals('\u005b'))
                            {
                                stage = 1;
                            }
                            else
                            {
                                variable += c;
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(variable) && !string.IsNullOrWhiteSpace(value))
                {
                    switch (variable.ToLower())
                    {
                        case "attack1damage":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.attack1Damage = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "attack2damage":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.attack2Damage = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "speed":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.speed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "jumptimemax":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.jumpTimeMax = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "idletimemax":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.idleTimeMax = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "maxhealth":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.maxHealth = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "maxshield":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.maxShield = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "maxenergy":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.maxEnergy = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "supershockwaveholdtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.superShockwaveHoldtime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shockwavemaxeffectdistance":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shockwaveMaxEffectDistance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shockwaveeffectivedistance":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shockwaveEffectiveDistance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shockwavestuntime":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shockwaveStunTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "invulnerablemaxtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.invulnerableMaxTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shieldrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shieldRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "energyrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.energyRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "healthrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.healthRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "energyusablemargin":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.energyUsableMargin = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "minsqrdetectdistance":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.minSqrDetectDistance = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "animationspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < animationSpeed.Count)
                                    {
                                        animationSpeed[extraResult] = floatResult;
                                    }
                                    else
                                    {
                                        while (extraResult > animationSpeed.Count)
                                        {
                                            animationSpeed.Add(-1);
                                        }
                                        animationSpeed.Add(floatResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }
                            break;
                        case "shieldanimationspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shieldAnimationSpeed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shieldhealingpercentage":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.shieldHealingPercentage = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "cameralerpspeed":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.cameraLerpSpeed = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "terminalvelocity":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.terminalVelocity = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "masteraudiolevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.masterAudioLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "musiclevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.musicLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "soundeffectslevel":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.soundEffectsLevel = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "potionrechargerate":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.potionRechargeRate = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "potionrechargetime":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.potionRechargeTime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "walljumpholdtime":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.walljumpHoldtime = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "swingmagnitude":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.swingMagnitude = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "swingduration":
                            if (float.TryParse(value, out floatResult))
                            {
                                playerSettings.swingDuration = floatResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;






                        case "collideroffset":                                                         //place new floats above
                            playerSettings.colliderOffset = PointParser(value);
                            if (playerSettings.colliderOffset == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "collidersize":
                            playerSettings.colliderSize = PointParser(value);
                            if (playerSettings.colliderSize == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "attackcolliderwidth":
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.attackColliderWidth = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "attackcolliderheight":
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.attackColliderHeight = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "animationframes":
                            if (int.TryParse(value, out intResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < animationFrames.Count)
                                    {
                                        animationFrames[extraResult] = intResult;
                                    }
                                    else
                                    {
                                        while (extraResult > animationFrames.Count)
                                        {
                                            animationFrames.Add(-1);
                                        }
                                        animationFrames.Add(intResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }
                            break;
                        case "characterheight":
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.characterHeight = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "characterwidth":
                            if (int.TryParse(value, out intResult))
                            {
                                if (int.TryParse(extra, out extraResult))
                                {
                                    if (extraResult < characterWidth.Count)
                                    {
                                        characterWidth[extraResult] = intResult;
                                    }
                                    else
                                    {
                                        while (extraResult > characterWidth.Count)
                                        {
                                            characterWidth.Add(-1);
                                        }
                                        characterWidth.Add(intResult);
                                    }
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine("error: variable index \"" + variable + "[" + extra + "]\" could not be parsed");
                                    errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "[" + extra + "]\" could not be parsed");
                                errorVars = errorVars + "\n  " + variable + "[" + extra + "]\n    value:" + value;
                            }

                            break;
                        case "screenscale":
                            if (float.TryParse(value, out floatResult))
                            {
                                if (floatResult > 0)
                                { playerSettings.screenScale = floatResult; }
                                else
                                { playerSettings.screenScale = 0; }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "maxnumberofpotions":
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.maxNumberOfPotions = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;

                            



                        case "attackkey":                                                               //place new ints above
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.attackKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altattackkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altAttackKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shockwavekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.shockwaveKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altshockwavekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altShockwaveKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "shieldkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.shieldKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altshieldkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altShieldKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "jumpkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.jumpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altjumpkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altJumpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "upkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.upKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altupkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altUpKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "downkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.downKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altdownkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altDownKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "leftkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.leftKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altleftkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altLeftKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "rightkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.rightKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altrightkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altRightKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "rollkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.rollKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altrollkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altRollKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "potionkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.potionKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altpotionkey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altPotionKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "usekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.useKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "altusekey":
                            if (Enum.TryParse(value, out keyResult))
                            {
                                playerSettings.altUseKey = keyResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;





                        case "shockwaveknockback":                                                        //place new key binds above
                            playerSettings.shockwaveKnockback = Vector2Parser(value);
                            if (playerSettings.shockwaveKnockback == new Vector2(-0.112f, -0.112f))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "initialposition":
                            playerSettings.initialPosition = Vector2Parser(value);
                            if (playerSettings.initialPosition == new Vector2(-0.112f, -0.112f))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "boundingbox":
                            playerSettings.boundingBox = RectangleParser(value);
                            if (playerSettings.boundingBox == new Rectangle(-0, -1, -1, -2))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "timeraccuracy":
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.timerAccuracy = "00.";
                                if (intResult > 8)
                                {
                                    for (int i = 8; i > 0; i--)
                                    {
                                        playerSettings.timerAccuracy += "0";
                                    }
                                }
                                else
                                {
                                    for (int i = intResult; i > 0; i--)
                                    {
                                        playerSettings.timerAccuracy += "0";
                                    }
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            
                            checker.Add(true); break;
                        case "resolution":
                            playerSettings.resolution = PointParser(value);
                            if (playerSettings.resolution == new Point(-0112, -0112))
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;





                        case "cameralerp":                                                                //place new "etc variable" (like vectors and rectangles) above
                            if (bool.TryParse(value, out boolResult))
                            {
                                playerSettings.cameraLerpSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "camerashakesetting":
                            if (bool.TryParse(value, out boolResult))
                            {
                                playerSettings.cameraShakeSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "fullscreen":
                            if (bool.TryParse(value, out boolResult))
                            {
                                playerSettings.fullscreen = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "vsync":
                            if (bool.TryParse(value, out boolResult))
                            {
                                playerSettings.vSync = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        case "cameraswingsetting":
                            if (bool.TryParse(value, out boolResult))
                            {
                                playerSettings.cameraSwingSetting = boolResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;






                        case "debug":                                                                     //place new bools above
                            if (int.TryParse(value, out intResult))
                            {
                                playerSettings.debug = intResult;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            checker.Add(true); break;
                        default:
                            if (string.IsNullOrWhiteSpace(extra))
                            {
                                Irbis.Irbis.WriteLine("error: no variable with name: \"" + variable + "\"");
                                errorVars = errorVars + "\n  name:" + variable + "\n    value:" + value;
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("error: no variable with name: \"" + variable + "[" + extra + "]\"");
                                errorVars = errorVars + "\n    " + variable + "[" + extra + "]";
                            }
                            break;
                    }
                }
            }
        }

        if (animationFrames.Count > 0)
        {
            checker.Add(true);
        }
        if (animationSpeed.Count > 0)
        {
            checker.Add(true);
        }
        if (characterWidth.Count > 0)
        {
            checker.Add(true);
        }

        playerSettings.animationFrames = animationFrames.ToArray();
        playerSettings.animationSpeed = animationSpeed.ToArray();
        playerSettings.characterWidth = characterWidth.ToArray();

        bool variablesHaveEncounteredErrors = !string.IsNullOrWhiteSpace(errorVars);
        int variablesThatHaveBeenAssigned = checker.Count;

        //checker.count should always be == to number of variable, currently 66 variables
        if (variablesHaveEncounteredErrors || variablesThatHaveBeenAssigned < numberOfVariables)
        {
            if (!string.IsNullOrWhiteSpace(errorVars))
            {
                Irbis.Irbis.WriteLine("could not load:" + errorVars + "\nthese variables.");
            }

            if (variablesThatHaveBeenAssigned < numberOfVariables)
            {
                Irbis.Irbis.WriteLine("Loaded " + variablesThatHaveBeenAssigned + " variables successfully.");
                Irbis.Irbis.WriteLine("Not all variables were assigned values. Is something misspelled?");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("load successful.");
        }
        Irbis.Irbis.WriteLine();
        reader.Close();

        return playerSettings;
    }

    private static Vector2 Vector2Parser(string value)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.Vector2Parser"); }
        string Xval = string.Empty;
        string Yval = string.Empty;
        //value = {X:000Y:000}
        value = value.Substring(1);
        //value = X:000Y:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }

        if (float.TryParse(Xval, out float Xresult))
        {
            if (float.TryParse(Yval, out float Yresult))
            {
                return new Vector2(Xresult, Yresult);
            }
            else
            {
                Irbis.Irbis.WriteLine("error: Vector2 could not be parsed");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("error: Vector2 could not be parsed");
        }
        return new Vector2(-0.112f, -0.112f);
    }

    private static Point PointParser(string value)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.PointParser"); }
        string Xval = string.Empty;
        string Yval = string.Empty;
        //value = {X:000Y:000}
        if (!(value[0].Equals('\u0058') || value[0].Equals('\u0078') || value[0].Equals('\u0059') || value[0].Equals('\u0079')))
        {
            value = value.Substring(1);
        }
        //value = X:000Y:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://- //This is the same as Vector2Variable(string), with the caviet that ints will never have a decimal
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d') || value[j].Equals('\u002e'))) //'\u002d'://- '\u002e'://.
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }
        if (int.TryParse(Xval, out int Xresult))
        {
            if (int.TryParse(Yval, out int Yresult))
            {
                return new Point(Xresult, Yresult);
            }
            else
            {
                Irbis.Irbis.WriteLine("error: Point could not be parsed");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("error: Point could not be parsed");
        }
        return new Point(-0112, -0112);
    }

    private static Rectangle RectangleParser(string value)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("PlayerSettings.RectangleParser"); }
        string Xval = string.Empty;
        string Yval = string.Empty;
        string widthVal = string.Empty;
        string heightVal = string.Empty;
        //value = {X:000Y:000Width:000Height:000}
        value = value.Substring(1);
        //value = X:000Y:000Width:000Height:000}
        while (value.Length > 0 && !value[0].Equals('\u007d'))   //'\u007d'://}
        {
            if (value[0].Equals('\u0058') || value[0].Equals('\u0078'))  //'\u0058'://X '\u0078'://x
            {
                // X + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://- This is the same as Vector2Variable(string), with the caviet that ints will never have a decimal
                {
                    Xval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0059') || value[0].Equals('\u0079')) //'\u0059'://Y '\u0079'://y
            {
                // Y + : = 2 characters before data starts
                int j = 2;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    Yval += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0057') || value[0].Equals('\u0077')) //'\u0057'://W '\u0077'://w
            {
                // Width + : = 6 characters before data starts
                int j = 6;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    widthVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
            if (value[0].Equals('\u0048') || value[0].Equals('\u0068')) //'\u0048'://H '\u0068'://h
            {
                // Height + : = 7 characters before data starts
                int j = 7;
                while (value.Length > j && (char.IsDigit(value[j]) || value[j].Equals('\u002d'))) //'\u002d'://-
                {
                    heightVal += value[j];
                    j++;
                }
                value = value.Substring(j);
            }
        }
        //boundingBox.X = int.Parse(buttonList[menuSelection].buttonStatement) - (boundingBox.Width / 2);
        if (int.TryParse(Xval, out int Xresult))
        {
            if (int.TryParse(Yval, out int Yresult))
            {
                if (int.TryParse(widthVal, out int widthResult))
                {
                    if (int.TryParse(heightVal, out int HeightResult))
                    {
                        return new Rectangle(Xresult - (widthResult / 2), Yresult - (HeightResult / 2), widthResult, HeightResult);
                    }
                    else
                    {
                        Irbis.Irbis.WriteLine("error: Rectangle could not be parsed");
                    }
                }
                else
                {
                    Irbis.Irbis.WriteLine("error: Rectangle could not be parsed");
                }
            }
            else
            {
                Irbis.Irbis.WriteLine("error: Rectangle could not be parsed");
            }
        }
        else
        {
            Irbis.Irbis.WriteLine("error: Rectangle could not be parsed");
        }
        return new Rectangle(-0, -1, -1, -2);
    }
}
