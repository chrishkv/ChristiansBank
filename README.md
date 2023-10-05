# ChristiansBank
Los proyectos se dividen en 2 microservicios existen en las carpeta Cliente y Cuenta_Movimiento

Se debe entrar en la carpeta "Cliente"
-cd Cliente

Luego ejecutar el comando docker compose
-docker compose up -d

Esto creara los contenedores del proyecto, de la base de datos y ejecuta la migracion de la base de datos

La url para hacer las peticiones para el proyecto de "Cliente" es: 
-"http://localhost:80/Clientes"
-"http://localhost:80/Personas"

Se debe entrar en la carpeta "Cuenta_Movimiento"
-cd Cuenta_Movimiento

Luego ejecutar el comando docker compose
-docker compose up -d

Esto creara los contenedores del proyecto, de la base de datos y ejecuta la migracion de la base de datos

La url para hacer las peticiones para el proyecto de "Cuenta_Movimiento" es: 
-"http://localhost:81/Cuentas"
-"http://localhost:81/Movimientos"
-"http://localhost:81/Reportes"

La direccion de github del proyecto es:
https://github.com/chrishkv/ChristiansBank