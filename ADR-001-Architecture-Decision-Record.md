<img width="1249" height="641" alt="image" src="https://github.com/user-attachments/assets/be75363a-a72a-4f84-b04a-33c454139377" /># ADR-001 – Decisiones de Arquitectura del Servicio de Scoring Crediticio

## Estado
Aceptado

## Contexto

PaymentTech Solutions está modernizando su plataforma legacy para soportar nuevos productos financieros y cumplir regulaciones como PSD2 y SOX.  
Dentro de este contexto, se requiere desarrollar un servicio de validación crediticia que será crítico para el flujo de checkout del producto *PayLater*.

El servicio debe:
- Validar datos del cliente (nombre, identificación y monto).
- Consultar un proveedor externo de scoring crediticio (FinScore).
- Aplicar reglas de negocio para aprobar o rechazar la transacción.
- Retornar una decisión inmediata al consumidor.
- Registrar la solicitud y la respuesta para fines de auditoría, trazabilidad y análisis.

Dado que el servicio impacta directamente en la experiencia del usuario, la latencia, disponibilidad y resiliencia son factores clave de diseño.

---

## Decisiones de Arquitectura

### Diagrama de Componentes C1
<img width="720" height="720" alt="image" src="https://github.com/user-attachments/assets/4e011999-b03b-4ea8-928a-cf4bd934eae5" />



### Diagrama de Componentes C2

<img width="1249" height="641" alt="image" src="https://github.com/user-attachments/assets/c0f71ce4-df47-45bd-b94b-95bf30d47435" />



### 1. Estilo arquitectónico

Se adopta **Clean Architecture** con el objetivo de:
- Separar responsabilidades.
- Aislar el dominio de dependencias externas.
- Facilitar pruebas unitarias y de integración.
- Permitir evolución tecnológica sin afectar la lógica de negocio.

Capas principales:
- **API**: Exposición de endpoints HTTP.
- **Application**: Casos de uso y reglas de negocio.
- **Domain**: Entidades y modelos del dominio.
- **Infrastructure**: Integraciones externas, persistencia y mensajería.

---

### 2. Integración con proveedor externo

La integración con el proveedor FinScore se realiza mediante un cliente HTTP encapsulado en la capa **Infrastructure**, expuesto a la capa **Application** a través de una interfaz.

Esto permite:
- Bajo acoplamiento.
- Sustitución futura del proveedor.
- Mocking sencillo para pruebas.

---

### 3. Reglas de negocio

Las reglas de decisión se implementan en la capa **Application**, desacopladas de transporte, persistencia e infraestructura.

Reglas definidas:
- Score mayor o igual a 700: Aprobado.
- Score entre 500 y 699: Rechazado.
- Score menor a 500: Rechazado.

---

### 4. Persistencia de solicitudes y respuestas

#### Implementación utilizada en el ejercicio

Para fines del ejercicio técnico:
- Se utiliza una **base de datos local**.
- La persistencia se implementa de forma directa.
- El objetivo es facilitar la ejecución local y la evaluación del flujo completo.

Esta decisión reduce la complejidad operativa del ejercicio sin afectar la validación de los requerimientos funcionales.

---

#### Enfoque recomendado para un entorno productivo

En un escenario real, la persistencia **no debe formar parte del camino crítico del negocio**.  
Por esta razón, la decisión arquitectónica recomendada es:

- Publicar un evento con el resultado del scoring en **Azure Service Bus**.
- Implementar un **componente Worker/Consumer independiente** que:
  - Se suscriba a la cola o tópico.
  - Realice la persistencia en base de datos.
  - Maneje reintentos y Dead Letter Queue (DLQ).

Este enfoque desacopla el API de la base de datos y evita fallos en cascada ante problemas de infraestructura.

---

### 5. Uso de mensajería asíncrona

El uso de mensajería permite:
- Responder al cliente sin depender de la disponibilidad de la base de datos.
- Reducir la latencia percibida.
- Garantizar la entrega confiable de eventos.
- Escalar el procesamiento de persistencia de manera independiente.

El API únicamente espera la confirmación de publicación del mensaje, no el procesamiento del mismo.

---

## Justificación

Las decisiones adoptadas buscan:
- Alta disponibilidad del servicio.
- Bajo tiempo de respuesta para reducir abandono en checkout.
- Mayor resiliencia ante fallos.
- Escalabilidad horizontal.
- Alineación con buenas prácticas de arquitectura enterprise.

Se acepta consistencia eventual para la persistencia, ya que esta no impacta la decisión inmediata del negocio.

---

## Consecuencias

### Positivas
- Menor acoplamiento entre componentes.
- Mejor experiencia de usuario.
- Arquitectura preparada para crecimiento.
- Facilidad de auditoría y reprocesamiento.
- Diseño alineado a sistemas financieros modernos.

### Consideraciones
- Se requiere monitoreo de colas y DLQ.
- Se introduce un componente adicional (Worker).
- La persistencia no es inmediata, sino eventual.

Estas consideraciones son aceptables y esperadas en arquitecturas distribuidas.

---

## Alternativas consideradas

### Persistencia síncrona directa
Rechazada para un entorno productivo por generar dependencia fuerte con la base de datos y mayor riesgo operativo.

### Outbox Pattern
Considerada, pero descartada para este ejercicio por introducir mayor complejidad sin un beneficio proporcional en el contexto evaluado.

---

## Conclusión

La solución propuesta cumple con los requerimientos del ejercicio y aplica principios de arquitectura moderna.  
Aunque el ejercicio utiliza una base de datos local para simplificar la implementación, la arquitectura recomendada desacopla la persistencia mediante mensajería y procesamiento asíncrono, garantizando resiliencia, escalabilidad y una experiencia óptima para el usuario final.
