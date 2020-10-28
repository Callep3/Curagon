using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    /* -- Rules --
     * - Happiness -
     * If too low (30%?), work worse
     * If poop on floor happiness goes down fast
     * If hunger too low, happiness goes down faster
     * If work happiness goes down faster
     * 
     * - Work -
     * When working, stamina & hunger & happiness drains
     * If stamina runs out stop working
     * Village Exp goes up, depending on curagons level
     * 
     * - Stamina - 
     * Goes down constantly by time
     * If working it goes down fastest
     * If playing it goes down fast
     * You need stamina to work
     * You need stamina to play
     * Stamina gained when sleeping
     * Stamina sleightly gained when eating
     * When stamina max wake up
     * 
     * - Hunger -
     * You always get hungrier
     * Goes down faster when you play
     * Goes down fastest when you work
     * You gain hunger when you eat
     * When you eat enough you poop
     * If starve(Hunger 0%), game over
     * 
     * - Village -
     * When work, village gains experience
     * When work, village recovers (is cured) and gains health
     * If you work enough, level up
     * Village dies if health == 0
     * If village dies, game over
     * Village health scales with level, level 1 = 1 day of survival for the village
     * 
     */
}
