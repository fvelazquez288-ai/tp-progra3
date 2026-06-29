<?php
// altas.php
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $tipo_doc  = $_POST['tipo_doc'];
    $documento = $_POST['documento'];
    $usuario   = $_POST['usuario'];
    $passwordA = $_POST['passwordA'];
    $passwordB = $_POST['passwordB'];

    if ($passwordA !== $passwordB) {
        die("Error: Las contraseñas ingresadas no coinciden. <a href='registro.html'>Volver</a>");
    }

    if ($tipo_doc !== 'DNI' && $tipo_doc !== 'PASAPORTE') {
        die("Error: Tipo de documento no válido. <a href='registro.html'>Volver</a>");
    }

    include('conectando.php');

    // 1. Verificar que el cliente ya existe en el sistema (cargado por el admin vía C#)
    //    y que aún no activó su cuenta web (usuario IS NULL)
    $sql_verificar = "SELECT u.documento 
                      FROM usuarios u
                      INNER JOIN tarjetas t ON t.dni_titular = u.documento
                      WHERE u.documento  = '$documento'
                        AND u.tipo_doc   = '$tipo_doc'
                        AND u.usuario    IS NULL";

    $resultado_verificar = mysqli_query($conexion, $sql_verificar);

    if (mysqli_num_rows($resultado_verificar) === 0) {
        mysqli_close($conexion);
        die("Error: No se encontró una tarjeta pendiente de activación para ese documento, o la cuenta ya fue activada. <a href='registro.html'>Volver</a>");
    }

    // 2. Verificar que el nombre de usuario elegido no esté tomado
    $sql_usuario_libre = "SELECT documento FROM usuarios WHERE usuario = '$usuario'";
    $resultado_usuario_libre = mysqli_query($conexion, $sql_usuario_libre);

    if (mysqli_num_rows($resultado_usuario_libre) > 0) {
        mysqli_close($conexion);
        die("Error: El nombre de usuario elegido ya está en uso. <a href='registro.html'>Volver</a>");
    }

    // 3. Activar la cuenta: actualizar usuario y contraseña
    $sql_activar = "UPDATE usuarios 
                    SET usuario = '$usuario', password = '$passwordA' 
                    WHERE documento = '$documento'";

    if (mysqli_query($conexion, $sql_activar)) {
        mysqli_close($conexion);
        echo '<meta http-equiv="refresh" content="0;url=ingreso.html">';
        exit();
    } else {
        mysqli_close($conexion);
        die("Error al activar la cuenta: " . mysqli_error($conexion));
    }

} else {
    echo '<meta http-equiv="refresh" content="0;url=registro.html">';
    exit();
}
?>
