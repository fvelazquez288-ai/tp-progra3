<?php

// conectando.php
$servername = "localhost";
$username = "root";
$password = "root";
$dbname = "mi_banco_db";

$conexion = mysqli_connect($servername, $username, $password, $dbname);

if (!$conexion) {
    die("Error de conexión: " . mysqli_connect_error());
}

mysqli_set_charset($conexion, "utf8mb4");
?>