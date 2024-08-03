using System;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al juego de Sudoku");

            int maximoPuntaje = 0;

            while (true)
            {
                int nivel = SeleccionarNivel();

                int[,] sudoku = GenerarSudoku(nivel);

                MostrarSudoku(sudoku);

                int fallos = JugarSudoku(sudoku);

                int puntaje = CalcularPuntaje(fallos);

                if (puntaje > maximoPuntaje)
                {
                    maximoPuntaje = puntaje;
                }

                MostrarResultados(puntaje);

                if (!JugarNuevamente())
                {
                    break;
                }
            }

            Console.WriteLine("Puntaje máximo alcanzado: " + maximoPuntaje);
            Console.WriteLine("¡Gracias por jugar!");
        }

        static int SeleccionarNivel()
        {
            Console.WriteLine("\nSelecciona el nivel de dificultad:");
            Console.WriteLine("1 - Principiante");
            Console.WriteLine("2 - Intermedio");
            Console.WriteLine("3 - Avanzado");

            int nivel = LeerEntero("Ingresa el número correspondiente al nivel: ");

            return nivel;
        }

        static int[,] GenerarSudoku(int nivel)
        {
            Random rnd = new Random();
            int filas, columnas;

            if (nivel == 1)
            {
                filas = 4;
                columnas = 4;
            }
            else if (nivel == 2)
            {
                filas = 6;
                columnas = 6;
            }
            else if (nivel == 3)
            {
                filas = 9;
                columnas = 9;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nivel inválido. Selecciona un nivel válido.");
                Console.ForegroundColor = ConsoleColor.Gray;

                return null;
            }

            int[,] sudoku = new int[filas, columnas];

            int numerosColocados = 0;
            int maxNumerosColocados = filas * columnas / 2; // La mitad de las celdas se llenarán inicialmente

            while (numerosColocados < maxNumerosColocados)
            {
                int fila = rnd.Next(0, filas);
                int columna = rnd.Next(0, columnas);
                int numero = rnd.Next(1, 10);

                if (sudoku[fila, columna] == 0 && EsValido(sudoku, fila, columna, numero))
                {
                    sudoku[fila, columna] = numero;
                    numerosColocados++;
                }
            }

            return sudoku;
        }

        static void MostrarSudoku(int[,] sudoku)
        {
            int filas = sudoku.GetLength(0);
            int columnas = sudoku.GetLength(1);
            int tamanoCelda = 3;

            Console.WriteLine("-------------------------");

            for (int i = 0; i < filas; i++)
            {
                if (i != 0 && i % tamanoCelda == 0)
                {
                    Console.WriteLine("|-------+-------+-------|");
                }

                for (int j = 0; j < columnas; j++)
                {
                    if (j != 0 && j % tamanoCelda == 0)
                    {
                        Console.Write("| ");
                    }

                    if (sudoku[i, j] == 0)
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write(sudoku[i, j] + " ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("-------------------------");
        }

        static int JugarSudoku(int[,] sudoku)
        {
            int fallos = 0;
            int filas = sudoku.GetLength(0);
            int columnas = sudoku.GetLength(1);

            while (true)
            {
                Console.WriteLine();
                int fila = LeerEntero("Ingresa la fila (1-" + filas + "): ");
                int columna = LeerEntero("Ingresa la columna (1-" + columnas + "): ");
                int valor = LeerEntero("Ingresa el número (1-9): ");

                if (fila < 1 || fila > filas || columna < 1 || columna > columnas || valor < 1 || valor > 9)
                {
                    Console.WriteLine("Entrada inválida. Ingresa valores dentro del rango permitido.");
                    continue;
                }

                if (sudoku[fila - 1, columna - 1] != 0)
                {
                    Console.WriteLine("La celda seleccionada ya contiene un número.");
                    continue;
                }

                if (!EsValido(sudoku, fila - 1, columna - 1, valor))
                {
                    Console.WriteLine("Valor inválido. Ya existe el número en la fila, columna o región.");
                    fallos++;
                }

                sudoku[fila - 1, columna - 1] = valor;
                MostrarSudoku(sudoku);

                if (JuegoCompletado(sudoku))
                {
                    Console.WriteLine("¡Felicidades! Has completado el Sudoku.");
                    break;
                }
            }

            return fallos;
        }

        static bool EsValido(int[,] sudoku, int fila, int columna, int valor)
        {
            int filas = sudoku.GetLength(0);
            int columnas = sudoku.GetLength(1);
            int tamanoCelda = 3;
            int filaInicio = (fila / tamanoCelda) * tamanoCelda;
            int columnaInicio = (columna / tamanoCelda) * tamanoCelda;

            for (int i = 0; i < columnas; i++)
            {
                if (sudoku[fila, i] == valor)
                {
                    return false;
                }
            }

            for (int i = 0; i < filas; i++)
            {
                if (sudoku[i, columna] == valor)
                {
                    return false;
                }
            }

            for (int i = filaInicio; i < filaInicio + tamanoCelda; i++)
            {
                for (int j = columnaInicio; j < columnaInicio + tamanoCelda; j++)
                {
                    if (sudoku[i, j] == valor)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static bool JuegoCompletado(int[,] sudoku)
        {
            int filas = sudoku.GetLength(0);
            int columnas = sudoku.GetLength(1);

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static int CalcularPuntaje(int fallos)
        {
            int puntaje = 100 - (fallos * 10);
            return puntaje < 0 ? 0 : puntaje;
        }

        static void MostrarResultados(int puntaje)
        {
            Console.WriteLine("Puntaje obtenido: " + puntaje);
        }

        static bool JugarNuevamente()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("¿Deseas jugar nuevamente? (s/n): ");
                string respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    return true;
                }
                else if (respuesta == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Respuesta inválida. Ingresa 's' para sí o 'n' para no.");
                }
            }
        }

        static int LeerEntero(string mensaje)
        {
            int numero;
            bool entradaValida = false;

            do
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();

                if (int.TryParse(entrada, out numero))
                {
                    entradaValida = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Ingresa un número entero.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            } while (!entradaValida);

            return numero;
        }

        static bool ValidarRango(int min, int max)
        {
            bool esValido = false;

            return esValido;
        }
    }
}

