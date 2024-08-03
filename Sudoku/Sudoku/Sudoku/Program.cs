using System;
using System.Linq;

class Program
{
    //Declaramos lo arrays necesarios
    static int[,] sudokuParaResolver = new int[9, 9];
    static int[,] miSudoku = new int[9, 9];

    static int puntaje;
    static int errores;
    static int ValidarDato(string dato)
    {
        int numero;

        while (!int.TryParse(dato, out numero))
        {
            //Si el valor ingresado no es un numero entero mostramos un mensaje de error
            EscribirConColor(ConsoleColor.Red, "El valor ingresado no es un numero entero.");
            Console.WriteLine();
            //Volvemos a pedir el numero
            dato = Console.ReadLine();
        }
        return numero;
    }
    static void EscribirConColor(ConsoleColor color, string mensaje)
    {
        Console.ForegroundColor = color;
        Console.Write("" + mensaje);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    static void Main()
    {
        Console.WriteLine("///SUDOKU///");
        Console.WriteLine("\n¿Quieres empezar el juego?\n1.Si\n2.No");
        int opcion = ValidarDato(Console.ReadLine());
        while (opcion != 1 && opcion != 2)
        {
            EscribirConColor(ConsoleColor.Red, "El nuero debe ser 1 para Si o 2 para No.");
            Console.WriteLine();
            opcion = ValidarDato(Console.ReadLine());
        }

        if (opcion == 1)
            Juego();
        //Cuando elige que No, se acaba el juego.
        EscribirConColor(ConsoleColor.Yellow, "Adios, esperamos que vuelva pronto.");
    }
    static void Juego()
    {
        int nivel;
        sudokuParaResolver = new int[9, 9];
        miSudoku = new int[9, 9];

        //Se elige la dificultad cada nuevo juego
        Console.WriteLine("Elige la dificultad\n1.Principiante\n2.Intermedio\n3.Avanzado");
        nivel = ValidarDato(Console.ReadLine());
        while (nivel < 1 || nivel > 3)
        {
            EscribirConColor(ConsoleColor.Red, "Debes ingresar un valor entre 1, 2 y 3");
            Console.WriteLine();
            nivel = ValidarDato(Console.ReadLine());
        }
        GenerarSudoku();
        CrearSudokuParaResolver(nivel);
        //El puntaje base depende de la dificultad
        puntaje = nivel * 1000;
        errores = 0;
        //El bucle de juego
        while (!HasGanado())
        {
            MostrarSudoku();
            PedirPosicion();
        }
        //Mostrar la puntuacion y se ofrece jugar de nuevo
        EscribirConColor(ConsoleColor.Green, "Felicidades, Ganaste!");
        EscribirConColor(ConsoleColor.Blue, " Puntuacion: " + puntaje);

        if (errores > 0)
        {
            EscribirConColor(ConsoleColor.Red, " Errores: " + errores);
        }

        Console.WriteLine("\n¿Quieres jugar de nuevo?\n1.Si\n2.No");
        int opcion = ValidarDato(Console.ReadLine());
        while (opcion != 1 && opcion != 2)
        {
            EscribirConColor(ConsoleColor.Red, "El nuero debe ser 1 para Si o 2 para No.");
            Console.WriteLine();
            opcion = ValidarDato(Console.ReadLine());
        }

        if (opcion == 1)
            Juego();
        //Fin del juego.
        EscribirConColor(ConsoleColor.Yellow, "Muchas gracias por jugar.");
    }

    static void GenerarSudoku()
    {
        Random rnd = new Random();
        int[] clave = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        clave = clave.OrderBy(n => rnd.Next()).ToArray();
        for (int i = 0; i < 9; i++)
            sudokuParaResolver[0, i] = clave[i];
        //Se repite en blucle hasta crear un sudoku con solucion
        while (!SudokuResuelto(0, 0))
            SudokuResuelto(0, 0);
    }

    static bool SudokuResuelto(int fila, int columna)
    {
        //Cuando la fila se pasa de 8, se reinicia
        if (fila == 9)
        {
            fila = 0;
            //Cuando fila y columna valen 9, termina el bucle
            if (++columna == 9)
                return true;
        }
        //Si el valor es distinto de 0, pasa a la siguiente fila
        if (sudokuParaResolver[fila, columna] != 0)
            return SudokuResuelto(fila + 1, columna);
        //Se va tanteando hasta encontrar un numero valido
        for (int num = 1; num <= 9; num ++)
        {
            if (EsValido(fila, columna, sudokuParaResolver, num))
            {
                sudokuParaResolver[fila, columna] = num;
                if (SudokuResuelto(fila + 1, columna))
                    return true;
            }
        }
        //Si no se encontró un numero valido, el valor se vuelve 0.
        sudokuParaResolver[fila, columna] = 0;
        return false;
    }
    static void CrearSudokuParaResolver(int nivel)
    {
        int NumerosEliminaos = 0;
        int AEliminar = 0;
        Random rnd = new Random();
        //Elegimos la cantadidad de espacio segun la dificultad
        switch (nivel)
        {
            case 1: AEliminar = 15; break;
            case 2: AEliminar = 30; break;
            case 3: AEliminar = 45; break;
        }
        //Elegimos un espacio aleatorio y lo volvemos 0.
        while (NumerosEliminaos < AEliminar)
        {
            int fila = rnd.Next(0, 9);
            int columna = rnd.Next(0, 9);
            if (sudokuParaResolver[fila, columna] != 0)
            {
                sudokuParaResolver[fila, columna] = 0;
                NumerosEliminaos++;
            }
        }
        //Copiamos el sudoku generado en el sudoku que vamos a manipular
        Array.Copy(sudokuParaResolver, miSudoku, 81);
    }

    static bool EsValido(int fila, int columna, int[,] cudricula, int numero)
    {
        //Validar fila
        for (int i = 0; i < 9; i++)
        {
            if (cudricula[fila, i] == numero)
            {
                if (columna != i)
                    return false;
            }
        }
        //Validar columna
        for (int i = 0; i < 9; i++)
        {
            if (cudricula[i, columna] == numero)
            {
                if (fila != i)
                    return false;
            }
        }
        //Validar subcuadricula
        int filaInicial = fila % 3;
        int columnaInicial = columna % 3;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int filaSubC = fila - filaInicial + i;
                int columnaSubC = columna - columnaInicial + j;
                if (cudricula[filaSubC, columnaSubC] == numero)
                {
                    if (i != filaInicial && j != columnaInicial)
                        return false;
                }
            }
        }
        return true;
    }

    static void MostrarSudoku()
    {
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
                Console.WriteLine("+-------+-------+-------+");
            for (int j = 0; j < 9; j++)
            {
                if (j % 3 == 0)
                    Console.Write("| ");
                //Cambiamos el color para diferenciar los valores no manipulables
                if (sudokuParaResolver[i, j] != 0)
                {
                    EscribirConColor(ConsoleColor.Gray, "" + miSudoku[i, j]);
                }
                else
                {
                    //Cambiamos el color para destacar los numeros
                    if (miSudoku[i, j] != 0)
                    {
                        if (EsValido(i, j, miSudoku, miSudoku[i, j]))
                        {
                            EscribirConColor(ConsoleColor.Green, "" + miSudoku[i, j]);
                        }
                        else
                        {
                            EscribirConColor(ConsoleColor.Red, "" + miSudoku[i, j]);
                        }
                    }
                    else
                    {
                        EscribirConColor(ConsoleColor.DarkGray, "" + miSudoku[i, j]);
                    }
                }
                Console.Write(" ");
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("+-------+-------+-------+");
    }
    static void PedirPosicion()
    {
        int fila, columna;
        bool posOk = false;
        do
        {
            //pedir fila
            Console.WriteLine();
            bool filaOk = false;
            do
            {
                Console.WriteLine("en que fila del 1 al 9?");
                fila = ValidarDato(Console.ReadLine());
                if (fila >= 1 && fila <= 9)
                {
                    fila--;
                    filaOk = true;
                }
                else
                {
                    EscribirConColor(ConsoleColor.Red, "Fila erronea");
                    Console.WriteLine();
                }
            } while (!filaOk);
            //pedir columna
            bool columnaOk = false;
            do
            {
                Console.WriteLine("en que columna del 1 al 9?");
                columna = ValidarDato(Console.ReadLine());
                if (columna >= 1 && columna <= 9)
                {
                    columna--;
                    columnaOk = true;
                }
                else
                {
                    EscribirConColor(ConsoleColor.Red, "Columna erronea");
                    Console.WriteLine();
                }
            } while (!columnaOk);
            //Validar que el numero sea manipulable
            if (sudokuParaResolver[fila, columna] != 0)
            {
                EscribirConColor(ConsoleColor.Red, "Casilla ocupada");
                Console.WriteLine();
            }
            else
            {
                miSudoku[fila, columna] = PreguntarNumero();
                posOk = true;
                //Si el numero es incorrecto le restamos un punto
                if (!EsValido(fila, columna, miSudoku, miSudoku[fila, columna]))
                {
                    errores++;
                    puntaje -= 10;
                }
            }
        } while (!posOk);
    }
    static int PreguntarNumero()
    {
        int num = 0;
        Console.WriteLine();
        Console.WriteLine("Introduce el valor de la casilla");
        num = ValidarDato(Console.ReadLine());
        //Si el nuero esta fuera del rango vuelve a preguntar el numero
        while (num < 1 || num > 9)
        {
            EscribirConColor(ConsoleColor.Red, "Debes ingresar un numero en el rango del 1 al 9");
            num = ValidarDato(Console.ReadLine());
        }
        return num;
    }
    static bool HasGanado()
    {
        //Comprobar si todovia quedan ceros
        for (int fila = 0; fila < 9; fila++)
        {
            for (int columna = 0; columna < 9; columna++)
            {
                if (miSudoku[fila, columna] == 0)
                    return false;
            }
        }
        //Comprobar si todos los numeros colocados son validos
        for (int fila = 0; fila < 9; fila++)
        {
            for (int columna = 0; columna < 9; columna++)
            {
                if (sudokuParaResolver[fila, columna] == 0)
                {
                    if (!EsValido(fila, columna, miSudoku, miSudoku[fila, columna]))
                        return false;
                }
            }
        }
        return true;
    }
}
// referencias
//https://www.youtube.com/@unitywok
//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/
//https://www.w3schools.com/cs/