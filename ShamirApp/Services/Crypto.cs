using ShamirApp.Models.Account;

namespace ShamirApp.Services
{
    public static class Crypto
    {
        public static double Interpolate(List<Point> points, int x)
        {
            var x1 = points[0].X;
            var x2 = points[1].X;
            var x3 = points[2].X;

            var y1 = points[0].Y;
            var y2 = points[1].Y;
            var y3 = points[2].Y;

            double y =
                (y1 * (double)((x - x2) * (x - x3))) / ((x1 - x2) * (x1 - x3)) +
                (y2 * (double)((x - x1) * (x - x3))) / ((x2 - x1) * (x2 - x3)) +
                (y3 * (double)((x - x1) * (x - x2))) / ((x3 - x1) * (x3 - x2));

            return y;
        }

        public static double GetSecret(List<Point> keys)
        {
            double result = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                double p = 1;
                for (int j = 0; j < keys.Count; j++)
                {
                    if (i == j) continue; // пропускаем
                    p *= (double)(0 - keys[j].X) / (double)(keys[i].X - keys[j].X); // вычисляем дробь и домножаем результат
                }
                result += p * keys[i].Y; // дополнительно умножим на Y и сложим
            }
            return result;
        }
    }
}
