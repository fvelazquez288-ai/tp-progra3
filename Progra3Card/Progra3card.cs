using System;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=root;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta();      break;
                    case "2": MenuListarTarjetas();     break;
                    case "3": MenuVerDetalleTarjeta();  break;
                    case "4": MenuEliminarTarjeta();    break;
                    case "5": MenuEmitirLiquidacion();  break;
                    case "6": salir = true;             break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MenuEmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA TARJETA (ALTA DE CLIENTE) ---");

            Console.Write("Ingrese el Número de Documento: ");
            string documentoCliente = Console.ReadLine();

            Console.WriteLine("Seleccione Tipo de Documento:");
            Console.WriteLine("1. DNI");
            Console.WriteLine("2. PASAPORTE");
            Console.Write("Opción: ");
            string opcionTipoDocumento = Console.ReadLine();
            string tipoDocumentoBaseDatos = (opcionTipoDocumento == "2") ? "PASAPORTE" : "DNI";

            Console.Write("Ingrese el Nombre: ");
            string nombreCliente = Console.ReadLine();

            Console.Write("Ingrese el Apellido: ");
            string apellidoCliente = Console.ReadLine();

            Console.Write("Ingrese la Fecha de Nacimiento (AAAA-MM-DD): ");
            string fechaNacimientoCliente = Console.ReadLine();

            Console.Write("Ingrese el Correo Electrónico: ");
            string emailCliente = Console.ReadLine();

            Console.Write("Ingrese el Número de Tarjeta (16 dígitos): ");
            string numeroTarjetaCliente = Console.ReadLine();

            Console.WriteLine("Seleccione el Banco Emisor:");
            Console.WriteLine("1. Banco Nación");
            Console.WriteLine("2. Banco Provincia");
            Console.WriteLine("3. Banco Galicia");
            Console.WriteLine("4. Banco Santander");
            Console.WriteLine("5. Banco BBVA");
            Console.WriteLine("6. Banco Macro");
            Console.Write("Seleccione una opción numérica: ");
            string opcionBanco = Console.ReadLine();

            string bancoEmisorSeleccionado = "Banco Nación";
            switch (opcionBanco)
            {
                case "2": bancoEmisorSeleccionado = "Banco Provincia"; break;
                case "3": bancoEmisorSeleccionado = "Banco Galicia";   break;
                case "4": bancoEmisorSeleccionado = "Banco Santander"; break;
                case "5": bancoEmisorSeleccionado = "Banco BBVA";      break;
                case "6": bancoEmisorSeleccionado = "Banco Macro";     break;
            }

            using (MySqlConnection conexionBaseDatos = new MySqlConnection(connectionString))
            {
                try
                {
                    conexionBaseDatos.Open();

                    string sqlInsertarUsuario = "INSERT INTO usuarios (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email, usuario, password) " +
                                                "VALUES ('" + documentoCliente + "', '" + tipoDocumentoBaseDatos + "', '" + nombreCliente + "', '" + apellidoCliente + "', '" + fechaNacimientoCliente + "', '" + emailCliente + "', NULL, NULL)";
                    MySqlCommand comandoInsertarUsuario = new MySqlCommand(sqlInsertarUsuario, conexionBaseDatos);
                    comandoInsertarUsuario.ExecuteNonQuery();

                    string sqlInsertarTarjeta = "INSERT INTO tarjetas (numero_tarjeta, banco_emisor, estado, saldo, dni_titular) " +
                                                "VALUES ('" + numeroTarjetaCliente + "', '" + bancoEmisorSeleccionado + "', 'Activa', 0.00, '" + documentoCliente + "')";
                    MySqlCommand comandoInsertarTarjeta = new MySqlCommand(sqlInsertarTarjeta, conexionBaseDatos);
                    comandoInsertarTarjeta.ExecuteNonQuery();

                    Console.WriteLine("\n[ÉXITO] Alta comercial completada.");
                }
                catch (Exception excepcionOcurrida)
                {
                    Console.WriteLine("\n[ERROR] No se pudo procesar la emisión: " + excepcionOcurrida.Message);
                }
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEmitirLiquidacion()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA LIQUIDACIÓN MENSUAL ---");

            Console.Write("Ingrese el Número de Cuenta de la tarjeta: ");
            string numeroCuentaTarjeta = Console.ReadLine();

            // periodo en formato YYYY-MM según la BD
            Console.Write("Ingrese el Año del período (AAAA): ");
            string añoPeriodo = Console.ReadLine();
            Console.Write("Ingrese el Mes del período (MM, con cero si corresponde): ");
            string mesPeriodo = Console.ReadLine();
            string periodoCombinado = añoPeriodo + "-" + mesPeriodo;

            Console.Write("Ingrese el Total a Pagar de la liquidación: ");
            string totalAPagar = Console.ReadLine();

            Console.Write("Ingrese el Pago Mínimo: ");
            string pagoMinimo = Console.ReadLine();

            Console.Write("Ingrese la Fecha de Vencimiento (AAAA-MM-DD): ");
            string fechaVencimientoLiquidacion = Console.ReadLine();

            using (MySqlConnection conexionBaseDatos = new MySqlConnection(connectionString))
            {
                try
                {
                    conexionBaseDatos.Open();

                    string sqlInsertarLiquidacion = "INSERT INTO liquidaciones (num_cuenta, periodo, fecha_vencimiento, total_a_pagar, pago_minimo) " +
                                                    "VALUES (" + numeroCuentaTarjeta + ", '" + periodoCombinado + "', '" + fechaVencimientoLiquidacion + "', " + totalAPagar + ", " + pagoMinimo + ")";
                    MySqlCommand comandoInsertarLiquidacion = new MySqlCommand(sqlInsertarLiquidacion, conexionBaseDatos);
                    comandoInsertarLiquidacion.ExecuteNonQuery();

                    // Actualizar saldo acumulado en la tarjeta
                    string sqlActualizarSaldo = "UPDATE tarjetas SET saldo = saldo + " + totalAPagar + " WHERE num_cuenta = " + numeroCuentaTarjeta;
                    MySqlCommand comandoActualizarSaldo = new MySqlCommand(sqlActualizarSaldo, conexionBaseDatos);
                    comandoActualizarSaldo.ExecuteNonQuery();

                    Console.WriteLine("\n[ÉXITO] Liquidación registrada correctamente. Impacto inmediato en la web.");
                }
                catch (Exception excepcionOcurrida)
                {
                    Console.WriteLine("\n[ERROR] No se pudo registrar la liquidación: " + excepcionOcurrida.Message);
                }
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");
            ObtenerYMostrarTarjetas();
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());
            MostrarDetalleCompleto(numCuenta);
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta y sus datos vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");

            if (Console.ReadLine().ToUpper() == "S")
            {
                bool exito = DarDeBajaTarjeta(numCuenta);
                if (exito) Console.WriteLine("\nTarjeta eliminada correctamente.");
                else       Console.WriteLine("\nError. Verifique el número de cuenta.");
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void ObtenerYMostrarTarjetas()
        {
            using (MySqlConnection conexionBaseDatos = new MySqlConnection(connectionString))
            {
                try
                {
                    conexionBaseDatos.Open();
                    string sqlSeleccionarTarjetas = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas";
                    MySqlCommand comandoSeleccionar = new MySqlCommand(sqlSeleccionarTarjetas, conexionBaseDatos);

                    using (MySqlDataReader lectorRegistros = comandoSeleccionar.ExecuteReader())
                    {
                        while (lectorRegistros.Read())
                        {
                            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}",
                                lectorRegistros["num_cuenta"], lectorRegistros["numero_tarjeta"],
                                lectorRegistros["banco_emisor"], lectorRegistros["dni_titular"]);
                        }
                    }
                }
                catch (Exception excepcionOcurrida)
                {
                    Console.WriteLine("Error al listar tarjetas: " + excepcionOcurrida.Message);
                }
            }
        }

        static void MostrarDetalleCompleto(int cuenta)
        {
            using (MySqlConnection conexionBaseDatos = new MySqlConnection(connectionString))
            {
                try
                {
                    conexionBaseDatos.Open();
                    string sqlDetalleJoin = "SELECT tarjetas.num_cuenta, tarjetas.numero_tarjeta, tarjetas.banco_emisor, tarjetas.saldo, tarjetas.estado, " +
                                            "usuarios.nombre, usuarios.apellido, usuarios.email, usuarios.tipo_doc " +
                                            "FROM tarjetas " +
                                            "INNER JOIN usuarios ON tarjetas.dni_titular = usuarios.documento " +
                                            "WHERE tarjetas.num_cuenta = " + cuenta;

                    MySqlCommand comandoDetalle = new MySqlCommand(sqlDetalleJoin, conexionBaseDatos);

                    using (MySqlDataReader lectorDetalle = comandoDetalle.ExecuteReader())
                    {
                        if (lectorDetalle.Read())
                        {
                            Console.WriteLine("\n> INFORMACIÓN DEL PLÁSTICO");
                            Console.WriteLine("Número de Cuenta: "  + lectorDetalle["num_cuenta"]);
                            Console.WriteLine("Número de Tarjeta: " + lectorDetalle["numero_tarjeta"]);
                            Console.WriteLine("Banco Emisor: "      + lectorDetalle["banco_emisor"]);
                            Console.WriteLine("Estado: "            + lectorDetalle["estado"]);
                            Console.WriteLine("Saldo Utilizado: $"  + lectorDetalle["saldo"]);
                            Console.WriteLine("\n> INFORMACIÓN DEL TITULAR");
                            Console.WriteLine("Nombre: "   + lectorDetalle["apellido"] + ", " + lectorDetalle["nombre"]);
                            Console.WriteLine("Tipo Doc: " + lectorDetalle["tipo_doc"]);
                            Console.WriteLine("Email: "    + lectorDetalle["email"]);
                        }
                        else
                        {
                            Console.WriteLine("\nNo se encontró el número de cuenta " + cuenta);
                        }
                    }
                }
                catch (Exception excepcionOcurrida)
                {
                    Console.WriteLine("Error al consultar el detalle: " + excepcionOcurrida.Message);
                }
            }
        }

        static bool DarDeBajaTarjeta(int cuenta)
        {
            using (MySqlConnection conexionBaseDatos = new MySqlConnection(connectionString))
            {
                try
                {
                    conexionBaseDatos.Open();
                    string sqlEliminarTarjeta = "DELETE FROM tarjetas WHERE num_cuenta = " + cuenta;
                    MySqlCommand comandoEliminar = new MySqlCommand(sqlEliminarTarjeta, conexionBaseDatos);
                    int filasAfectadas = comandoEliminar.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}

