using System;
using System.Collections.Generic;

namespace Metodos_de_inventario
{
    internal class Program
    {
        // Diccionario para gestionar los productos con clave unica en el inventario
        static Dictionary<string, Stack<Producto>> Pila = new Dictionary<string, Stack<Producto>>(); // UEPS
        static Dictionary<string, Queue<Producto>> Cola = new Dictionary<string, Queue<Producto>>(); // PEPS
        static Dictionary<string, List<Producto>> Lista = new Dictionary<string, List<Producto>>(); // Costo promedio

        // Diccionario para guardar la paleta de colores de la interfaz
        static Dictionary<string, ConsoleColor> Colores = new Dictionary<string, ConsoleColor>()
        {
            { "Titulo", ConsoleColor.DarkYellow },
            { "Fondo", ConsoleColor.Black },
            { "Texto", ConsoleColor.Gray },
            { "Resaltado", ConsoleColor.DarkRed },
            { "Confirmacion", ConsoleColor.DarkGreen },
            { "Entrada", ConsoleColor.DarkCyan },
            { "Invalido", ConsoleColor.DarkGray }
        };

        static int Correlativo; // Codigo de producto

        static void Main(string[] args)
        {
            Correlativo = 1;

            // Colores base de la interfaz
            Console.BackgroundColor = Colores["Fondo"];
            Console.ForegroundColor = Colores["Texto"];

            Menu();
        }

        static void Menu()
        {
            int opcion;
            // Ciclo principal que muestra el menú de opciones y permite la interacción del usuario con el inventario.
            do
            {
                Console.Clear();

                Console.ForegroundColor = Colores["Titulo"];
                Console.WriteLine("\n\t=== Sistema de Gestión de Inventario ===\n");

                Console.ForegroundColor = Colores["Texto"];
                Console.WriteLine("\t1. Agregar producto");

                if (Lista.Count == 0)
                {
                    Console.ForegroundColor = Colores["Invalido"];
                }
                Console.WriteLine("\t2. Realizar venta");

                Console.ForegroundColor = Colores["Texto"];
                Console.WriteLine("\t3. Ver inventario");
                Console.WriteLine("\t4. Salir");

                Console.Write("\n\tSeleccione una opción: ");

                // Validación de la opción ingresada por el usuario.
                Console.ForegroundColor = Colores["Entrada"];
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            Ingresar_Producto(); // Llamada para agregar un nuevo producto.
                            break;
                        case 2:
                            if (Lista.Count == 0)
                            {
                                Menu();
                                return;
                            }
                            RealizarVenta(); // Llamada para realizar una venta.
                            break;
                        case 3:
                            VerInventario(); // Llamada para mostrar el inventario actual.
                            break;
                        case 4:
                            Console.WriteLine("\tSaliendo...");
                            break;
                        default:
                            Console.ForegroundColor = Colores["Resaltado"];
                            Console.WriteLine("\n\tOpción no válida. Intente nuevamente.");
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = Colores["Resaltado"];
                    Console.WriteLine("\n\tEntrada no válida. Intente nuevamente.");
                }

                Console.WriteLine("\tPresione una tecla para continuar...");
                Console.ReadKey();
            } while (opcion != 4); // El ciclo se ejecuta hasta que el usuario elija salir.
        }

        static void Ingresar_Producto()
        {
            int opcion;
            string codigo = "", nombre = "";

            // Interfaz
            Console.Clear();

            Console.ForegroundColor = Colores["Titulo"];
            Console.WriteLine("\n\t=== Agregar Producto ===\n");

            Console.ForegroundColor = Colores["Texto"];
            Console.WriteLine("\t1. Producto nuevo");

            if (Lista.Count == 0)
            {
                Console.ForegroundColor = Colores["Invalido"];
            }
            Console.WriteLine("\t2. Producto Existente");
            Console.ForegroundColor = Colores["Texto"];
            Console.WriteLine("\t3. Regresar");
            Console.Write("\n\tSeleccione una opción: ");

            // Validación de la opción ingresada por el usuario.
            Console.ForegroundColor = Colores["Entrada"];
            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.Clear();
                Console.ForegroundColor = Colores["Titulo"];
                Console.WriteLine("\n\t=== Agregar Producto ===\n");

                switch (opcion)
                {
                    case 1:
                        // Generación de un nuevo código
                        Console.ForegroundColor = Colores["Texto"];
                        Console.Write("\tCódigo del producto: ");
                        codigo = Convert.ToString(Correlativo++);
                        int longitud = 4;
                        while (codigo.Length < longitud)
                        {
                            codigo = "0" + codigo;
                        }
                        Console.WriteLine(codigo);

                        // Solicitud del nombre del producto.
                        Console.Write("\tNombre del producto: ");
                        Console.ForegroundColor = Colores["Entrada"];
                        nombre = Console.ReadLine();
                        break;
                    case 2:
                        if (Lista.Count == 0)
                        {
                            Ingresar_Producto();
                            return;
                        }
                        // Solicitud del código del productoo.
                        do
                        {
                            Console.ForegroundColor = Colores["Texto"];
                            Console.Write("\tCódigo del producto: ");

                            Console.ForegroundColor = Colores["Entrada"];
                            codigo = Console.ReadLine();

                            if (!Lista.ContainsKey(codigo))
                            {
                                Console.ForegroundColor = Colores["Resaltado"];
                                Console.WriteLine("\tEl producto no fue encontrado. Ingrese un código diferente.");
                            }
                        } while (!Lista.ContainsKey(codigo));

                        // Asignamos el nombre del producto
                        nombre = Lista[codigo][0].get_Nombre();
                        Console.ForegroundColor = Colores["Texto"];
                        Console.WriteLine("\tNombre del producto: " + nombre);
                        break;
                    default:
                        Console.ForegroundColor = Colores["Resaltado"];
                        Console.WriteLine("\tOpción no válida. Intente nuevamente.");
                        Menu();
                        return;

                }
            }
            else
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.WriteLine("\n\tEntrada no válida. Intente nuevamente.");
                Console.ReadKey();
                Ingresar_Producto();
                return;
            }

            // Validación de la cantidad ingresada, debe ser un número entero positivo.
            int cantidad;
            Console.ForegroundColor = Colores["Texto"];
            Console.Write("\tCantidad: ");
            Console.ForegroundColor = Colores["Entrada"];
            while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0)
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.Write("\n\tEntrada no válida. Intente nuevamente: ");

                Console.ForegroundColor = Colores["Texto"];
                Console.Write("\n\tCantidad: ");
                Console.ForegroundColor = Colores["Entrada"];
            }

            // Validación del precio ingresado, debe ser un número decimal positivo.
            double precio;
            Console.ForegroundColor = Colores["Texto"];
            Console.Write("\tPrecio: ");
            Console.ForegroundColor = Colores["Entrada"];
            while (!double.TryParse(Console.ReadLine(), out precio) || precio <= 0)
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.WriteLine("\n\tEntrada no válida. Intente nuevamente.");

                Console.ForegroundColor = Colores["Texto"];
                Console.Write("\n\tPrecio: ");
                Console.ForegroundColor = Colores["Entrada"];
            }

            // Creación de un nuevo producto y su adición a los tipos de inventario.
            Producto nuevoProducto = new Producto(codigo, nombre, cantidad, precio);

            if (opcion == 1) // Se agrega el nuevo codigo
            {
                Pila.Add(codigo, new Stack<Producto>()); // Se agrega al stack para UEPS.
                Cola.Add(codigo, new Queue<Producto>()); // Se agrega a la cola para PEPS.
                Lista.Add(codigo, new List<Producto>()); // Se agrega a la lista para costo promedio.
            }

            Pila[codigo].Push(nuevoProducto); // Se agrega al stack para UEPS.
            Cola[codigo].Enqueue(nuevoProducto); // Se agrega a la cola para PEPS.
            Lista[codigo].Add(nuevoProducto); // Se agrega a la lista para costo promedio.
        }

        static void RealizarVenta()
        {
            int opcion;
            string codigo = "", nombre = "";

            // Interfaz
            Console.Clear();

            Console.ForegroundColor = Colores["Titulo"];
            Console.WriteLine("\n\t=== Relizar Venta ===\n");

            Console.ForegroundColor = Colores["Texto"];
            Console.WriteLine("\t1. PEPS");
            Console.WriteLine("\t2. UEPS");
            Console.WriteLine("\t3. Costo promedio");
            Console.Write("\n\tSeleccione una opción: ");

            // Validación de la opción ingresada por el usuario.
            Console.ForegroundColor = Colores["Entrada"];
            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.Clear();
                Console.ForegroundColor = Colores["Titulo"];
                Console.WriteLine("\n\t=== Relizar Venta ===\n");
            }

            // Solicitud del código del productoo.
            do
            {
                Console.ForegroundColor = Colores["Texto"];
                Console.Write("\tCódigo del producto: ");

                Console.ForegroundColor = Colores["Entrada"];
                codigo = Console.ReadLine();

                if (!Lista.ContainsKey(codigo))
                {
                    Console.ForegroundColor = Colores["Resaltado"];
                    Console.WriteLine("\tEl producto no fue encontrado. Ingrese un código diferente.");
                }
            } while (!Lista.ContainsKey(codigo));

            // Asignamos el nombre del producto
            nombre = Lista[codigo][0].get_Nombre();
            Console.WriteLine("\tNombre del producto: " + nombre);

            // Validación de la cantidad ingresada, debe ser un número entero positivo.
            int cantidad;
            Console.ForegroundColor = Colores["Texto"];
            Console.Write("\tCantidad: ");
            Console.ForegroundColor = Colores["Entrada"];
            while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0)
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.Write("\n\tEntrada no válida. Intente nuevamente: ");

                Console.ForegroundColor = Colores["Texto"];
                Console.Write("\n\tCantidad: ");
                Console.ForegroundColor = Colores["Entrada"];
            }

            // Validacion de la catidad ingresada, debe haber stock suficiente
            int stock = 0;
            foreach (Producto producto in Lista[codigo])
            {
                stock += producto.get_Cantidad();
            }
            if (stock <= cantidad)
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.WriteLine("\tNo hay stock suficiente de " + nombre);
                return;
            }

            switch (opcion)
            {
                case 1:
                    MetodoPEPS(codigo, cantidad);
                    break;
                case 2:
                    MetodoUEPS(codigo, cantidad);
                    break;
                case 3:
                    MetodoCostoPromedio(codigo, cantidad);
                    break;
            }
        }
        static void VerInventario()
        {
            Console.Clear();
            Console.ForegroundColor = Colores["Titulo"];
            Console.WriteLine("\t=== Inventario Actual ===");

            if (Lista.Count == 0)
            {
                Console.ForegroundColor = Colores["Resaltado"];
                Console.WriteLine("\tNo hay productos en el inventario.");
                return;
            }

            Console.ForegroundColor = Colores["Texto"];
            // Muestra todos los productos del inventario.
            foreach (string codigo in Lista.Keys)
            {
                foreach (Producto producto in Lista[codigo])
                {
                    if (producto.get_Cantidad() > 0)
                    {
                        Console.WriteLine($"\tCódigo: {producto.get_Codigo()}, Nombre: {producto.get_Nombre()}, Cantidad: {producto.get_Cantidad()}, Precio: {producto.get_Precio():C}");
                    }
                }
            }
        }
        static void MetodoPEPS(string codigo, int total)
        {
            Console.ForegroundColor = Colores["Confirmacion"];
            Console.WriteLine("\n\tVendiendo...");
            while (total > 0)
            {
                Console.Write("\t" + Cola[codigo].Peek().get_Nombre() + ", ");
                int cantidad = Cola[codigo].Peek().get_Cantidad();
                double precio_unitario = Cola[codigo].Peek().get_Precio();
                if (total >= cantidad) // El total del pedido es mayor al lote
                {
                    Console.WriteLine(cantidad + ", " + (cantidad * precio_unitario).ToString("C2"));
                    total -= cantidad;
                    Cola[codigo].Dequeue().set_Cantidad(0);
                }
                else // El total del pedido es menor al lote
                {
                    Console.WriteLine(total + ", " + (total * precio_unitario).ToString("C2"));
                    Cola[codigo].Peek().set_Cantidad(cantidad - total);
                    total = 0;
                }
            }
        }
        static void MetodoUEPS(string codigo, int total)
        {
            Console.ForegroundColor = Colores["Confirmacion"];
            Console.WriteLine("\n\tVendiendo...");
            while (total > 0)
            {
                Console.Write("\t" + Pila[codigo].Peek().get_Nombre() + ", ");
                int cantidad = Pila[codigo].Peek().get_Cantidad();
                double precio_unitario = Pila[codigo].Peek().get_Precio();
                if (total >= cantidad) // El total del pedido es mayor al lote
                {
                    Console.WriteLine(cantidad + ", " + (cantidad * precio_unitario).ToString("C2"));
                    total -= cantidad;
                    Pila[codigo].Pop().set_Cantidad(0);
                }
                else // El total del pedido es menor al lote
                {
                    Console.WriteLine(total + ", " + (total * precio_unitario).ToString("C2"));
                    Pila[codigo].Peek().set_Cantidad(cantidad - total);
                    total = 0;
                }
            }
        }
        static void MetodoCostoPromedio(string codigo, int total)
        {
            Console.ForegroundColor = Colores["Confirmacion"];
            Console.WriteLine("\n\tVendiendo...");

            double costo_promedio = 0;
            foreach (Producto p in Lista[codigo])
            {
                costo_promedio += p.get_Precio() / p.get_Cantidad();
            }
            for (int i = Lista[codigo].Count - 1; i >= 0; i--)
            {
                int cantidad = Lista[codigo][i].get_Cantidad();
                double precio_unitario = Lista[codigo][i].get_Precio();
                Lista[codigo][i].set_Cantidad(0);

                Console.Write("\t" + Lista[codigo][i].get_Nombre() + ", ");

                if (total >= cantidad) // El total del pedido es mayor al lote
                {
                    Console.WriteLine(cantidad + ", " + (cantidad * precio_unitario).ToString("C2"));
                    total -= cantidad;
                    Lista[codigo][i].set_Cantidad(0);
                }
                else // El total del pedido es menor al lote
                {
                    Console.WriteLine(total + ", " + (total * precio_unitario).ToString("C2"));
                    Lista[codigo][i].set_Cantidad(cantidad - total);
                    total = 0;
                    return;
                }
            }
        }
    }
}
