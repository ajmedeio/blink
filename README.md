# Blink
## Alexander Medeiros

Blink is an action rpg (ARPG). The most important technical implementation is that of Unity Networking. On top of that is a beautiful map with sounds of wind blowing through the structures and water near the center of the map. We also have a full set of animations for our character generally called Garf by my friends and I. The player can use 'wasd-spacebar' to move. The mouse also offers increased control over the character. The dragging with the left-mouse button down changes the camera angle, while dragging with the right mouse-button down changes the character's angle. Holding both mouse buttons will make the character move forward. In order to use abilities we can press the numbers 1 & 2 to for blink and firebolt, respectively. Firebolt requires a living enemy target to be casted upon, whereas blink can be used without a target. Blink teleports the player forward 2.5 units. Firebolt hurls a devastating ball of fire towards the enemy dealing 50 damage. In order to target an enemy, the player needs to simply click on the enemy model.

### External Resources
Can be found under the Assets folder. Any folder not contained in BlinkAssets is from the Unity Asset Store.
- Courtyard map models (had to gut most features because poor performance)
- UMA: avatar creation framework
- PyroParticles: fireball prefab, still had to code homing ability and everything to do damage
- TextMeshPro: A Unity text solution.
