using System;

namespace LottoWPF.Core
{
    public class LottoEngine
    {
        public int CalculateCumulation()
        {
            var cumulationRandom = new Random();
            int cumulation = cumulationRandom.Next(2, 37) * 1000000;
            return cumulation;
        }
    }

}
