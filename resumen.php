<?php

header('Content-Type: text/plain; charset=utf-8');
session_start();

if (!isset($_SESSION['documento'])) {
    echo "ERROR: Acceso denegado. Usuario no autenticado.";
    exit();
}

include('conectando.php');

$documento_usuario = $_SESSION['documento'];
$nombre_usuario    = $_SESSION['nombre'];
$apellido_usuario  = $_SESSION['apellido'];

echo "==================================================\n";
echo "CLIENTE - TARJETA\n";
echo "==================================================\n";
echo "Bienvenido/a: " . $nombre_usuario . " " . $apellido_usuario . "\n";
echo "Documento: " . $documento_usuario . "\n\n";

$sql_buscar_tarjeta = "SELECT num_cuenta, numero_tarjeta, banco_emisor, saldo, estado 
                       FROM tarjetas 
                       WHERE dni_titular = '$documento_usuario'";

$resultado_buscar_tarjeta = mysqli_query($conexion, $sql_buscar_tarjeta);
$datos_tarjeta_usuario    = mysqli_fetch_assoc($resultado_buscar_tarjeta);

if ($datos_tarjeta_usuario) {
    echo "=== DATOS DE LA TARJETA ===\n";
    echo "Banco Emisor: "      . $datos_tarjeta_usuario['banco_emisor']    . "\n";
    echo "Número de Cuenta: "  . $datos_tarjeta_usuario['num_cuenta']      . "\n";
    echo "Número de Tarjeta: " . $datos_tarjeta_usuario['numero_tarjeta']  . "\n";
    echo "Estado: "            . $datos_tarjeta_usuario['estado']           . "\n";
    echo "Saldo Utilizado: $"  . $datos_tarjeta_usuario['saldo']           . "\n\n";

    $numero_cuenta_tarjeta = $datos_tarjeta_usuario['num_cuenta'];

    $sql_buscar_liquidaciones = "SELECT id_liquidacion, periodo, fecha_vencimiento, total_a_pagar, pago_minimo
                                 FROM liquidaciones 
                                 WHERE num_cuenta = '$numero_cuenta_tarjeta' 
                                 ORDER BY periodo DESC";

    $resultado_buscar_liquidaciones = mysqli_query($conexion, $sql_buscar_liquidaciones);

    $lista_liquidaciones_encontradas = [];
    while ($fila_liquidacion = mysqli_fetch_assoc($resultado_buscar_liquidaciones)) {
        $lista_liquidaciones_encontradas[] = $fila_liquidacion;
    }

    echo "=== LIQUIDACIÓN ACTUAL ===\n";
    if (!empty($lista_liquidaciones_encontradas)) {
        $ultima = $lista_liquidaciones_encontradas[0];
        echo "Período: "           . $ultima['periodo']          . "\n";
        echo "Vencimiento: "       . $ultima['fecha_vencimiento']. "\n";
        echo "Total a Pagar: $"    . $ultima['total_a_pagar']    . "\n";
        echo "Pago Mínimo: $"      . $ultima['pago_minimo']      . "\n\n";
    } else {
        echo "No hay liquidaciones emitidas para esa tarjeta.\n\n";
    }

    echo "=== HISTORIAL DE RESÚMENES ANTERIORES ===\n";
    if (count($lista_liquidaciones_encontradas) > 1) {
        for ($i = 1; $i < count($lista_liquidaciones_encontradas); $i++) {
            $resumen_historico = $lista_liquidaciones_encontradas[$i];
            echo "ID: "       . $resumen_historico['id_liquidacion']   . " | ";
            echo "Período: "  . $resumen_historico['periodo']          . " | ";
            echo "Monto: $"   . $resumen_historico['total_a_pagar']    . " | ";
            echo "Vence: "    . $resumen_historico['fecha_vencimiento']. "\n";
        }
    } else {
        echo "No se registran períodos anteriores en el historial.\n";
    }
} else {
    echo "ERROR: No se encontró ninguna tarjeta de crédito asociada.\n";
}

mysqli_close($conexion);

?>
