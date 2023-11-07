# Base de datos - Avance 1

## Cambios en el ER

1. Se agregó un campo “Apellido” como un nuevo atributo de la entidad “Usuario”, ya que se decidió que un usuario ingresará solo un apellido a la hora de registrarse dentro de la aplicación web
2. Se eliminó el atributo “Información de contacto” compuesto por “Email” y “Teléfono”, ya que no se utilizarán en los atributos de un registro, solo el nombre de un establecimiento.

## Análisis de comparación

Debido a que en el desarrollo del sprint 1 del proyecto integrador estamos utilizando el enfoque Code First, se presentan diferentes aspectos que se pueden apreciar a la hora de implementar la base de datos, esto es debido a que dicho enfoque propone crear la base de datos a partir de los modelos de datos y su definición del contexto.

Dentro de cada modelo de datos, se pueden definir los diferentes atributos que corresponden a una tabla del modelo relacional. Para definir la llave primaria que corresponde a los modelos de datos se puede realizar de dos formas distintas, la primera corresponde a adicionarle al atributo que se desea definir como llave primaria las letras “ID”, lo que se ejemplifica como “NombreProductoID”. La otra alternativa corresponde a adicionar la anotación “[KEY]”, de esta forma se define la llave primaria de los diferentes modelos de datos.

En el enfoque Code First, es necesario definir los atributos de navegación, estos atributos representan las relaciones que existen entre los modelos, por medio de una llave foránea. El modelo que posee la llave foránea como llave primaria, debe tener una colección de instancias de los modelos que adquieren las llaves foráneas, mientras que estos últimos solo deben tener una instancia del modelo que posee la llave foránea como llave. 

Un aspecto relevante que posee el enfoque Code First, es que este por defecto establece en las restricciones de la llave foránea las sentencias de ON DELETE CASCADE. Además, este enfoque coloca por defecto los atributos como NOT NULL, para evitar perder información relacionada a los atributos.

Por otro lado, es esencial crear un contexto que esté relacionado con la base de datos. Dentro de este contexto, se deben definir las tablas para cada entidad junto con sus nombres correspondientes. También es posible definir llaves primarias y foráneas, incluso cuando estas sean compuestas. Es importante establecer las cardinalidades cuando se relacionan dos entidades mediante una clave foránea, incluso si esta llave foránea es compuesta.

A pesar de que los scripts autogenerados y las bases de datos creadas manualmente a menudo tienen diferencias en su naturaleza, función y nivel de flexibilidad, en este proyecto en particular, el esquema relacional y el script SQL no presentan muchas diferencias en su diseño y estructura. Esto se debe a varias razones.

En primer lugar, antes de comenzar con el script autogenerado, se trabajó en el esquema relacional. Esto permitió que el código se basara en los requisitos en los que depende el esquema relacional. Por lo tanto, las diferencias entre estos dos aspectos fueron mínimas, ya que el script sigue en gran medida el mismo patrón de comportamiento que se esperaría al crearlo manualmente.

Sin embargo, es importante mencionar algunas diferencias en las tablas generadas y su cantidad. Esto se debe a que el enfoque inicial se centró en las historias más importantes, que fueron seleccionadas en el sprint Backlog para el desarrollo del sprint 1.

La elección entre las metodologías de desarrollo de bases de datos, ya sea utilizando un script SQL autogenerado o un esquema relacional, es una decisión que depende en gran medida de las necesidades específicas del proyecto y de las preferencias del programador. La complejidad del proyecto desempeña un papel crucial en la elección de la metodología. Para proyectos con niveles de complejidad bajos o moderados, un script SQL autogenerado puede ser una opción rápida y conveniente. Sin embargo, para proyectos altamente complejos, un esquema relacional ofrece un mayor control y estructura.

Si es importante mantener una convención y estándar específicos en la base de datos, un script autogenerado puede ser útil para garantizar la coherencia y reducir errores, ya que sigue un patrón predefinido. Cuando se requiere un alto grado de personalización en la obtención y manipulación de datos, un esquema relacional proporciona la flexibilidad necesaria para diseñar consultas y relaciones de datos de manera específica.

Desde esta perspectiva qué método utilizar se determina en función del sistema, ya que, por un lado  el script autogenerado puede generar ventajas como rapidez y automatización, pero desventajas como un control menor con respecto a tareas específicas y cambiantes, mientras que los scripts generados manualmente permiten un mayor control sobre la estructura y rendimiento de la base de datos.
