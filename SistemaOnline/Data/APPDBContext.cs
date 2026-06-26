using Microsoft.EntityFrameworkCore;
using SistemaOnline.Models;

namespace SistemaOnline.Data
{
    public class APPDBContext : DbContext
    {
        public APPDBContext(DbContextOptions<APPDBContext> options) : base(options)
        {

        }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Empleado_Turno> Empleados_Turnos { get; set; }
        public DbSet<Carta> Cartas { get; set; }
        public DbSet<Producto_Categoria> Productos_Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria_Ingrediente> Categorias_Ingredientes { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Producto_Ingrediente> Productos_Ingredientes { get; set; }
        public DbSet<Mesa_Restaurante> Mesas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Pedido_Detalle> Pedidos_Detalles { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Comprobante_Pago> Comprobantes_Pagos { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
        public DbSet<Proveedor_Ingrediente> Proveedores_Ingredientes { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Promocion> Promociones { get; set; }
        public DbSet<Producto_Promocion> Productos_Promociones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.ID_Rol);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { ID_Rol = 1, Nombre_Rol = "Administrador" },
                new Rol { ID_Rol = 2, Nombre_Rol = "Cajero" },
                new Rol { ID_Rol = 3, Nombre_Rol = "Mesero" },
                new Rol { ID_Rol = 4, Nombre_Rol = "Cliente" }
                );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { ID_Usuario = 1, Nombre_Usuario = "Tigre Capo", Email = "TrigeCapo123@gmail.com", Estado = true, Password = "aQwr@2", ID_Rol = 1 },
                new Usuario { ID_Usuario = 2, Nombre_Usuario = "Diego", Email = "Diego12@gmail.com", Estado = true, Password = "123", ID_Rol = 2 },
                new Usuario { ID_Usuario = 3, Nombre_Usuario = "Valeria", Email = "Valeria15@gmail.com", Estado = true, Password = "345", ID_Rol = 3 },
                new Usuario { ID_Usuario = 4, Nombre_Usuario = "Ulises", Email = "Ulises@gmail.com", Estado = true, Password = "678", ID_Rol = 4 }
                );

            // Relación 1:1 Opcional entre Empleado y Usuario
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Usuario)
                .WithOne()
                .HasForeignKey<Empleado>(e => e.ID_Usuario)
                .IsRequired(false);

            // Contrato hacia Empleado 
            modelBuilder.Entity<Contrato>()
                .HasOne(c => c.Empleado)
                .WithMany(e => e.Contratos)
                .HasForeignKey(c => c.ID_Empleado);

            // Contrato hacia Proveedor 
            modelBuilder.Entity<Contrato>()
                .HasOne(c => c.Proveedor)
                .WithMany(p => p.Contratos)
                .HasForeignKey(c => c.ID_Proveedor);

            // Empleado_Turno (tabla intermedia M:M)
            modelBuilder.Entity<Empleado_Turno>()
                .HasKey(et => new { et.ID_Turno, et.ID_Empleado });

            modelBuilder.Entity<Empleado_Turno>()
                .HasOne(et => et.Empleado)
                .WithMany(e => e.Empleado_Turnos)
                .HasForeignKey(et => et.ID_Empleado);

            modelBuilder.Entity<Empleado_Turno>()
                .HasOne(et => et.Turno)
                .WithMany(t => t.Empleado_Turnos)
                .HasForeignKey(et => et.ID_Turno);

            // Producto_Categoria hacia Carta 
            modelBuilder.Entity<Producto_Categoria>()
                .HasOne(pc => pc.Carta)
                .WithMany(c => c.Producto_Categorias)
                .HasForeignKey(pc => pc.ID_Carta);

            // Producto hacia Producto_Categoria 
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Producto_Categoria)
                .WithMany(pc => pc.Productos)
                .HasForeignKey(p => p.ID_Categoria);

            // Ingrediente hacia Categoria_Ingrediente
            modelBuilder.Entity<Ingrediente>()
                .HasOne(i => i.Categoria_Ingrediente)
                .WithMany(ci => ci.Ingredientes)
                .HasForeignKey(i => i.ID_Cat_Ingrediente);

            // Producto_Ingrediente hacia Producto 
            modelBuilder.Entity<Producto_Ingrediente>()
                .HasOne(pi => pi.Producto)
                .WithMany(p => p.Producto_Ingredientes)
                .HasForeignKey(pi => pi.ID_Producto);

            // Producto_Ingrediente hacia Ingrediente
            modelBuilder.Entity<Producto_Ingrediente>()
                .HasOne(pi => pi.Ingrediente)
                .WithMany(i => i.Producto_Ingredientes)
                .HasForeignKey(pi => pi.ID_Ingrediente);

            // Pedido hacia Empleado 
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Empleado)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.ID_Empleado);

            // Pedido hacia Mesa_Restaurante
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Mesa_Restaurante)
                .WithMany(m => m.Pedidos)
                .HasForeignKey(p => p.ID_Mesa);

            // Pedido_Detalle hacia Pedido 
            modelBuilder.Entity<Pedido_Detalle>()
                .HasOne(pd => pd.Pedido)
                .WithMany(p => p.Pedido_Detalles)
                .HasForeignKey(pd => pd.ID_Pedido);

            // Pedido_Detalle hacia Producto 
            modelBuilder.Entity<Pedido_Detalle>()
                .HasOne(pd => pd.Producto)
                .WithMany(p => p.Pedido_Detalles)
                .HasForeignKey(pd => pd.ID_Producto);

            // Pago hacia Pedido 
            modelBuilder.Entity<Pago>()
                .HasOne(pa => pa.Pedido)
                .WithMany(p => p.Pagos)
                .HasForeignKey(pa => pa.ID_Pedido);

            // Comprobante_Pago hacia Pedido
            modelBuilder.Entity<Comprobante_Pago>()
                .HasOne(cp => cp.Pedido)
                .WithMany(p => p.Comprobantes_Pago)
                .HasForeignKey(cp => cp.ID_Pedido);

            // Reservacion hacia Cliente 
            modelBuilder.Entity<Reservacion>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservaciones)
                .HasForeignKey(r => r.ID_Cliente);

            //  Reservacion hacia Mesa_Restaurante 
            modelBuilder.Entity<Reservacion>()
                .HasOne(r => r.Mesa_Restaurante)
                .WithMany(m => m.Reservaciones)
                .HasForeignKey(r => r.ID_Mesa);

            // Proveedor_Ingrediente (tabla intermedia M:M) 
            modelBuilder.Entity<Proveedor_Ingrediente>()
                .HasKey(pi => new { pi.ID_Proveedor, pi.ID_Ingrediente });

            modelBuilder.Entity<Proveedor_Ingrediente>()
                .HasOne(pi => pi.Proveedor)
                .WithMany(p => p.Proveedor_Ingredientes)
                .HasForeignKey(pi => pi.ID_Proveedor);

            modelBuilder.Entity<Proveedor_Ingrediente>()
                .HasOne(pi => pi.Ingrediente)
                .WithMany(i => i.Proveedor_Ingredientes)
                .HasForeignKey(pi => pi.ID_Ingrediente);

            // Inventario hacia Ingrediente
            modelBuilder.Entity<Inventario>()
                .HasOne(inv => inv.Ingrediente)
                .WithMany(i => i.Inventarios)
                .HasForeignKey(inv => inv.ID_Ingrediente);

            // Producto_Promocion (tabla intermedia M:M) 
            modelBuilder.Entity<Producto_Promocion>()
                .HasKey(pp => new { pp.ID_Producto, pp.ID_Promocion });

            modelBuilder.Entity<Producto_Promocion>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.Producto_Promociones)
                .HasForeignKey(pp => pp.ID_Producto);

            modelBuilder.Entity<Producto_Promocion>()
                .HasOne(pp => pp.Promocion)
                .WithMany(pr => pr.Producto_Promociones)
                .HasForeignKey(pp => pp.ID_Promocion);

            modelBuilder.Entity<Categoria_Ingrediente>().HasData(
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 1, Nombre_Categoria = "Carnes" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 2, Nombre_Categoria = "Pescados y Mariscos" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 3, Nombre_Categoria = "Verduras y Hortalizas" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 4, Nombre_Categoria = "Frutas" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 5, Nombre_Categoria = "Lácteos" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 6, Nombre_Categoria = "Cereales y Granos" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 7, Nombre_Categoria = "Legumbres" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 8, Nombre_Categoria = "Condimentos y Especias" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 9, Nombre_Categoria = "Aceites y Grasas" },
                new Categoria_Ingrediente { ID_Cat_Ingrediente = 10, Nombre_Categoria = "Harinas y Derivados" }
                );

            modelBuilder.Entity<Carta>().HasData(
                new Carta { ID_Carta = 1, Nombre_Carta = "Carta Principal", Cantidad_Platos = 20, Descripcion = "Carta principal del restaurante Rancho Sagrado", Precio = 0m }
                );

            modelBuilder.Entity<Producto_Categoria>().HasData(
                new Producto_Categoria { ID_Categoria = 1, Nombre_Categoria = "Platos Principales", Descripcion = "Platos de fondo del restaurante", ID_Carta = 1 },
                new Producto_Categoria { ID_Categoria = 2, Nombre_Categoria = "Bebidas", Descripcion = "Bebidas frías y calientes", ID_Carta = 1 },
                new Producto_Categoria { ID_Categoria = 3, Nombre_Categoria = "Postres", Descripcion = "Postres y dulces para finalizar la comida", ID_Carta = 1 }
                );

            modelBuilder.Entity<Producto>().HasData(
                new Producto { ID_Producto = 1, Nombre_Plato = "Lomo Saltado", Descripcion = "Tiras de res salteadas con cebolla, tomate y papas fritas", Tiempo_Preparacion = 20, Precio = 28.50m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 2, Nombre_Plato = "Estofado de Res", Descripcion = "Carne de res cocida a fuego lento con vegetales", Tiempo_Preparacion = 45, Precio = 26.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 3, Nombre_Plato = "Ceviche", Descripcion = "Pescado fresco marinado en limón con cebolla y ají", Tiempo_Preparacion = 15, Precio = 32.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 4, Nombre_Plato = "Arroz con Mariscos", Descripcion = "Arroz al estilo criollo con mezcla de mariscos", Tiempo_Preparacion = 30, Precio = 34.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 5, Nombre_Plato = "Ensalada César", Descripcion = "Lechuga, pollo, crutones y aderezo César", Tiempo_Preparacion = 10, Precio = 18.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 6, Nombre_Plato = "Ratatouille", Descripcion = "Guiso de vegetales al horno estilo francés", Tiempo_Preparacion = 35, Precio = 22.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 7, Nombre_Plato = "Ensalada de Frutas", Descripcion = "Mezcla de frutas frescas de temporada", Tiempo_Preparacion = 10, Precio = 14.00m, Disponibilidad = true, Categoria = "Postres", ID_Categoria = 3 },
                new Producto { ID_Producto = 8, Nombre_Plato = "Pie de Manzana", Descripcion = "Tarta de manzana horneada con canela", Tiempo_Preparacion = 25, Precio = 16.00m, Disponibilidad = true, Categoria = "Postres", ID_Categoria = 3 },
                new Producto { ID_Producto = 9, Nombre_Plato = "Lasaña de Queso", Descripcion = "Capas de pasta horneada con queso y bechamel", Tiempo_Preparacion = 40, Precio = 27.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 10, Nombre_Plato = "Fondue de Queso", Descripcion = "Queso fundido para compartir con pan y vegetales", Tiempo_Preparacion = 20, Precio = 30.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 11, Nombre_Plato = "Arroz Chaufa", Descripcion = "Arroz salteado estilo chifa con vegetales y proteína", Tiempo_Preparacion = 20, Precio = 24.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 12, Nombre_Plato = "Risotto", Descripcion = "Arroz cremoso italiano con queso parmesano", Tiempo_Preparacion = 30, Precio = 26.50m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 13, Nombre_Plato = "Lentejas Guisadas", Descripcion = "Lentejas cocidas a fuego lento con vegetales", Tiempo_Preparacion = 35, Precio = 19.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 14, Nombre_Plato = "Garbanzos con Espinaca", Descripcion = "Garbanzos guisados con espinaca fresca", Tiempo_Preparacion = 30, Precio = 20.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 15, Nombre_Plato = "Pollo al Curry", Descripcion = "Pollo guisado en salsa de curry y especias", Tiempo_Preparacion = 30, Precio = 25.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 16, Nombre_Plato = "Ají de Gallina", Descripcion = "Pollo deshilachado en crema de ají amarillo", Tiempo_Preparacion = 35, Precio = 27.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 17, Nombre_Plato = "Papas Fritas", Descripcion = "Papas fritas crocantes servidas como acompañamiento", Tiempo_Preparacion = 12, Precio = 12.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 18, Nombre_Plato = "Mayonesa Casera", Descripcion = "Salsa mayonesa preparada artesanalmente", Tiempo_Preparacion = 10, Precio = 6.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 19, Nombre_Plato = "Pizza", Descripcion = "Pizza horneada con queso y toppings variados", Tiempo_Preparacion = 25, Precio = 29.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 },
                new Producto { ID_Producto = 20, Nombre_Plato = "Empanadas", Descripcion = "Empanadas horneadas rellenas de carne y vegetales", Tiempo_Preparacion = 20, Precio = 15.00m, Disponibilidad = true, Categoria = "Platos Principales", ID_Categoria = 1 }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}