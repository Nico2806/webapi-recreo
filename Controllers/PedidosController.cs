using Microsoft.AspNetCore.Mvc;
using WebRecreo.Data;
using WebRecreo.Models;

namespace WebRecreo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedido()
        {
            var form = Request.Form;

            var tipoImpresion = form["tipoImpresion"].ToString();
            var tamano = form["tamano"].ToString();
            var cantidad = int.Parse(form["cantidad"]);
            var anillado = bool.Parse(form["anillado"]);

            Console.WriteLine("TIPO: " + tipoImpresion);
            Console.WriteLine("TAMANO: " + tamano);
            Console.WriteLine("CANTIDAD: " + cantidad);
            Console.WriteLine("ANILLADO: " + anillado);

            var archivo = form.Files["archivo"];

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            var ruta = Path.Combine(uploadsPath, nombreArchivo);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var pedido = new Pedido
            {
                ClienteNombre = form["clienteNombre"],
                ClienteTelefono = form["clienteTelefono"],
                ClienteEmail = form["clienteEmail"],
                NombreArchivo = archivo.FileName,
                RutaArchivo = ruta,
                TipoImpresion = tipoImpresion,
                Tamano = tamano,
                Cantidad = cantidad,
                Anillado = anillado,
                PrecioTotal = 20,
                Estado = "Pendiente",
                Fecha = DateTime.Now
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpGet]
        public IActionResult ObtenerPedidos()
        {
            var pedidos = _context.Pedidos.ToList();
            return Ok(pedidos);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.Estado = nuevoEstado;

            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpGet("{id}/archivo")]
        public IActionResult DescargarArchivo(int id)
        {
            var pedido = _context.Pedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null || string.IsNullOrEmpty(pedido.RutaArchivo))
                return NotFound();

            var ruta = pedido.RutaArchivo;

            if (!System.IO.File.Exists(ruta))
                return NotFound();
            
            var bytes = System.IO.File.ReadAllBytes(ruta);
            var nombre = Path.GetFileName(ruta);

            return File(bytes, "application/octet-stream", nombre);
        }

    }
}
