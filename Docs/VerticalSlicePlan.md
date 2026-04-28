# Vertical slice actual

## Premisa

La Estacion Aurora era una fabrica orbital que alimentaba colonias con cristales de energia. Un fallo dejo todos sus sectores dormidos. El jugador encuentra el nucleo activo minimo y debe tocarlo, reinvertir energia, automatizar maquinas y cumplir pedidos para restaurar la estacion.

## Fantasia del jugador

Pasar de operador manual a dueno de una fabrica espacial rentable. Al principio cada toque importa; luego las mejoras, pedidos y boosts convierten la produccion en un negocio que crece solo.

## Loop principal

1. Tocar el cristal para producir energia inicial.
2. Comprar mejoras que aumentan toque, produccion automatica o multiplicadores.
3. Cumplir pedidos de energia para recibir pagos claros.
4. Desbloquear sectores con lore y fondos distintos.
5. Volver cada dia por suministros y progreso offline.
6. Usar boosts voluntarios para acelerar momentos de alto valor.

## Pantallas del MVP

- Menu principal: presenta Aurora y la mision.
- Historia: explica por que existe la fabrica y que esta reparando el jugador.
- Carga: transicion breve con mensajes de calibracion.
- Fabrica: HUD, cristal, objetivo activo, acciones, mejoras y pedidos.
- Bandeja inferior: solo muestra una seccion a la vez mediante pestanas de Mejoras y Pedidos.
- Incentivo visible: proxima recompensa/sector y cristales faltantes para desbloquearlo.
- Tienda simulada: packs, Remove Ads y texto de monetizacion responsable.
- Popup de progreso: recompensa, pedido completado o sector restaurado.

## Monetizacion sostenible

- Rewarded ads: aceleradores opcionales ligados a Boost x2 y recompensas.
- Remove Ads: compra de comodidad.
- Packs de cristales: ahorro de tiempo, no bloqueo de contenido.
- Retencion: misiones cortas, recompensa diaria, progreso offline y sectores desbloqueables.
- Regla de diseno: el jugador siempre debe entender que puede avanzar sin pagar.

## Primera prueba en Unity

1. Abrir el proyecto en Unity.
2. Esperar compilacion completa.
3. Ejecutar `Tools > Cosmic Crystal Factory > Create Prototype Scene`.
4. Abrir `Assets/Scenes/Game.unity`.
5. Usar Game view en `9:16` o `1080x1920`.
6. Presionar Play.
7. Probar menu, historia, iniciar fabrica, tocar cristal, comprar mejora, reclamar pedido, abrir tienda y usar Boost x2.

## Proximos contenidos de alto impacto

- Mascota/robot asistente con frases contextuales.
- Animacion de restauracion por sector.
- Cofres de pedidos con rarezas.
- Skins de cristal vendibles sin afectar poder.
- Calendario semanal de pedidos especiales.
- Balance con sesiones de 2 a 5 minutos para mobile.
