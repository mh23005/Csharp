namespace Metodos_de_inventario
{
    public class Producto
    {
        private string Codigo;
        private string Nombre;
        private int Cantidad;
        private double Precio;

        public Producto(string codigo, string nombre, int cantidad, double precio)
        {
            Codigo = codigo;
            Nombre = nombre;
            Cantidad = cantidad;
            Precio = precio;
        }

        public string get_Codigo()
        {
            return Codigo;
        }

        public string get_Nombre()
        {
            return Nombre;
        }

        public int get_Cantidad()
        {
            return Cantidad;
        }

        public void set_Cantidad(int cantidad)
        {
            Cantidad = cantidad;
        }

        public double get_Precio()
        {
            return Precio;
        }
    }
}
