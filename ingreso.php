<?php

// ingreso.php
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $tipo_documento = $_POST['tipo_doc'];
    $documento = $_POST['documento'];
    $usuario = $_POST['usuario'];
    $clave_usuario = $_POST['clave'];

    include('conectando.php');

    $sql_buscar_usuario = "SELECT documento, nombre, apellido FROM usuarios 
                           WHERE tipo_doc = '$tipo_documento' 
                           AND documento = '$documento' 
                           AND usuario = '$usuario' 
                           AND `password` = '$clave_usuario'";
    
    $resultado_buscar_usuario = mysqli_query($conexion, $sql_buscar_usuario);

    if ($usuario_encontrado = mysqli_fetch_assoc($resultado_buscar_usuario)) {
        session_start();
        $_SESSION['documento'] = $usuario_encontrado['documento'];
        $_SESSION['nombre'] = $usuario_encontrado['nombre'];
        $_SESSION['apellido'] = $usuario_encontrado['apellido'];

        mysqli_close($conexion);
        echo '<meta http-equiv="refresh" content="0;url=resumen.php">';
        exit();
    } else {
        mysqli_close($conexion);
        die("Error: Credenciales inválidas. <a href='ingreso.html'>Volver</a>");
    }
} else {
    echo '<meta http-equiv="refresh" content="0;url=ingreso.html">';
    exit();
}
?>