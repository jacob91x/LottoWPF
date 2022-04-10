using System.Collections.Generic;

namespace LottoWPF
{
    public class Coupon
    {
        public int Count
        {
            get
            {
                return Numbers.Count;
            }
        }

        public List<int> Numbers { get; set; } = new List<int>();

        public override string ToString()
        {
            return string.Join(", ", Numbers);
        }

        public bool Contains(int number)
        {
            return Numbers.Contains(number);
        }

        public CouponError Add(int number)
        {
            if (Numbers.Contains(number))
            {
                return CouponError.NumberAlreadyExist;
            }
            else if (number < 1 || number >= 20)
            {
                return CouponError.NumberOutOfRange;
            }
            Numbers.Add(number);
            return CouponError.NoError;
        }

        public void RemoveLast()
        {
            if (Numbers.Count >= 1)
            {
                Numbers.RemoveAt(Numbers.Count - 1);
            }
        }
    }
}