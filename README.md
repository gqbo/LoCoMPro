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

## Tech Stack
gqbo/LoCoMPro está construido sobre el siguiente tech stack:

- <img width='25' height='25' src='https://img.stackshare.io/service/1015/1200px-C_Sharp_wordmark.svg.png' alt='C#'/> [C#](http://csharp.net) – Languages
- <img width='25' height='25' src='https://img.stackshare.io/service/1209/javascript.jpeg' alt='JavaScript'/> [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript) – Languages
- <img width='25' height='25' src='https://img.stackshare.io/service/1014/IoPy1dce_400x400.png' alt='.NET'/> [.NET](http://www.microsoft.com/net/) – Frameworks (Full Stack)
- <img width='25' height='25' src='https://img.stackshare.io/service/6403/default_91fc1f0ee315262794273aa1387eaf8fed8436e6.png' alt='.NET Core'/> [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/) – Frameworks (Full Stack)
- <img width='25' height='25' src='https://img.stackshare.io/service/11331/asp.net-core.png' alt='ASP.NET Core'/> [ASP.NET Core](docs.microsoft.com/en-us/aspnet/core/) – Frameworks (Full Stack)
- <img width='25' height='25' src='https://img.stackshare.io/service/3251/no-img-open-source.png' alt='Entity Framework'/> [Entity Framework](https://docs.microsoft.com/en-us/aspnet/entity-framework) – Object Relational Mapper (ORM)
- <img width='25' height='25' src='https://img.stackshare.io/service/10254/no-img-open-source.png' alt='Entity Framework Core'/> [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) – Object Relational Mapper (ORM)
- <img width='25' height='25' src='https://img.stackshare.io/service/1027/sql_server.png' alt='Microsoft SQL Server'/> [Microsoft SQL Server](http://microsoft.com/sqlserver) – Databases
- <img width='25' height='25' src='https://img.stackshare.io/service/1071/sqlite.jpg' alt='SQLite'/> [SQLite](http://www.sqlite.org/) – Databases
- <img width='25' height='25' src='https://img.stackshare.io/service/1628/1434934.png' alt='Moq'/> [Moq](https://github.com/Moq/moq4) – Testing Frameworks
- <img width='25' height='25' src='https://img.stackshare.io/service/2371/jZ6MYx5Y_400x400.png' alt='NUnit'/> [NUnit](http://www.nunit.org/) – Testing Frameworks
- <img width='25' height='25' src='https://img.stackshare.io/service/1517/sbUizSli_400x400.jpg' alt='Selenium'/> [Selenium](http://www.seleniumhq.org/) – Browser Testing
- <img width='25' height='25' src='https://img.stackshare.io/service/1451/SR2hUhQN.png' alt='Visual Studio'/> [Visual Studio](http://msdn.microsoft.com/en-us/vstudio/aa718325.aspx) – Integrated Development Environment

Full tech stack [aquí](/techstack.md)

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

En relación al Sprint 1, se crearon diferentes carpetas donde se contempla todo lo relacionado al Sprint 1.

La carpeta principal corresponde a `source` que contiene el proyecto con el código fuente del Sprint 1.

[Proyecto de código fuente del Sprint 1](source/LoCoMPro_LV)

La carpeta para las pruebas unitarias corresponde a `tests` que contiene el proyecto con las pruebas unitarias del Sprint 1.

[Proyecto de pruebas unitarias del Sprint 1](source/tests)

La carpeta para la documentación corresponde a `doc` que contiene un archivo Doxyfile que autogenera la documentación Doxygen del código fuente para el Sprint 1.

[Documentación del Sprint 1](source/doc)

### Sprint 2

Con respecto al Sprint 2, se crearon diferentes carpetas donde se contempla todo lo relacionado al Sprint 2.

La carpeta principal corresponde a `source` que contiene el proyecto con el código fuente del Sprint 2.

[Proyecto de código fuente del Sprint 2](source/LoCoMPro_LV)

La carpeta para las pruebas unitarias corresponde a `tests/unit_tests` que contiene el proyecto con las pruebas unitarias del Sprint 2.

[Proyecto de pruebas unitarias del Sprint 2](source/tests/unit_tests)

La carpeta para las pruebas funcionales corresponde a `tests/functional_tests` que contiene el proyecto con las pruebas funcionales realizadas durante el Sprint 2.

[Proyecto de pruebas unitarias del Sprint 2](source/tests/functional_tests)

La carpeta para la documentación corresponde a `doc` que contiene un archivo Doxyfile que autogenera la documentación Doxygen del código fuente para el Sprint 2.

[Documentación del Sprint 2](source/doc)

### Sprint 3

Con respecto al Sprint 3, se crearon diferentes carpetas donde se contempla todo lo relacionado al Sprint 3.

La carpeta principal corresponde a `source` que contiene el proyecto con el código fuente del Sprint 3.

[Proyecto de código fuente del Sprint 3](source/LoCoMPro_LV)

La carpeta para las pruebas unitarias corresponde a `tests/unit_tests` que contiene el proyecto con las pruebas unitarias del Sprint 3.

[Proyecto de pruebas unitarias del Sprint 3](source/tests/unit_tests)

La carpeta para las pruebas funcionales corresponde a `tests/functional_tests` que contiene el proyecto con las pruebas funcionales realizadas durante el Sprint 3.

[Proyecto de pruebas unitarias del Sprint 3](source/tests/functional_tests)

La carpeta para la documentación corresponde a `doc` que contiene un archivo Doxyfile que autogenera la documentación Doxygen del código fuente para el Sprint 3.

[Documentación del Sprint 3](source/doc)

## Manual de usuario de la aplicación

Cuando se ingresa a la página principal de la aplicación web se muestran diferentes funcionalidades como buscar productos, registrarse e iniciar sesión.

#### 1. Puedes buscar productos ingresando el nombre del producto de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/89c77c10-7688-4961-aa76-7af93e5aeb80)

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/829b1353-42d3-4414-9c84-17ac8518dcb5)


#### 2. Puedes realizar búsquedas de productos utilizando las opciones avanzadas de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/5456a2ca-b28c-46ab-8169-e6f3a521f7cf)

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/16e74055-2046-40b1-896e-1f37e63cf3b7)

#### 3. Cuando se encuentra en la vista del resultados de las búsquedas, es posible filtrar los resultados por ubicación o por establecimiento de la siguiente manera:

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/b68f6c0f-e3eb-4c87-8c0e-74bea2dd87ad)


#### 4. Cuando efectúas una búsqueda de un producto, ya sea utilizando únicamente su nombre o haciendo uso de las opciones avanzadas, tienes la posibilidad de acceder a los registros asociados a dicho producto simplemente presionando el nombre del mismo.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/03f941c9-4d83-4940-aac3-5dd14550fb23)

Como se observa en la imagen en esta vista se pueden realizar valoraciones de un registro a la hora de presionar las estrellas. A la vez es posible agregar el producto a una lista de interés presionando el botón `Agregar a la lista`. Por otro lado, se pueden realizar reportes a la hora de presionar el rectangulo rojo.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/c1afce84-f4c6-4e43-8137-b8b6c3458113)

#### 4. Si deseas crear una cuenta en la aplicación web, puedes hacerlo presionando la opción "Registrarse" en la parte superior y completando los datos requeridos.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/94494689/a4de7391-7414-46f1-a36f-a3c9c46152a4)

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/94494689/c773b1b1-3cdb-4b43-bfe9-2adbdbd26157)

#### 5. Si un usuario registrado desea acceder a la aplicación, puede hacerlo presionando la opción "Iniciar Sesión" en la parte superior e ingresando sus credenciales.
   
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/cd90e389-b5c8-468f-91fe-cb3f33a32fcf)

#### 6. Una vez que un usuario inicia sesión, se habilita la funcionalidad de "Agregar Producto". Esto permite al usuario crear un registro de un producto existente o crear un producto completamente nuevo.
   
![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/8ec3819f-421f-49f8-892a-9f5c0989cf88)

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/65a4cbba-d1ca-48ee-9e50-f1fdcbba6c15)

#### 7. Después de iniciar sesión, el usuario también tiene la opción de Mi Perfil en el que se dan a elegir 4 opciones distintas.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/d373aee1-fe96-46e4-a522-9533649c42e3)

La opción 1 - Mis Aportes: Muestra todos los registros agregados por el usuario registrado.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/9d253469-2823-496e-9195-45d733599d99)

La opción 2 - Mi lista de interés: Permite al usuario visualizar los productos de su lista de interés y realizar búsquedas en lote.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/ba038bb1-3582-4fc4-bd90-9b01c6f0ecc1)

La opción 3 - Gestionar Cuenta: Permite al usuario personalizar y controlar diversos aspectos de su perfil.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/94494689/3edaac78-7e9b-495f-997c-d1aaf584f856)

La opción 4 - Cerrar Sesión: Permite a un usuario finalizar la sesión activa en una plataforma.

#### 8. Un usuario moderador tiene habilitada la opción Moderar, donde se comprenden 4 funcionalidades de moderación:

La opción 1 - Reportes: Muestra todos reportes relacionados a los registros creados por los usuarios.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/28fc547e-8aa9-48be-b6a6-4069e303db9f)

Al presionar un reporte se muestra una pantalla con información del registro y usuario reportado. Además en la parte inferior se muestra el usuario que reporta y la razón del reporte con dos botones el de aceptar que indica que la razón del reporte es válida y el de recahazar que indicaría lo contrario.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/68618348-8656-4480-ac10-f10d00947ac5)


La opción 2 - Top Reportados: Permite al moderador visualizar a los usuarios que más han sido reportados en sus registros.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/bc38afbc-e63f-4b2c-a0bf-d43241ad7a3e)

La opción 3 - Top Reportadores: Permite al moderador visualizar a los usuarios que más reportes realizan.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/29df06b0-a0a2-451d-9495-8041ea1f02f7)

La opción 4 - Anomalías: Permite al moderador visualizar las anomalías que se encuentren entre los registros, ya sea en la fecha de registro o bien en los precios.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/3b98697f-4f23-4dc6-83a7-be73f1822478)

Al presionar el botón de `Buscar Anomalías en Fecha` se muestran los registros que posean anomalías en la fecha.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/68b4b673-4ff7-4df0-bf9f-67142e116a21)

Si se presiona el título del registro, el moderador puede decidir si acepta o rechaza la anomalía.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/2d54916d-63fb-4ea4-b4f5-56f89e0b7f84)

Al presionar el botón de `Buscar Anomalías en Precio` se muestran los registros que posean anomalías en el precio.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/3219d4fe-a496-4f20-8b0c-a6e664665be7)

Si se presiona el título del registro, el moderador puede decidir si acepta o rechaza la anomalía. 

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/102630255/bf1be3be-5c2a-4d41-9f39-c0dda76c7161)

## Manual técnico de la aplicación

### Requerimientos de instalación o ejecución

1. Instalar Visual Studio 2022.
2. Instalar la herramienta ASP.NET and web development dentro de Visual Studio 2022.
3. Abrir la solución LoCoMPro_LV, dirigirse a la opción `Tools`, seguido de `NuGet Packet Manager` y por último `Manage NuGet Packages for Solution...`
   
   ![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/72f3b70f-393f-4d0c-8683-db187ee73ff9)

4. Instalar los siguientes paquetes.

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/69997150/a82d8ba5-da72-4ae1-967f-34cb528bd5cc)

![image](https://github.com/gqbo/ci0128_23b_velociraptors/assets/94494689/7ca3757d-2a89-4c29-a032-ebc353832f8f)


### Manual de instalación o ejecución del sistema

1. Construir la solución en el apartado `Build`, seguido de `Build Solution`.
2. Seleccionar el botón `Start Without Debugging`.

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





