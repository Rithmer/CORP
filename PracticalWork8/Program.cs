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
                Console.WriteLine("1. Создать/Заполнить матрицу");
                Console.WriteLine("2. Показать матрицы");
                Console.WriteLine("3. A + B");
                Console.WriteLine("4. A * B");
                Console.WriteLine("5. Транспонирование");
                Console.WriteLine("6. Найти детерминант");
                Console.WriteLine("7. Найти обратную матрицу");
                Console.WriteLine("8. Решить Ax = b");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                string? key = Console.ReadLine();

                if (key == "0") break;

                try
                {
                    switch (key)
                    {
                        case "1":
                            Console.Write("Выберите матрицу (A/B): ");
                            string? choice1 = Console.ReadLine()?.Trim().ToUpper();
                            if (choice1 == "A") service.CreateOrFillMatrix(ref A, "A");
                            else if (choice1 == "B") service.CreateOrFillMatrix(ref B, "B");
                            else Console.WriteLine("Неверная матрица.");
                            break;
                        case "2": service.PrintMatrices(A, B); break;
                        case "3": service.AddMatrices(A, B); break;
                        case "4": service.MultiplyMatrices(A, B); break;
                        case "5":
                            Console.Write("Выберите матрицу (A/B): ");
                            string? choice5 = Console.ReadLine()?.Trim().ToUpper();
                            if (choice5 == "A") service.TransposeMatrix(ref A, "A");
                            else if (choice5 == "B") service.TransposeMatrix(ref B, "B");
                            else Console.WriteLine("Неверная матрица.");
                            break;
                        case "6":
                            Console.Write("Выберите матрицу (A/B): ");
                            string? choice6 = Console.ReadLine()?.Trim().ToUpper();
                            if (choice6 == "A") service.ShowDeterminant(A, "A");
                            else if (choice6 == "B") service.ShowDeterminant(B, "B");
                            else Console.WriteLine("Неверная матрица.");
                            break;
                        case "7":
                            Console.Write("Выберите матрицу (A/B): ");
                            string? choice7 = Console.ReadLine()?.Trim().ToUpper();
                            if (choice7 == "A") service.ShowInverse(A, "A");
                            else if (choice7 == "B") service.ShowInverse(B, "B");
                            else Console.WriteLine("Неверная матрица.");
                            break;
                        case "8": service.SolveSystem(A); break;
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
    // СЕРВИСНЫЙ КЛАСС
    // ======================================================
    class MatrixService
    {
        public void CreateOrFillMatrix(ref Matrix? M, string name)
        {
            Console.WriteLine($"\nВы выбрали матрицу {name}.");
            Console.WriteLine("1. Создать вручную");
            Console.WriteLine("2. Заполнить случайно");
            Console.Write("Выбор: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    M = ReadMatrixSize(name);
                    FillManual(ref M);
                    break;
                case "2":
                    if (M == null) M = ReadMatrixSize(name);
                    Console.Write("Введите диапазон a b: ");
                    var parts = Console.ReadLine()?.Split();
                    if (parts == null || parts.Length != 2 || !int.TryParse(parts[0], out int a) || !int.TryParse(parts[1], out int b))
                        throw new FormatException("Неверный формат диапазона.");
                    M = Matrix.Random(M.Rows, M.Cols, a, b);
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
            Console.WriteLine($"Матрица {name} готова.");
        }

        public void PrintMatrices(Matrix? A, Matrix? B)
        {
            if (A != null)
            {
                Console.WriteLine("\nМатрица A:");
                A.Print();
            }
            if (B != null)
            {
                Console.WriteLine("\nМатрица B:");
                B.Print();
            }
            if (A == null && B == null) Console.WriteLine("Матрицы не созданы.");
        }

        public void AddMatrices(Matrix? A, Matrix? B)
        {
            if (A == null || B == null) { Console.WriteLine("Матрицы не созданы."); return; }
            if (A.Rows != B.Rows || A.Cols != B.Cols) { Console.WriteLine("Размеры не совпадают."); return; }
            Console.WriteLine("Результат A + B:");
            A.Add(B).Print();
        }

        public void MultiplyMatrices(Matrix? A, Matrix? B)
        {
            if (A == null || B == null) { Console.WriteLine("Матрицы не созданы."); return; }
            if (A.Cols != B.Rows) { Console.WriteLine("Несовместимые размеры для умножения."); return; }
            Console.WriteLine("Результат A * B:");
            A.Multiply(B).Print();
        }

        public void TransposeMatrix(ref Matrix? M, string name)
        {
            if (M == null) { Console.WriteLine($"Матрица {name} не создана."); return; }
            M = M.Transpose();
            Console.WriteLine($"Транспонированная матрица {name}:");
            M.Print();
        }

        public void ShowDeterminant(Matrix? M, string name)
        {
            if (M == null) { Console.WriteLine($"Матрица {name} не создана."); return; }
            if (M.Rows != M.Cols) { Console.WriteLine("Матрица не квадратная."); return; }
            Console.WriteLine($"Детерминант {name}: {M.Determinant():F12}");
        }

        public void ShowInverse(Matrix? M, string name)
        {
            if (M == null) { Console.WriteLine($"Матрица {name} не создана."); return; }
            if (M.Rows != M.Cols) { Console.WriteLine("Матрица не квадратная."); return; }
            decimal det = M.Determinant();
            if (Decimal.Abs(det) < 1e-28m) { Console.WriteLine("Обратная матрица не существует."); return; }
            Console.WriteLine($"Обратная матрица {name}:");
            M.Inverse().Print();
        }

        public void SolveSystem(Matrix? A)
        {
            if (A == null) throw new InvalidOperationException("Матрица A не создана.");
            if (A.Rows != A.Cols) throw new InvalidOperationException("A должна быть квадратной.");

            decimal[] bvec = new decimal[A.Rows];
            Console.WriteLine("Введите вектор b:");
            for (int i = 0; i < A.Rows; i++)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentNullException($"b[{i}] не введён.");
                if (!decimal.TryParse(input, out bvec[i]))
                    throw new FormatException($"Неверный формат числа для b[{i}]: {input}");
            }

            var x = A.Solve(in bvec, out bool unique);
            if (!unique) Console.WriteLine("Нет уникального решения.");
            else
            {
                Console.WriteLine("Решение x:");
                for (int i = 0; i < x.Length; i++)
                    Console.WriteLine($"x[{i}] = {x[i]:F12}");
            }
        }

        // --- вспомогательные методы ---
        private void FillManual(ref Matrix M)
        {
            Console.WriteLine("Введите элементы построчно:");
            for (int i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Cols; j++)
                {
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                        throw new ArgumentNullException($"Ввод для элемента [{i},{j}] пустой.");
                    if (!decimal.TryParse(input, out decimal value))
                        throw new FormatException($"Неверный формат числа для элемента [{i},{j}]: {input}");
                    M[i, j] = value;
                }
        }

        private Matrix ReadMatrixSize(string name)
        {
            Console.Write($"Введите размеры матрицы {name} (строки колонки): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException($"Размеры матрицы {name} не введены.");
            var parts = input.Split();
            if (parts.Length != 2 || !int.TryParse(parts[0], out int n) || !int.TryParse(parts[1], out int m))
                throw new FormatException($"Неверный формат размеров матрицы {name}.");
            return new Matrix(n, m);
        }
    }

    // ======================================================
    // КЛАСС MATRIX
    // ======================================================
    class Matrix
    {
        private decimal[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new decimal[rows, cols];
        }

        public decimal this[int i, int j]
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
                    M[i, j] = a + (decimal)rnd.NextDouble() * (b - a);
            return M;
        }

        public Matrix Add(Matrix other) => Transform((i, j) => this[i, j] + other[i, j]);

        public Matrix Multiply(Matrix other)
        {
            var R = new Matrix(Rows, other.Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < other.Cols; j++)
                    for (int k = 0; k < Cols; k++)
                        R[i, j] += this[i, k] * other[k, j];
            return R;
        }

        public Matrix Transpose() => Transform((i, j) => this[j, i]);

        private Matrix Transform(Func<int, int, decimal> f)
        {
            var R = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    R[i, j] = f(i, j);
            return R;
        }

        public decimal[] Solve(in decimal[] b, out bool unique)
        {
            unique = true;
            int n = Rows;
            var aug = new decimal[n, n + 1];
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
                    if (Decimal.Abs(aug[r, i]) > Decimal.Abs(aug[piv, i])) piv = r;
                if (Decimal.Abs(aug[piv, i]) < 1e-28m) { unique = false; return null!; }

                if (piv != i)
                    for (int c = 0; c <= n; c++)
                        (aug[i, c], aug[piv, c]) = (aug[piv, c], aug[i, c]);

                decimal div = aug[i, i];
                for (int c = i; c <= n; c++) aug[i, c] /= div;

                for (int r = 0; r < n; r++)
                {
                    if (r == i) continue;
                    decimal factor = aug[r, i];
                    for (int c = i; c <= n; c++) aug[r, c] -= factor * aug[i, c];
                }
            }

            var x = new decimal[n];
            for (int i = 0; i < n; i++) x[i] = aug[i, n];
            return x;
        }

        public decimal Determinant() =>
            Rows == 1 ? this[0, 0] :
            Rows == 2 ? this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0] :
            Enumerable.Range(0, Cols).Sum(j => this[0, j] * Cofactor(0, j));

        private decimal Cofactor(int row, int col)
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
            return ((row + col) % 2 == 0 ? 1m : -1m) * minor.Determinant();
        }

        public Matrix Inverse()
        {
            decimal det = Determinant();
            if (Decimal.Abs(det) < 1e-28m)
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
                    Console.Write($"{this[i, j],16:F12} ");
                Console.WriteLine("|");
            }
        }
    }
}
