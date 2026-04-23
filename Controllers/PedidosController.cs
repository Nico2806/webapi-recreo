using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebRecreo.Models;
using System.Globalization;

namespace WebRecreo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private static List<Pedido> pedidos = new List<Pedido>();

        // ✅ CREAR PEDIDO
        [HttpPost]
        public IActionResult Post()
        {
            var form = Request.Form;

            foreach (var key in form.Keys)
            {
                Console.WriteLine($"KEY: {key} - VALUE: {form[key]}");
            }

            var nuevo = new Pedido
            {
                Id = pedidos.Count + 1,
                Fecha = DateTime.Now,
                ClienteNombre = form["clienteNombre"],
                ClienteTelefono = form["clienteTelefono"],
                ClienteEmail = form["clienteEmail"],
                TipoImpresion = form["tipoImpresion"],
                Tamano = form["tamano"],
                Cantidad = int.TryParse(form["cantidad"], out var c) ? c : 0,
                Anillado = bool.TryParse(form["anillado"], out var a) && a,
                Estado = "Pendiente"
            };

            // 🔥 FIX PRECIO


            

            var precioStr = Request.Form["precioTotal"].FirstOrDefault();

            Console.WriteLine("PRECIO RAW: " + precioStr);

            if (!string.IsNullOrEmpty(precioStr))
            {
                nuevo.PrecioTotal = Convert.ToDecimal(precioStr, CultureInfo.InvariantCulture);
            }
            Console.WriteLine("PRECIO FINAL: " + nuevo.PrecioTotal);

            pedidos.Add(nuevo);

            return Ok(nuevo);
        }

        // ✅ OBTENER PEDIDOS
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(pedidos);
        }

        // ✅ CAMBIAR ESTADO
        [HttpPut("{id}/estado")]
        public IActionResult CambiarEstado(int id, [FromBody] string estado)
        {
            var pedido = pedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            pedido.Estado = estado;

            return Ok(pedido);
        }

        // ✅ DESCARGAR ARCHIVO
        [HttpGet("{id}/archivo")]
        public IActionResult DescargarArchivo(int id)
        {
            var pedido = pedidos.FirstOrDefault(p => p.Id == id);

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
