using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numirologia
{
    class Class1
    {
        private static int[] NumCountArray = new int[10];
        private static int[] FateArray = new int[7];
        private static int[] VolitionArray = new int[7];
        private static int[] CountYear = new int[7] { 0, 12, 24, 36, 48, 60, 72 };
        private static DateTime[] DateRootsVect = new DateTime[7];
        private static int CursorDate;
        private static double CursorFate;
        private static double CursorVolition;
        private static int NumItog;


        private static void NumDecomposition(int N, int variant)
        {
            int[] BuffArray = new int[10];
            int Ost;
            int i = 6;
            N = (N > 0) ? N : -N;
            while (N != 0)
            {
                Ost = N % 10;
                if (variant == 1)//матрица
                {
                    NumCountArray[Ost]++;
                }
                else if (variant == 2)//Судьба
                {
                    FateArray[i--] = Ost;
                }
                else if (variant == 3)//Воля
                {
                    VolitionArray[i--] = Ost;
                }
                N = (int)N / 10;
                //Console.WriteLine(Ost.ToString());
            }
        }

        private static int SumNumCountArray()
        {
            int Sum = 0;
            int i = 0;
            foreach (var item in NumCountArray)
            {
                Sum += (i++) * item;
            }
            return Sum;
        }

        private static void CalcMatrix(int day, int Month, int Year)
        {
            NumDecomposition(day, 1);
            NumDecomposition(Month, 1);
            NumDecomposition(Year, 1);
            int OldSum = SumNumCountArray();
            NumDecomposition(OldSum, 1);
            int MedlSum;
            int Def;
            if (OldSum >= 10)
            {
                MedlSum = SumNumCountArray();
                Def = MedlSum - OldSum;
                NumDecomposition(Def, 1);
            }
            int CorrConst = (Year < 2000) ? -2 : 19;
            NumDecomposition(CorrConst, 1);
            int DefSum = OldSum + CorrConst;
            OldSum = SumNumCountArray();
            NumDecomposition(DefSum, 1);
            MedlSum = SumNumCountArray();
            Def = MedlSum - OldSum;
            NumDecomposition(Def, 1);
        }

        private static void CalcGraf(int day, int Month, int Year)
        {
            int ProductGraf = (day * 100 + Month) * Year;
            NumDecomposition(ProductGraf, 2);
            int DayCorrect = ((day < 10) ? 10 : 0) + ((day % 10 == 0) ? 10 : 0);
            int MonthCorrect = ((Month < 10) ? 10 : 0) + ((Month % 10 == 0) ? 10 : 0);
            int YearCorrect = (((Year - Year / 1000 * 1000) < 100) ? 100 : 0) +
                              (((Year - Year / 100 * 100) < 10) ? 10 : 0) +
                              (((Year % 10) == 0) ? 1 : 0);
            ProductGraf = ((day + DayCorrect) * 100 + Month + MonthCorrect) * (Year + YearCorrect);
            NumDecomposition(ProductGraf, 3);
        }

        private static double CalcСrossing(int y1, int y2, int y3, int y4)
        {
            int x1 = 0;
            int x2 = 12;
            double k1 = (double)(y2 - y1) / (x2 - x1);
            double k2 = (double)(y4 - y3) / (x2 - x1);
            double c1 = y1 - k1 * x1;
            double c2 = y3 - k2 * x1;
            double x = -1;
            double result = -1;
            if (Math.Abs(k1 - k2) > 0.00000005)
            {
                x = (c2 - c1) / (k1 - k2);
                if (x > x1 && result <= x2)
                {
                    result = (x - x1) / (x2 - x1);
                }
            }
            return result;
        }

        private static void CalcСrossingDate(DateTime date)
        {
            var OldDate = date;
            var NewDate = new DateTime();
            if (FateArray[0] == VolitionArray[0] && FateArray[1] != VolitionArray[1])
            {
                DateRootsVect[1] = OldDate;
            }
            for (int i = 1; i < 7; i++)
            {
                NewDate = date.AddYears(CountYear[i]);
                var diff = NewDate - OldDate;

                if (FateArray[i - 1] != VolitionArray[i - 1])
                {
                    double koef = CalcСrossing(FateArray[i - 1], FateArray[i], VolitionArray[i - 1], VolitionArray[i]);
                    if (koef > 0)
                    {
                        int days = (int)((double)diff.Days * koef);
                        DateRootsVect[i] = OldDate.AddDays(days);
                    }
                }
                OldDate = NewDate;
            }
        }

        private static void CalcCursorDate(DateTime date)
        {
            var now = DateTime.Now;
            var diff = now - date;
            var AllDif = date.AddYears(72) - date;
            CursorDate = (int)((double)(72 * diff.Days) / AllDif.Days);
        }

        private static void CalcNumItog(int day, int Month, int Year)
        {
            int Sum = 0;
            int CurSum = 1;

            NumDecomposition(day, 1);
            NumDecomposition(Month, 1);
            NumDecomposition(Year, 1);

            while (Sum != CurSum)
            {
                Sum = CurSum;
                CurSum = SumNumCountArray();
                Array.Clear(NumCountArray, 0, NumCountArray.Length);
                NumDecomposition(CurSum, 1);
            }
            Array.Clear(NumCountArray, 0, NumCountArray.Length);
            NumItog = Sum;
        }

        public int CalcMenNum()
        {
            int sum = 0;
            for (int i = 2; i < 10; i += 2)
            {
                sum += NumCountArray[i];
            }
            return sum;
        }

        public  int CalcWomenNum()
        {
            int sum = 0;
            for (int i = 1; i < 10; i += 2)
            {
                sum += NumCountArray[i];
            }
            return sum;
        }

        public int CalcYearsOld(DateTime date)
        {
            var now = DateTime.Now;
            
            return now.Year-date.Year;
        }

        private static void CalcCurentNum()
        {
            for (int i = 1; i < 7; i++)
            {
                if (CursorDate < CountYear[i])
                {
                    int x1 = CountYear[i - 1];
                    int x2 = CountYear[i];
                    int y1 = FateArray[i - 1];
                    int y2 = FateArray[i];
                    int y3 = VolitionArray[i - 1];
                    int y4 = VolitionArray[i];
                    double k1 = (double)(y2 - y1) / (x2 - x1);
                    double k2 = (double)(y4 - y3) / (x2 - x1);
                    double c1 = y1 - k1 * x1;
                    double c2 = y3 - k2 * x1;
                    CursorFate = CursorDate * k1 + c1;
                    CursorVolition = CursorDate * k2 + c2;
                    break;
                }
            }
        }



        public Class1(DateTime date)
        {
            //var date = new DateTime(1989, 01, 01);
            CalcNumItog(date.Day, date.Month, date.Year);
            CalcMatrix(date.Day, date.Month, date.Year);
            CalcGraf(date.Day, date.Month, date.Year);
            CalcCursorDate(date);
            CalcСrossingDate(date);
            CalcCurentNum();
        }

        //int[] NumCountArray = new
        //int[] FateArray = new int
        //int[] VolitionArray = new
        //int[] CountYear = new int
        //DateTime[] DateRootsVect
        //int CursorDate;
        //Double CursorFate;
        //Double CursorVolition;
        //int NumItog;
        public int GetNumItog()
        {
            return NumItog;
        }
        public double GetCursorVolition()
        {
            return CursorVolition;
        }
        public double GetCursorFate()
        {
            return CursorFate;
        }
        public int[] GetNumCountArray()
        {
            return NumCountArray;
        }
        public int[] GetFateArray()
        {
            return FateArray;
        }
        public int[] GetVolitionArray()
        {
            return VolitionArray;
        }
        public DateTime[] GetDateRootsVect()
        {
            return DateRootsVect;
        }
    }
}
