using System;

namespace ritaripeli
{
    /// <summary>
    /// Tästä luokasta peritään kaikki erilaiset 
    /// tavarat joita voi säilyttää repussa
    /// </summary>
    internal abstract class Tavara
    {
        public string Nimi { get; protected set; } = "Tavara";

        public override string ToString()
        {
            return Nimi;
        }
    }
}