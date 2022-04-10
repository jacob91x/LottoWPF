using System;
using System.Linq;

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

        public int[] DrawLuckyNumbers()
        {
            int[] drawnNumbers = new int[6];
            var drawRandomNumber = new Random();

            for (int i = 0; i < drawnNumbers.Length; i++)
            {
                int draw = drawRandomNumber.Next(1, 21);

                if (!drawnNumbers.Contains(draw))
                {
                    drawnNumbers[i] = draw;
                }
                else
                {
                    i--;
                }
            }
            return drawnNumbers;
        }
    }
}
