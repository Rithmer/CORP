namespace PracticalWork8
{
    class Program
    {
        static void Main()
        {
            Matrix? A = null, B = null;
            var service = new MatrixService();

            while (true)
            {
                Console.WriteLine("\n=== Калькулятор матриц ===");
                Console.WriteLine("1. Создать матрицы A и B");
                Console.WriteLine("2. Заполнить вручную");
                Console.WriteLine("3. Заполнить случайно");
                Console.WriteLine("4. Показать матрицы");
                Console.WriteLine("5. A + B");
                Console.WriteLine("6. A * B");
                Console.WriteLine("7. Транспонирование");
                Console.WriteLine("8. Найти детерминант");
                Console.WriteLine("9. Найти обратную матрицу");
                Console.WriteLine("10. Решить Ax = b");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                string? key = Console.ReadLine();

                if (key == "0") break;

                try
                {
                    switch (key)
                    {
                        case "1": (A, B) = service.CreateMatrices(); break;
                        case "2": service.FillMatrices(ref A, ref B); break;
                        case "3": (A, B) = service.FillRandom(A, B); break;
                        case "4": service.PrintMatrices(A, B); break;
                        case "5": service.AddMatrices(A, B); break;
                        case "6": service.MultiplyMatrices(A, B); break;
                        case "7": service.TransposeMatrices(A, B); break;
                        case "8": service.ShowDeterminant(A, B); break;
                        case "9": service.ShowInverse(A, B); break;
                        case "10": service.SolveSystem(A); break;
                        default: Console.WriteLine("Неверный пункт."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("До свидания!");
        }
    }

    // ======================================================
    // СЕРВИСНЫЙ КЛАСС — ЛОГИКА ВНЕ МЕНЮ
    // ======================================================
    class MatrixService
    {
        public (Matrix, Matrix) CreateMatrices()
        {
            Matrix A = ReadMatrixSize("A");
            Matrix B = ReadMatrixSize("B");
            Console.WriteLine("Матрицы созданы.");
            return (A, B);
        }

        public void FillMatrices(ref Matrix A, ref Matrix B)
        {
            if (!Check(A, B)) return;
            Console.WriteLine("Ввод матрицы A:");
            FillManual(ref A);
            Console.WriteLine("Ввод матрицы B:");
            FillManual(ref B);
        }

        public (Matrix, Matrix) FillRandom(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return (A, B);

            Console.Write("Введите диапазон a b: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException("Диапазон не введён.");

            var parts = input.Split();
            if (parts.Length != 2 || !int.TryParse(parts[0], out int a) || !int.TryParse(parts[1], out int b))
                throw new FormatException("Неверный формат диапазона.");

            A = Matrix.Random(A.Rows, A.Cols, a, b);
            B = Matrix.Random(B.Rows, B.Cols, a, b);
            Console.WriteLine("Матрицы заполнены случайно.");
            return (A, B);
        }

        public void PrintMatrices(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            Console.WriteLine("\nМатрица A:");
            A.Print();
            Console.WriteLine("\nМатрица B:");
            B.Print();
        }

        public void AddMatrices(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            if (A.Rows != B.Rows || A.Cols != B.Cols)
            {
                Console.WriteLine("Невозможно сложить: размеры не совпадают.");
                return;
            }
            Console.WriteLine("Результат A + B:");
            A.Add(B).Print();
        }

        public void MultiplyMatrices(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            if (A.Cols != B.Rows)
            {
                Console.WriteLine("Несовместимые размеры для умножения.");
                return;
            }
            Console.WriteLine("Результат A * B:");
            A.Multiply(B).Print();
        }

        public void TransposeMatrices(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            Console.WriteLine("A^T:");
            A.Transpose().Print();
            Console.WriteLine("B^T:");
            B.Transpose().Print();
        }

        public void ShowDeterminant(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            Console.Write("Выберите матрицу (A/B): ");
            string? s = Console.ReadLine()?.Trim().ToUpper();
            Matrix? chosen = s == "A" ? A : s == "B" ? B : null;
            if (chosen == null) throw new ArgumentException("Неверный выбор матрицы.");

            if (chosen.Rows != chosen.Cols)
            {
                Console.WriteLine("Невозможно вычислить детерминант: матрица не квадратная.");
                return;
            }

            double det = chosen.Determinant();
            Console.WriteLine($"Детерминант выбранной матрицы: {det}");
        }

        public void ShowInverse(Matrix A, Matrix B)
        {
            if (!Check(A, B)) return;
            Console.Write("Выберите матрицу (A/B): ");
            string? s = Console.ReadLine()?.Trim().ToUpper();
            Matrix? chosen = s == "A" ? A : s == "B" ? B : null;
            if (chosen == null) throw new ArgumentException("Неверный выбор матрицы.");

            if (chosen.Rows != chosen.Cols)
            {
                Console.WriteLine("Невозможно найти обратную: матрица не квадратная.");
                return;
            }

            double det = chosen.Determinant();
            if (Math.Abs(det) < 1e-9)
            {
                Console.WriteLine("Обратная матрица не существует (det = 0).");
                return;
            }

            Console.WriteLine("Обратная матрица:");
            chosen.Inverse().Print();
        }

        public void SolveSystem(Matrix? A)
        {
            if (A == null) throw new InvalidOperationException("Матрица A не создана.");
            if (A.Rows != A.Cols) throw new InvalidOperationException("A должна быть квадратной.");

            double[] bvec = new double[A.Rows];
            Console.WriteLine("Введите вектор b:");
            for (int i = 0; i < A.Rows; i++)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentNullException($"b[{i}] не введён.");
                if (!double.TryParse(input, out bvec[i]))
                    throw new FormatException($"Неверный формат числа для b[{i}]: {input}");
            }

            var x = A.Solve(in bvec, out bool unique);
            if (!unique) Console.WriteLine("Нет уникального решения.");
            else
            {
                Console.WriteLine("Решение x:");
                for (int i = 0; i < x.Length; i++)
                    Console.WriteLine($"x[{i}] = {x[i]:F3}");
            }
        }

        // --- вспомогательные методы ---
        private void FillManual(ref Matrix M)
        {
            for (int i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Cols; j++)
                {
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                        throw new ArgumentNullException($"Ввод для элемента [{i},{j}] пустой.");
                    if (!double.TryParse(input, out double value))
                        throw new FormatException($"Неверный формат числа для элемента [{i},{j}]: {input}");
                    M[i, j] = value;
                }
        }

        private Matrix ReadMatrixSize(string name)
        {
            Console.Write($"Введите размеры матрицы {name} (n m): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException($"Размеры матрицы {name} не введены.");
            var parts = input.Split();
            if (parts.Length != 2 || !int.TryParse(parts[0], out int n) || !int.TryParse(parts[1], out int m))
                throw new FormatException($"Неверный формат размеров матрицы {name}.");
            return new Matrix(n, m);
        }

        private bool Check(Matrix? A, Matrix? B)
        {
            if (A == null || B == null)
            {
                Console.WriteLine("Матрицы не созданы.");
                return false;
            }
            return true;
        }
    }

    // ======================================================
    // КЛАСС MATRIX (ОПЕРАЦИИ С МАТРИЦЕЙ)
    // ======================================================
    class Matrix
    {
        private double[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new double[rows, cols];
        }

        public double this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public static Matrix Random(int rows, int cols, int a, int b)
        {
            var rnd = new Random();
            var M = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    M[i, j] = rnd.Next(a, b + 1);
            return M;
        }

        public Matrix Add(Matrix other) => Transform((i, j) => this[i, j] + other[i, j]);

        public Matrix Multiply(Matrix other)
        {
            if (Cols != other.Rows)
                throw new InvalidOperationException("Несовместимые размеры матриц.");
            var R = new Matrix(Rows, other.Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < other.Cols; j++)
                    for (int k = 0; k < Cols; k++)
                        R[i, j] += this[i, k] * other[k, j];
            return R;
        }

        public Matrix Transpose() => Transform((i, j) => this[j, i]);

        private Matrix Transform(Func<int, int, double> f)
        {
            var R = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    R[i, j] = f(i, j);
            return R;
        }

        public double[] Solve(in double[] b, out bool unique)
        {
            unique = true;
            int n = Rows;
            var aug = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    aug[i, j] = this[i, j];
                aug[i, n] = b[i];
            }

            for (int i = 0; i < n; i++)
            {
                int piv = i;
                for (int r = i + 1; r < n; r++)
                    if (Math.Abs(aug[r, i]) > Math.Abs(aug[piv, i])) piv = r;
                if (Math.Abs(aug[piv, i]) < 1e-12) { unique = false; return null!; }

                if (piv != i)
                    for (int c = 0; c <= n; c++)
                        (aug[i, c], aug[piv, c]) = (aug[piv, c], aug[i, c]);

                double div = aug[i, i];
                for (int c = i; c <= n; c++) aug[i, c] /= div;

                for (int r = 0; r < n; r++)
                {
                    if (r == i) continue;
                    double factor = aug[r, i];
                    for (int c = i; c <= n; c++) aug[r, c] -= factor * aug[i, c];
                }
            }

            var x = new double[n];
            for (int i = 0; i < n; i++) x[i] = aug[i, n];
            return x;
        }

        public double Determinant() =>
            Rows == 1 ? this[0, 0] :
            Rows == 2 ? this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0] :
            Enumerable.Range(0, Cols).Sum(j => this[0, j] * Cofactor(0, j));

        private double Cofactor(int row, int col)
        {
            var minor = new Matrix(Rows - 1, Cols - 1);
            for (int i = 0, mi = 0; i < Rows; i++)
            {
                if (i == row) continue;
                for (int j = 0, mj = 0; j < Cols; j++)
                {
                    if (j == col) continue;
                    minor[mi, mj++] = this[i, j];
                }
                mi++;
            }
            return ((row + col) % 2 == 0 ? 1 : -1) * minor.Determinant();
        }

        public Matrix Inverse()
        {
            double det = Determinant();
            if (Math.Abs(det) < 1e-9)
                throw new InvalidOperationException("Матрица вырождена (det=0).");
            var adj = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    adj[j, i] = Cofactor(i, j) / det;
            return adj;
        }

        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < Cols; j++)
                    Console.Write($"{this[i, j],8:F3} ");
                Console.WriteLine("|");
            }
        }
    }
}
