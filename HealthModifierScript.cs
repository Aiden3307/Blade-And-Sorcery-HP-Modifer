using System;
using ThunderRoad;
using UnityEngine;

namespace NPCHealthModifier
{
    public class HealthModifierScript : ThunderScript
    {
        // Define mod option slider for NPC HP (range 1 to 1000, default is 100 which is index 99)
        [ModOption("NPC HP Override", "Overrides max HP for newly spawned NPCs (1 - 1000)", defaultValueIndex = 99)]
        [ModOptionOrder(0)]
        [ModOptionSave]
        [ModOptionIntValues(1, 1000, 1)]
        [ModOptionSlider]
        public static int npcHpOverride = 100;

        public override void ScriptLoaded(ModManager.ModData modData)
        {
            base.ScriptLoaded(modData);
            
            // Hook into NPC spawning event
            EventManager.onCreatureSpawn += EventManager_onCreatureSpawn;
            Debug.Log("[NPCHealthModifier] Loaded and hooked event.");
        }

        public override void ScriptUnload()
        {
            base.ScriptUnload();
            
            // Unsubscribe to avoid memory leaks
            EventManager.onCreatureSpawn -= EventManager_onCreatureSpawn;
            Debug.Log("[NPCHealthModifier] Unloaded and unhooked event.");
        }

        private void EventManager_onCreatureSpawn(Creature creature)
        {
            // Only apply override if creature is valid and is NOT the player
            if (creature == null || creature.isPlayer)
                return;

            try
            {
                // Assign new max health
                creature.maxHealth = npcHpOverride;
                
                // Assign new current health
                creature.currentHealth = npcHpOverride;
                
                Debug.Log($"[NPCHealthModifier] Overrode {creature.name} HP to {npcHpOverride}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NPCHealthModifier] Failed to set HP: {ex.Message}");
            }
        }
    }
}
