# Proyecto final para la clase Artificial Life:

Realizar un ambiente donde existen dos especies, Presas y Depredadores, junto con Recursos Naturales.

Tanto las presas como los depredadores van a ser definidas por un Código Genético, que dan origen al fenotipo(individuo), en este código se define:
* La edad (iteraciones).
* Edad de crecimiento (tamaño inicial -> tamaño final), solo en la edad adulta (tamaño final) pueden reproducirse.
* Periodo de vida (distribución normal).
* Rango de visión (rango de visión).
* Color de su piel (turing morph parametters).
* Metabolismo (cantidad de energia para vivir y reproducirse) donde cada individuo se encarga de la energia que tiene, este metabolismo tiene energia minima y máxima, un energia necesaria para reprodución, y una energia para movimiento.

Los depredadores tienen como rasgo especial una mayor velocidad de movimiento que las presas.

Las presas o buscan energia (plantas) o buscan pareja, cuando no estan buscando pareja se mueven como boids, evitando depredadores.

Al momento de la reproducción, tanto en presas como depredadores, se hace un cruce de código genético de los individuos con una alteración aleatoria (mutación), generando un hijo único.

Para las plantas se tiene lo siguiente:
* 2 o 3 tipos de plantas, generadas con L-systems, parametros aleatorios.
* Nota: Distribución de aparición puede seguir distribución sand pile.
* Se distinguiran por 2 regiones, que simulan estaciones, donde la estación puede ser invierno o verano, y distingue si una planta tiene energia para dar o no. 
* Estaciones definidas con modelo Sugar-landscape.
