namespace WebRecreo.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public string? NombreArchivo { get; set; }
        public string? RutaArchivo { get; set; }

        public string? TipoImpresion { get; set; } // Color / B&N
        public string? Tamano { get; set; } // A4, A3
        public int Cantidad { get; set; }
        public bool Anillado { get; set; }

        public decimal PrecioTotal { get; set; }

        public string? Estado { get; set; } = "Pendiente";

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? ClienteNombre { get; set; }
        public string? ClienteTelefono { get; set; }
        public string? ClienteEmail { get; set; }
    }
}
