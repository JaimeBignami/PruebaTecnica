# Proyecto PruebaTecnica

Este proyecto es una API desarrollada en .NET 9 con C# 13.0, cuyo objetivo es gestionar transacciones de manera robusta y escalable. A continuación se detallan las decisiones de diseño y pasos realizados:

## Decisiones de Diseño

- **Estructura en Capas:**  
  Se dividió la solución en múltiples proyectos: API (presentación), Application (lógica y validaciones), Infrastructure (acceso a datos y servicios externos) y Test (pruebas unitarias).  
- **Inyección de Dependencias:**  
  Se implementó la inyección de dependencias para facilitar la mantenibilidad y testabilidad. Esto incluye la integración de repositorios, servicios de simulación y validadores.
- **Registro de Logs:**  
  Se usó NLog para el registro de eventos (información, advertencias y errores). La configuración se gestionó a través del archivo `nlog.config` para permitir ajustes flexibles.
- **Política CORS:**  
  Se configuró una política de CORS (`AllowAll`) en el __Program.cs__ para permitir que la API sea consumida desde múltiples orígenes.
- **Estrategia de Validación:**  
  Se utiliza un validador personalizado (`CreateTransactionsValidator`) para garantizar que los datos de entrada cumplan con las reglas de negocio antes de procesar una transacción.
- **Manejo de Respuestas ISO:**  
  La lógica de negocio asocia diferentes códigos ISO a estados específicos de transacción (aprobado, rechazado, fondos insuficientes, error temporal, y no autorizado), permitiendo una comunicación clara del resultado del procesamiento.
- **Simulación de Respuestas:**  
  Se implementó el servicio `IMockService` para simular las respuestas del adquirente y validar distintos escenarios, como transacciones rechazadas o con error temporal.
- **Pruebas Unitarias:**  
  Se desarrollaron pruebas unitarias con xUnit y Moq en el proyecto __Test__ para verificar que cada escenario (transacción válida, rechazada, fondos insuficientes, error temporal y tarjeta expirada) se maneje según lo esperado. 

## Pasos Realizados

1. **Configuración del Host y Servicios:**  
   Se configuró el host en el archivo __Program.cs__, cargando configuraciones a partir de archivos JSON y estableciendo proveedores de logging y políticas de CORS.

2. **Implementación de la Lógica de Transacciones:**  
   La capa de aplicación define comandos, validadores y manejadores (ej. `CreateTransactionsHandler`) para procesar las transacciones.  
   - Se registra cada intento de transacción en la base de datos.
   - Se validan las solicitudes y se simulan respuestas del adquirente.
   - Se controlan escenarios de reintentos y se registran estados según los códigos ISO recibidos.

3. **Diseño del Acceso a Datos:**  
   Se implementaron repositorios que se comunican con una base de datos a través de abstracciones (`IBaseRepository` y `ITransactionsRepository`) para facilitar la persistencia de las transacciones.

4. **Configuración de Logging y Manejo de Errores:**  
   Se añadió un sistema robusto de logging a lo largo de la aplicación, que captura información detallada en cada paso del proceso y en caso de errores.

5. **Cobertura con Pruebas Unitarias:**  
   Se desarrollaron pruebas unitarias (ubicadas en __PruebaTecnica.Test__) que validan los distintos caminos de ejecución, asegurando la integridad de la lógica de transacciones.

Este enfoque arquitectura permite un mantenimiento sencillo y la incorporación de nuevas funcionalidades de forma modular, asegurando al mismo tiempo robustez y confiabilidad en la operación de la API.
