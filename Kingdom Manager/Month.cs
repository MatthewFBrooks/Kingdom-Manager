using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdom_Manager
{
    class Month : IComparable
    {
        public string Name { get; set; }
        public string UUID { get; set; }
        public string YearUUID { get; set; }
        private int num;
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                if (value >=1 && value <=12)
                {
                    num = value;
                }
            }
        }
        private int foodIncome;
        public int FoodIncome
        {
            get
            {
                return foodIncome;
            }
            set
            {
                foodIncome = value;
            }
        }

        private int goldIncome;
        public int GoldIncome
        {
            get
            {
                return goldIncome;
            }
            set
            {
                goldIncome = value;
            }

        }

        public int this[int resource]
        {
            get
            {
                if (resource == 0)
                {
                    return FoodIncome;
                }
                else if (resource == 1)
                {
                    return GoldIncome;
                }
                else
                {
                    throw new System.IndexOutOfRangeException("Tried to index resource: " + resource);
                }
            }
            set
            {
                if (resource == 0)
                {
                    FoodIncome = value;
                }
                else if (resource == 1)
                {
                    GoldIncome = value;
                }
                else
                {
                    throw new System.IndexOutOfRangeException("Tried to index resource: " + resource);
                }
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Month)
            {
                return Num.CompareTo(((Month)obj).Num);
            }
            else
            {
                throw new InvalidCastException("Not a valid Year object.");
            }
        }
    }
}
