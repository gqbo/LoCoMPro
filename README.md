# PI. Ingeniería de Software y Bases de Datos CI-0128
# Velociraptors

### Integrantes del equipo
Cristopher Hernández Calderón cristopher.hernandezcalderon@ucr.ac.cr

Gabriel González Flores gabriel.gonzalezflores@ucr.ac.cr

James Araya Rodríguez james.araya@ucr.ac.cr

Sebastián Rodríguez Tencio sebastian.rodrigueztencio@ucr.ac.cr

Yordi Robles Siles yordi.robles@ucr.ac.cr

## Docente(s):
Dra. Alexandra Martínez alexandra.martinez@ucr.ac.cr

Dr. Allan Berrocal Rojas allan.berrocal@ucr.ac.cr

## Descripción del problema

Se planea crear una aplicación web, con el fin de realizar las distintas compras que un usuario desea realizar, ahorrando costos e invirtiendo la menor cantidad de tiempo. En esta se plantea que la información sea ingresada a la aplicación por medio de crowdsourcing, lo cual significa que esta sea recopilada solo por medio de los mismos usuarios de la aplicación. Para esto los usuarios van a poder tener distintas funciones dentro de la aplicación, tales como consultor (el cual es el que utiliza la aplicación para buscar productos), generador (el cual utiliza la aplicación para subir información de productos) y moderador (un tipo de generador el cual posee más funciones).

## Estructura del repositorio

Se utiliza un sistema de carpetas en el cuál se abarcaran diferentes elementos esenciales para el desarrollo del proyecto. En el caso del diseño hay una carpeta llamada "design" la cual contendrá un diagrama del diseño de la base de datos y diferentes mockups del diseño de la aplicación. Un ejemplo de la jerarquía de carpetas corresponde a: "design/Sprint0/mockups/avance1".

### Sprint 0

En relación al Sprint 0, se crearon diferentes carpetas donde se pueden apreciar los diferentes avances presentados. En el caso del diseño conceptual de la base de datos, se aprecian dos imágenes que permiten ilustrar los cambios realizados después de las distintas retroalimentaciones. A continuación, se adjunta un enlace que permite observar el diseño que representa el último avance.

[Diseño actual de la base de datos](design/sprint0/database/avance2)

Con respecto a los mockups, los avances se representan de la misma manera que el diseño de la base de datos. En la carpeta correspondiente a los mockups, ubicada dentro de la carpeta "design," se aprecian una colección de imágenes correspondientes a un avance. A continuación, se adjunta un enlace que permite observar los diferentes diseños que representan el último avance.

[Mockups de la aplicación web](design/sprint0/mockups/avance2)

### Sprint 1

En relación al Sprint 1, se crearon diferentes carpertas donde se contempla todo lo relacionado al Sprint 1.

La carpeta principal corresponde a `source` que contiene el proyecto con el código fuente del Sprint 1.

[Proyecto de código fuente del Sprint 1](source/LoCoMPro_LV)

La carpeta para las pruebas unitarias corresponde a `tests` que contiene el proyecto con las pruebas unitarias del Sprint 1.

[Proyecto de pruebas unitarias del Sprint 1](source/tests)

La carpeta para la documentación corresponde a `doc` que contiene un archivo Doxyfile que autogenera la documentación Doxygen del código fuente para el Sprint 1.

[Documentación del Sprint 1](source/doc)

## Manual de usuario de la aplicación

Cuando se ingresa a la página principal de la aplicación web se muestra diferentes funcionalidades como buscar productos, registrarse e iniciar sesión.

1. Puedes buscar productos ingresando el nombre del producto de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/89c77c10-7688-4961-aa76-7af93e5aeb80)
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/16ed51fb-3ecf-4601-9e2a-1e830c1d7683)

2. Puedes realizar búsquedas de productos utilizando las opciones avanzadas de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/5456a2ca-b28c-46ab-8169-e6f3a521f7cf)
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/78cc9ed9-26d9-499d-8cfc-e4b392a6bf10)

3. Cuando efectúas una búsqueda de un producto, ya sea utilizando únicamente su nombre o haciendo uso de las opciones avanzadas, tienes la posibilidad de acceder a los registros asociados a dicho producto simplemente presionando el nombre del mismo.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/ceef4545-62bc-4bd8-9c42-2c9fd949d7d4)

4. Si deseas crear una cuenta en la aplicación web, puedes hacerlo presionando la opción "Registrarse" en la parte superior y completando los datos requeridos.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/139385b0-7671-4166-8050-66814387b324)

5. Si un usuario registrado desea acceder a la aplicación, puede hacerlo presionando la opción "Iniciar Sesión" en la parte superior e ingresando sus credenciales.
   
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/cd90e389-b5c8-468f-91fe-cb3f33a32fcf)

6. Una vez que un usuario inicia sesión, se habilita la funcionalidad de "Agregar Producto". Esto permite al usuario crear un registro de un producto existente o crear un producto completamente nuevo.
   
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/75f9de59-5614-467f-912f-f2729c586a2a)

7. Después de iniciar sesión, el usuario tiene la opción de cerrar su sesión en cualquier momento.

 ![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/338b4848-04b1-4a9c-a462-6633b8d3a9b1)

## Manual técnico de la aplicación

### Requerimientos de instalación o ejecución

1. Instalar Visual Studio 2022.
2. Instalar la herramienta ASP.NET and web development dentro de Visual Studio 2022.
3. Abrir la solución LoCoMPro_LV, dirigirse a la opción `Tools`, seguido de `NuGet Packet Manager` y por último `Manage NuGet Packages for Solution...`
   
   ![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/72f3b70f-393f-4d0c-8683-db187ee73ff9)

4. Instalar los siguientes paquetes.

  ![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/a82d8ba5-da72-4ae1-967f-34cb528bd5cc)

### Preparación de la base de datos

1. Abrir la consola `Package Manager`.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/f82c37de-0951-419b-8f6e-6b87bceb8f20)

2. Dentro de la consola escribir el comando `Update-Database`.

3. Verificar la creación de las tablas dentro de la base de datos especificada en el archivo `appsettings.json` en el apartado `View`, seguido de `SQL Server Object Explorer`.

### Manual de instalación o ejecución del sistema

1. Construir la solución en el apartado `Build`, seguido de `Build Solution`.
2. Selecionar el botón `Start Without Debugging`.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/7e9f5c49-544c-4dd1-9810-d2d57afc11eb)

### Manual de ejecución de los casos de prueba

1. Construir la solución en el apartado `Build`, seguido de `Build Solution`.

2. Seleccionar la opción `Run All Tests`

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/1962c469-aa0d-40c4-b705-cbc75bdb7267)

### Generar la documentación Doxygen

1. Ingresar a la página oficial de [Doxygen]([http://example.com](https://www.doxygen.nl/download.html)) y descargarlo.
2. Agregar el directorio donde se instaló Doxygen en las variables del entorno en Windows de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/142f7606-ab2a-4a23-a719-c2eb29b32524)

3. Ingresar a la carpeta `source/doc` en una terminal y ejecutar el comando `doxygen Doxyfile` de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/bd45c007-5888-42d8-aade-2cb4ec6b716b)





