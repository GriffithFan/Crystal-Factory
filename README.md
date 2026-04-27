# Fabrica de Cristales Cosmica

Base inicial de un juego mobile idle clicker 2D para Android hecho en Unity.

## Objetivo

Crear un MVP monetizable de forma sostenible:

- Progreso por toque.
- Produccion automatica.
- Upgrades con economia configurable.
- Guardado local.
- Progreso offline.
- Recompensa diaria.
- Etapas narrativas con lore desbloqueable.
- Misiones de corto plazo.
- Boost temporal por rewarded ad.
- Rewarded ads simulados.
- IAP simulados para reemplazar luego con Google Play Billing.

## Documentos importantes

- `Juego.txt`: documento de diseno completo.
- `Docs/UnitySetup.md`: pasos para montar la escena en Unity.
- `Docs/SustainableMonetization.md`: reglas de monetizacion sostenible.
- `Docs/GitHubUnityWorkflow.md`: flujo recomendado Unity + VS Code Agent + GitHub.
- `Docs/GitCommands.md`: comandos Git utiles para el proyecto.
- `Docs/DevelopmentLog.md`: registro vivo de progreso y contexto.
- `Docs/ArtAndLoreDirection.md`: guia visual, lore y prompts IA.
- `Docs/AIPrompts.md`: prompts listos para generar imagenes IA del juego.
- `Assets/Resources/GameData/UpgradeCatalog.json`: economia inicial editable.
- `Assets/Resources/GameData/MissionCatalog.json`: misiones iniciales editables.
- `Assets/Resources/GameData/StageCatalog.json`: etapas narrativas y paleta visual.

## Primer hito

Lograr en Unity que el jugador pueda:

1. Tocar el cristal.
2. Ganar cristales.
3. Comprar upgrades.
4. Generar cristales por segundo.
5. Cerrar y abrir el juego.
6. Recibir progreso offline.
7. Desbloquear etapas narrativas.
8. Completar y reclamar misiones.

Cuando este hito funcione, el siguiente paso es crear una UI visualmente atractiva y reemplazar los stubs de ads/IAP por SDKs reales.

## Flujo recomendado

Usar GitHub como punto central del proyecto:

1. Codear sistemas y documentacion desde VS Code.
2. Probar escenas, prefabs y assets desde Unity.
3. Subir avances a GitHub con commits frecuentes.
4. Volver a VS Code para que el agente trabaje siempre sobre el repo actualizado.

## Vista recomendada en Unity

El juego esta pensado para movil vertical. En la pestaña `Game`, usa un aspect ratio vertical como `9:16`, `1080x1920` o `Portrait` para revisar la UI correctamente.
