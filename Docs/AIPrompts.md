# Prompts IA para Crystal Factory

Objetivo: generar assets consistentes para un juego mobile vertical, idle clicker, llamativo y legible en pantallas pequenas.

## Reglas generales

Usa estos parametros en casi todos los prompts:

```text
2D mobile game art, polished casual idle clicker style, clean silhouette, high readability at small size, vibrant but controlled colors, cyan crystal glow, dark futuristic background, no text, no watermark
```

Evita:

- Texto dentro de la imagen.
- Fondos demasiado cargados.
- Personajes hiperrealistas.
- Estilos distintos entre assets.
- Imagenes muy oscuras donde no se distinga el objeto.

Formato recomendado:

- Fondos: PNG 1080x1920.
- Iconos/upgrades: PNG 512x512 con fondo transparente.
- Cristal/personajes: PNG 1024x1024 con fondo transparente.
- Store feature graphic: 1024x500.

## Cristal principal

```text
2D mobile game asset, large glowing cyan crystal core, faceted magical gemstone, floating slightly above a small sci-fi pedestal, clean silhouette, transparent background, polished casual idle clicker style, high readability, cyan and turquoise glow, no text, no watermark
```

Variacion mas premium:

```text
2D mobile game hero asset, cosmic crystal heart, faceted cyan gemstone with golden inner light, elegant sci-fi pedestal, soft particles, transparent background, polished casual game style, no text
```

## Fondo etapa 1: Fragmento Dormido

```text
Vertical 2D mobile game background, dark cozy futuristic laboratory, empty center space for a crystal button, small workbench, dim cyan glow, subtle cables and tools, clean readable composition, polished casual idle game style, no text
```

## Fondo etapa 2: Primera Resonancia

```text
Vertical 2D mobile game background, futuristic crystal lab waking up, glowing cyan energy lines, workbench with activated tools, soft magical particles, empty center space, polished casual game art, no text
```

## Fondo etapa 3: Taller Automatico

```text
Vertical 2D mobile game background, automatic crystal workshop, cute small machines, conveyor belts, tiny robotic arms, cyan and gold lighting, clear center space for UI, polished casual idle clicker style, no text
```

## Fondo etapa 4: Nucleo del Portal

```text
Vertical 2D mobile game background, cosmic crystal portal chamber, glowing circular portal behind center area, cyan and violet energy, sci-fi runes without readable text, dramatic but clean composition, mobile game art, no text
```

## Fondo etapa 5: Fundicion Orbital

```text
Vertical 2D mobile game background, orbital crystal foundry in space, small drones, glowing machinery, planet visible in distance, cyan gold and deep navy palette, empty center for gameplay, polished casual game style, no text
```

## Fondo etapa 6: Motor Galactico

```text
Vertical 2D mobile game background, massive galactic crystal engine, huge reactor rings, stars and nebula, cyan magenta and gold accents, epic progression feeling, clean mobile composition, no text
```

## Mascota/personaje guia

Idea: un pequeno robot asistente que explique misiones, recompensas y eventos.

```text
2D mobile game character, cute small floating robot assistant, crystal-powered core, cyan glowing eyes, friendly expression, compact silhouette, transparent background, polished casual game style, no text
```

Variaciones:

```text
2D mobile game character sheet, cute crystal lab robot assistant, 4 expressions happy surprised focused celebrating, cyan glow, transparent background, polished casual style, no text
```

## Maquinas/generadores

### Pulidor Manual

```text
2D mobile game upgrade icon, small hand crystal polishing tool, cyan gem dust, clean outline, transparent background, polished casual style, no text
```

### Martillo de Resonancia

```text
2D mobile game upgrade icon, futuristic resonance hammer with cyan crystal head, glowing vibration rings, transparent background, clean silhouette, no text
```

### Mesa Energetica

```text
2D mobile game upgrade icon, compact energy workbench with glowing cyan crystal, sci-fi lab design, transparent background, polished casual style, no text
```

### Mini Robot Minero

```text
2D mobile game upgrade icon, cute mini mining robot holding tiny crystal drill, cyan glow, friendly design, transparent background, no text
```

### Reactor de Luz

```text
2D mobile game upgrade icon, small golden cyan light reactor, circular sci-fi device, glowing core, transparent background, polished casual style, no text
```

### Portal Mineral

```text
2D mobile game upgrade icon, small portal ring with crystal fragments coming through, cyan violet glow, transparent background, clean silhouette, no text
```

## Iconos UI

### Cristales moneda

```text
2D mobile game currency icon, small stack of cyan crystal shards, shiny, readable at 64px, transparent background, no text
```

### Recompensa diaria

```text
2D mobile game UI icon, daily reward chest with cyan crystal glow and golden trim, transparent background, polished casual style, no text
```

### Rewarded ad / boost

```text
2D mobile game UI icon, glowing play button inside crystal energy badge, cyan and green boost effect, transparent background, no text
```

### Remove Ads

```text
2D mobile game shop icon, clean shield blocking small ad cards, premium gold cyan style, transparent background, no text
```

## Cofres

```text
2D mobile game reward chest, crystal-powered chest, cyan glow, golden corners, transparent background, polished casual style, no text
```

```text
2D mobile game premium reward chest, cosmic crystal chest, violet cyan glow, golden trim, transparent background, high value feeling, no text
```

## Store / Google Play

### Icono app

```text
Mobile game app icon, close-up glowing cyan cosmic crystal, dark navy background, golden rim, strong silhouette, polished casual game icon, no text
```

### Feature graphic 1024x500

```text
Mobile game store feature graphic, cosmic crystal factory, glowing cyan crystal core, cute machines and small robot assistant, sense of progression and rewards, polished casual idle clicker art, no text, wide composition
```

### Screenshot background promocional

```text
Vertical mobile game promotional background, crystal factory idle clicker, glowing crystal center, upgrade machines around it, reward particles, clean space for UI overlay, polished app store screenshot style, no text
```

## Orden recomendado de generacion

1. Cristal principal.
2. Fondo etapa 1.
3. Icono moneda.
4. Iconos de los primeros 5 upgrades.
5. Mascota robot.
6. Fondo etapa 3.
7. Icono app.
8. Feature graphic.

## Como importar a Unity

Guarda los archivos en:

- `Assets/Art/Backgrounds/`
- `Assets/Art/Icons/`
- `Assets/Art/Machines/`
- `Assets/Art/UI/`

Despues en Unity:

1. Selecciona el PNG.
2. Texture Type: `Sprite (2D and UI)`.
3. Sprite Mode: `Single`.
4. Pixels Per Unit: mantener default para UI.
5. Apply.
