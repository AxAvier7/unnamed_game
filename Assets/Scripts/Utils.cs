using System;
public static class Utils
    {
        public static void Print<T>(T info)
        {
            Console.WriteLine(info);
        }

        public static void PrintArray<T>(T[,] array)
        {
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }
        }

        public static void PrintMatrix<T>(T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static int RNG(int min, int max)
        {
            Random random = new Random();
            int result = min + (random.Next() * (max - min));
            return result;
        }

        public static int[,] AddMatrices(int[,] matrix1, int[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int cols = matrix1.GetLength(1);

            if (rows != matrix2.GetLength(0) || cols != matrix2.GetLength(1))
                throw new ArgumentException("Las matrices deben tener las mismas dimensiones.");

            int[,] result = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }

        public static bool EqualMatrix<T>(T[,] matrix1, T[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int cols = matrix1.GetLength(1);

            if (rows != matrix2.GetLength(0) || cols != matrix2.GetLength(1))
                return false;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if(matrix1[i, j].ToString() == matrix2[i,j].ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static (int, int)? FindInMatrix<T>(T[,] matrix, T value)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j].Equals(value))
                        return (i, j);
                }
            }
            return null;
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static bool IsSuperPrime(int number)
        {
            if(!IsPrime(number)) return false;
            if(number%10 == number) return true;
            return IsSuperPrime(number/10);
        }

        public static void OpenWebSite(string path)
        {
            System.Diagnostics.Process.Start("MicrosoftEdge.exe", path);
        }

        public static string BinaryConversion(int num)
        {
            string binario = "";
            if(num == 0) Console.WriteLine(num);
            while(num > 0)
            {
                if(num %2 == 0) binario = "0" + binario;
                else binario = "1" + binario;
                num = num/2;
            }
            Console.WriteLine(binario);
            return binario;
        }

        public static void Fibonacci(int maxfibo)
        {
            int numero = 1;
            int temp = 0;
            int resultado;

            while(numero < maxfibo)
            {
                Console.WriteLine(numero);
                resultado = numero + temp;
                temp = numero;
                numero = resultado;
            } 
        }

        public static double DistanceBetweenPoints(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static int BinarySearch(int[] a, int x)
        {
            int inicio = 0;
            int fin = a.Length - 1;

            while(inicio <= fin)
            {
                int medio = (inicio + fin)/2;
                if(x < medio - 1)
                {
                    fin = medio - 1;
                }
                else if (x > a[medio])
                {
                    inicio = medio + 1;
                }
                else return medio;
            }
            return -1;
        }

        public static int[] InvertArray(int[] array)
        {
            int[] res = new int[array.Length];
            for(int i = 0; i < res.Length; i++)
                res[i] = array[array.Length - 1 - i];
            return res;
        }

        public static T[] InsertInArray<T>(T[] original, T x, int pos)
        {
            T[] copy = new T[original.Length + 1];

            for (int i = 0; i < pos; i++)
                copy[i] = original[1];

            copy[pos] = x;

            for (int i = pos; i < original.Length; i++)
                copy[i + 1] = original[i];

            return copy;
        }

        public static T[] RemoveFromArray<T>(T[] original, T x, int pos)
        {
            T[] copy = new T[original.Length - 1];

            for (int i = 0; i < pos; i++)
                copy[i] = original[i];

            for(int i = pos; i < copy.Length; i++)
                copy[i] = original[i + 1];

            return copy;
        }

        public static int RollDice()
        {    
            Random gennumero = new Random();
            int rolldice = gennumero.Next(1, 7);
            return rolldice;
        }

        public static int Sumatory(int n)
        {
            if(n==0) return 0;
            else return Sumatory(n-1) +n;
        }

        public static int Factorial(int n)
        {
            if(n==0 || n==1) return 1;
            else return Factorial(n-1)*n;
        }

        public static int MCD(int a, int b) => b==0 ? Math.Abs(a) : MCD(b, a%b);

        public static void Collatz(int n)
        {
            Console.WriteLine(n);
            if(n!=1 || n!=-1)
            {
                if(n%2==0)
                    Collatz(n/2);
                else Collatz(3*n+1);
            }
        }
    }