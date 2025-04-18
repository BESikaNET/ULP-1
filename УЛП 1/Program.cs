using System;
using System.Collections.Generic;

// Интерфейс для шахматных фигур
interface IChessPiece
{
    bool CanKill(ChessPiece other);
    void PrintMoves();
}

// Абстрактный класс
abstract class ChessPiece : IChessPiece
{
    public int X;
    public int Y;
    public string Name;

    public ChessPiece(int x, int y)
    {
        X = x;
        Y = y;
    }

    public abstract bool CanKill(ChessPiece other);
    public abstract void PrintMoves();

    ~ChessPiece()
    {
        Console.WriteLine($"Удаление {Name} с позиции ({X}, {Y})");
    }
}

// Ферзь
class Queen : ChessPiece
{
    public Queen(int x, int y) : base(x, y)
    {
        Name = "Ферзь";
    }

    public override bool CanKill(ChessPiece other)
    {
        return X == other.X || Y == other.Y || Math.Abs(X - other.X) == Math.Abs(Y - other.Y);
    }

    public override void PrintMoves()
    {
        Console.WriteLine("Ферзь ходит по вертикали, горизонтали и диагонали.");
    }
}

// Пешка
class Pawn : ChessPiece
{
    public Pawn(int x, int y) : base(x, y)
    {
        Name = "Пешка";
    }

    public override bool CanKill(ChessPiece other)
    {
        return (Math.Abs(X - other.X) == 1 && other.Y - Y == 1);
    }

    public override void PrintMoves()
    {
        Console.WriteLine("Пешка ходит вперед и бьёт по диагонали.");
    }
}

// Конь
class Knight : ChessPiece
{
    public Knight(int x, int y) : base(x, y)
    {
        Name = "Конь";
    }

    public override bool CanKill(ChessPiece other)
    {
        int dx = Math.Abs(X - other.X);
        int dy = Math.Abs(Y - other.Y);
        return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
    }

    public override void PrintMoves()
    {
        Console.WriteLine("Конь ходит буквой Г (2 на 1 или 1 на 2).");
    }
}

class Program
{
    static void Main()
    {
        List<ChessPiece> pieces = new List<ChessPiece>();

        int count;
        while (true)
        {
            try
            {
                Console.Write("Сколько фигур вы хотите ввести? ");
                count = int.Parse(Console.ReadLine());

                if (count <= 0)
                    throw new Exception("Количество фигур должно быть положительным числом.");

                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите корректное число.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        for (int i = 0; i < count; i++)
        {
            try
            {
                Console.WriteLine($"\nФигура {i + 1}:");
                Console.Write("Тип фигуры (ферзь [1], пешка [2], конь [3]): ");
                string type = Console.ReadLine().ToLower();
                int x, y;
                while (true)
                {
                    try
                    {
                        Console.Write("Координата X: ");
                        x = int.Parse(Console.ReadLine());
                        Console.Write("Координата Y: ");
                        y = int.Parse(Console.ReadLine());

                        if (x < 1 || x > 8 || y < 1 || y > 8)
                            throw new Exception("Координаты должны быть в пределах доски (1-8).");

                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ошибка: Введите корректное число для координат.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }

                ChessPiece piece = type switch
                {
                    "1" or "ферзь" => new Queen(x, y),
                    "2" or "пешка" => new Pawn(x, y),
                    "3" or "конь" => new Knight(x, y),
                    _ => throw new Exception("Неизвестный тип фигуры")
                };

                pieces.Add(piece);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        int index;
        while (true)
        {
            try
            {
                Console.Write("\nВыберите номер фигуры для анализа (начиная с 1): ");
                index = int.Parse(Console.ReadLine()) - 1;

                if (index < 0 || index >= pieces.Count)
                    throw new Exception("Неверный индекс. Пожалуйста, выберите существующую фигуру.");

                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите корректный номер.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        ChessPiece selected = pieces[index];
        Console.WriteLine($"\nАнализ для фигуры: {selected.Name} на позиции ({selected.X}, {selected.Y})");
        selected.PrintMoves();

        Console.WriteLine("\nМожет побить следующие фигуры:");
        for (int i = 0; i < pieces.Count; i++)
        {
            if (i == index) continue;
            if (selected.CanKill(pieces[i]))
            {
                Console.WriteLine($"- {pieces[i].Name} на позиции ({pieces[i].X}, {pieces[i].Y})");
            }
        }

    }
}
