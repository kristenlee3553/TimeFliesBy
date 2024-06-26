using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Static class that holds key variables for the game.
/// <br></br>
/// Script will not disappear between scenes
/// </summary>
public static class GameManager
{

    // -------------- Level Variables -----------------

    /// <summary>
    /// Current Level Phase
    /// </summary>
    public static int s_curPhase = 1;

    /// <summary>
    /// Name of the level, not including phase number
    /// </summary>
    public static string s_level = "Dino";

    /// <summary>
    /// Name of the scene that is loaded. Often GameManager.Level + GameManager.Phase
    /// </summary>
    public static string s_curScene = "Dino1";

    /// <summary>
    /// Name of the last scene that is loaded. Needed for time movement logic
    /// </summary>
    public static string s_lastScene = "";

    /// <summary>
    /// Probably will delete later. Tells if game screen is on a level not on a UI screen.
    /// </summary>
    public static bool s_onGameLevel = true;

    /// <summary>
    /// Phase when checkpoint is reached
    /// </summary>
    public static int s_checkpointPhase = 1;

    /// <summary>
    /// First phase when level loads. Usually 1, some levels may start in the middle.
    /// </summary>
    public static int s_firstPhase = 1;

    // -------------- Starting Positions ---------------------

    /// <summary>
    /// Wizard's starting x position
    /// </summary>
    public static float s_wizardResetX = -7.7f;

    // dino -> -7.53f;
    // tutorial -> -7.56f

    /// <summary>
    /// Wizard's starting y position
    /// </summary>
    public static float s_wizardResetY = -3.1f;

    // dino -> 1.07f;
    // tutorial -> -3.19f

    /// <summary>
    /// Fairy's starting x position
    /// </summary>
    public static float s_fairyResetX = -6.08f;

    /// <summary>
    /// Fairy's starting y position
    /// </summary>
    public static float s_fairyResetY = -2.63f;

    // -------------- Respawn Point ---------------------

    /// <summary>
    /// Wizard's Respawn x position. For checkpoint
    /// </summary>
    public static float s_wizardRespawnX = -7.53f;

    /// <summary>
    /// Wizard's Respawn y position. For checkpoints
    /// </summary>
    public static float s_wizardRespawnY = 1.07f;

    // --------------- Game Over Variables -------------------
    public static bool s_onDeathObject = false;
    public static bool s_sceneChange = false;

    /// <summary>
    /// Stores gameobject that wizard is colliding with, if time WERE TO BE moved backwards or forwards. 
    /// Stores based on phase. null if not colliding with any.
    /// Access with s_reposition[phase num]
    /// </summary>
    public static IReposition[] s_reposition = {null, null, null, null, null, null };

    /// <summary>
    /// Keeps track of how many colliders the wizard is touching. 
    /// </summary>
    public static int wizardCollisions = 0;

    // --------------- Key Bindings -----------------------

    /// <summary>
    /// Dictionary that maps an enum KeyBind to the corresponding Keycode
    /// <br></br>
    /// Ex: GameManager.KeyBind.WizardJump maps to KeyCode.Space aka space bar = wizard jump for the default value
    /// </summary>
    public static Dictionary<KeyBind, KeyCode> s_keyBinds;

    // Actual key binds are in LoadGame script

    // --------------- Enums -----------------------------
    
    /// <summary>
    /// Enums of keyboard actions. Use when wanting to change keybinding. 
    /// Use when getting/updating values in s_keybinds.
    /// </summary>
    public enum KeyBind { 
        WizardJump, WizardLeft, WizardRight, 
        FairyUp, FairyLeft, FairyDown, FairyRight,
        MoveTimeBack, MoveTimeFor, Preserve, Interact
    };


    // ---------------- Boundaries -------------------------
    public static float s_boundaryLeft = -8.52f;
    public static float s_boundaryRight = 8.52f;
    public static float s_boundaryUp = 4.25f;
    public static float s_boundaryDown = -4.02f;


    // ---------------- Saved Data for Orbs --------------------------------
    public static bool[] s_dinoOrbs = { false, false, false };
    public static bool[] s_medOrbs = { false, false, false };

    // ---------------- Current level orb data -------------------------
    /// <summary>
    /// Current data about orbs for current level
    /// <br></br>
    /// True if player collected orbs
    /// </summary>
    public static bool[] s_curOrbs = { false, false, false };

    // ------------------ Wizard Interactables ------------------------
    public static Color s_interactColor = Color.cyan;
    
}
