using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundTwoMono
{
    // fighter character state enumerator
    enum FighterState
    {
        neutral,
        walk,
        jumping,
        invincible,
        hitstun,
        airHitstun,
        blockstun,
        projectileInvincible,
        attackStartup,
        attackRecovery,
        jumpingAttack

    };

    enum CancelState
    {
        light,
        medium,
        heavy,
        special,
        none
    };
    enum FighterAnimations
    {
        neutral,
        walkToward,
        walkBack,
        light,
        medium,
        heavy,
        sp1,
        sp2,
        sp3,
        jumpLight,
        jumpMedium,
        jumpHeavy,
        jumpRising,
        jumpDecending,
        jumpLanding,
        hit,
        airHit,
        knockdown,
        blocking,
        throwTry,
        throwComplete,
        Super,
        intro,
        win,
        deathKnockdown,
        chunliThrow


    }
}
