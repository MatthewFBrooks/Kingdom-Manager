using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingdom_Manager
{
    class Year : IComparable
    {
        public List<Month> Months;
        private int num;
        public string UUID { get; set; }
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                if (value >= 0)
                {
                    num = value;
                }
            }
        }

        public Year()
        {
            UUID = Guid.NewGuid().ToString();
            Months = new List<Month>();
            for (int month = 0; month < Program.MONTHS; month++)
            {
                Months.Add(new Month() { UUID = Guid.NewGuid().ToString(), YearUUID = UUID, Num = month });
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Year)
            {
                return Num.CompareTo(((Year)obj).Num);
            }
            else
            {
                throw new InvalidCastException("Not a valid Year object.");
            }
        }
    }
}
