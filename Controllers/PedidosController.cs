using Microsoft.AspNetCore.Mvc;
using WebRecreo.Models;

namespace WebRecreo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private static List<Pedido> pedidos = new List<Pedido>();

        // ✅ CREAR PEDIDO
        [HttpPost]
        public IActionResult Post([FromForm] Pedido nuevo)
        {
            nuevo.Id = pedidos.Count + 1;
            nuevo.Fecha = DateTime.Now;

            if (string.IsNullOrEmpty(nuevo.Estado))
                nuevo.Estado = "Pendiente";

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
