using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Logite.Database.Core
{
    /// <summary>
    /// Represents an attack vector such as an attack request or an attack in the referer
    /// </summary>
    public class AttackVector
    {
        public const string RequestAttackName = "Request Attack";
        public const string RefererAttackName = "Referer Attack";
        public const string AgentAttackName = "Agent Attack";

        /// <summary>
        /// Gets the attack vector type
        /// </summary>
        public AttackVectorType AttackType
        {
            get;
        }

        /// <summary>
        /// Gets the attack vector type as a long.
        /// </summary>
        public long AttackTypeLong => (long)AttackType;

        /// <summary>
        /// Gets the string of the attack
        /// </summary>
        public string Attack
        {
            get;
        }

        /// <summary>
        /// Gets <see cref="Attack"/> with single quotes escaped as double single quotes.
        /// </summary>
        public string EscapedAttack => Attack.Replace("'", "''");

        /// <summary>
        /// Initializes a new instance of the <see cref="AttackVector"/> class.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        /// <param name="attack">The attack string.</param>
        internal AttackVector(AttackVectorType attackType, string attack)
        {
            AttackType = attackType;
            Attack = attack;
        }

        public override string ToString()
        {
            return $"{AttackType} {Attack}";
        }
    }
}
