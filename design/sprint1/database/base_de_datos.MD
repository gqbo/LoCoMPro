# Base de datos - Avance 1

## Cambios en el ER

1. Se agreg� un campo �Apellido� como un nuevo atributo de la entidad �Usuario�, ya que se decidi� que un usuario ingresar� solo un apellido a la hora de registrarse dentro de la aplicaci�n web
2. Se elimin� el atributo �Informaci�n de contacto� compuesto por �Email� y �Tel�fono�, ya que no se utilizar�n en los atributos de un registro, solo el nombre de un establecimiento.

## An�lisis de comparaci�n

Debido a que en el desarrollo del sprint 1 del proyecto integrador estamos utilizando el enfoque Code First, se presentan diferentes aspectos que se pueden apreciar a la hora de implementar la base de datos, esto es debido a que dicho enfoque propone crear la base de datos a partir de los modelos de datos y su definici�n del contexto.

Dentro de cada modelo de datos, se pueden definir los diferentes atributos que corresponden a una tabla del modelo relacional. Para definir la llave primaria que corresponde a los modelos de datos se puede realizar de dos formas distintas, la primera corresponde a adicionarle al atributo que se desea definir como llave primaria las letras �ID�, lo que se ejemplifica como �NombreProductoID�. La otra alternativa corresponde a adicionar la anotaci�n �[KEY]�, de esta forma se define la llave primaria de los diferentes modelos de datos.

En el enfoque Code First, es necesario definir los atributos de navegaci�n, estos atributos representan las relaciones que existen entre los modelos, por medio de una llave for�nea. El modelo que posee la llave for�nea como llave primaria, debe tener una colecci�n de instancias de los modelos que adquieren las llaves for�neas, mientras que estos �ltimos solo deben tener una instancia del modelo que posee la llave for�nea como llave. 

Un aspecto relevante que posee el enfoque Code First, es que este por defecto establece en las restricciones de la llave for�nea las sentencias de ON DELETE CASCADE. Adem�s, este enfoque coloca por defecto los atributos como NOT NULL, para evitar perder informaci�n relacionada a los atributos.

Por otro lado, es esencial crear un contexto que est� relacionado con la base de datos. Dentro de este contexto, se deben definir las tablas para cada entidad junto con sus nombres correspondientes. Tambi�n es posible definir llaves primarias y for�neas, incluso cuando estas sean compuestas. Es importante establecer las cardinalidades cuando se relacionan dos entidades mediante una clave for�nea, incluso si esta llave for�nea es compuesta.

A pesar de que los scripts autogenerados y las bases de datos creadas manualmente a menudo tienen diferencias en su naturaleza, funci�n y nivel de flexibilidad, en este proyecto en particular, el esquema relacional y el script SQL no presentan muchas diferencias en su dise�o y estructura. Esto se debe a varias razones.

En primer lugar, antes de comenzar con el script autogenerado, se trabaj� en el esquema relacional. Esto permiti� que el c�digo se basara en los requisitos en los que depende el esquema relacional. Por lo tanto, las diferencias entre estos dos aspectos fueron m�nimas, ya que el script sigue en gran medida el mismo patr�n de comportamiento que se esperar�a al crearlo manualmente.

Sin embargo, es importante mencionar algunas diferencias en las tablas generadas y su cantidad. Esto se debe a que el enfoque inicial se centr� en las historias m�s importantes, que fueron seleccionadas en el sprint Backlog para el desarrollo del sprint 1.

La elecci�n entre las metodolog�as de desarrollo de bases de datos, ya sea utilizando un script SQL autogenerado o un esquema relacional, es una decisi�n que depende en gran medida de las necesidades espec�ficas del proyecto y de las preferencias del programador. La complejidad del proyecto desempe�a un papel crucial en la elecci�n de la metodolog�a. Para proyectos con niveles de complejidad bajos o moderados, un script SQL autogenerado puede ser una opci�n r�pida y conveniente. Sin embargo, para proyectos altamente complejos, un esquema relacional ofrece un mayor control y estructura.

Si es importante mantener una convenci�n y est�ndar espec�ficos en la base de datos, un script autogenerado puede ser �til para garantizar la coherencia y reducir errores, ya que sigue un patr�n predefinido. Cuando se requiere un alto grado de personalizaci�n en la obtenci�n y manipulaci�n de datos, un esquema relacional proporciona la flexibilidad necesaria para dise�ar consultas y relaciones de datos de manera espec�fica.

Desde esta perspectiva qu� m�todo utilizar se determina en funci�n del sistema, ya que, por un lado  el script autogenerado puede generar ventajas como rapidez y automatizaci�n, pero desventajas como un control menor con respecto a tareas espec�ficas y cambiantes, mientras que los scripts generados manualmente permiten un mayor control sobre la estructura y rendimiento de la base de datos.
