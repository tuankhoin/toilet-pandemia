# Toilet Pandemia

Play a trial version of the game on browser here: https://lil-karens.itch.io/toilet-pandemia

## Game Explanation and Gameplay
This was the main game project for COMP30019 - Graphics & Interactions

### **Game Explanation**

Our game is a first person shooter (FPS), based in a post-apocalyptic world where COVID-19 has ravaged the world's population. You assume the role of an average manager, intent on locating and distributing the vaccine to finally put an end to the pandemic. However this vaccine is held in a nearby shopping center, defended by a horde of Karens who want nothing more than to see the world burn, having succumbed to the frustrations of state-enforced lockdown long ago. 

Your objective, to enter the shopping center, and collect critical supplies for the residents in your community, all the while doing the following:

1. Avoiding incineration at the hands of the Karens' powerful fire attacks
2. Maintaining an appropriate 1.5m social distance, or else risk contracting COVID-19 from the Karens themselves
3. Surviving long enough to discover the super vaccine, and taking it when it appears

Points are accrued for gathering supplies, defeating Karens, and surviving levels. Health packs will also randomly spawn, that will allow the player to recover any lost health. The game takes on a classic arcade 'survival' format, that is, the player plays until he/she finally falls to the Karen hordes, an inevitability since each level rises in difficulty to eventually impossible scenarios.

### **Gameplay**

<p align="center">
  <img src="Gifs/Play.gif" width="800" >
  <br>A game play of Toilet Pandemia.
</p>

#### **<u>Controls</u>**
|    Button    |      Function      |
| :----------: | :----------------: |
|  `W/A/S/D`   | Character Movement |
|   `Space`    |        Jump        |
| `Left-Mouse` |       Shoot        |
|     `R`      |       Reload       |

## External Code/APIs

### Assets
To conserve time and focus on gameplay elements, many of the gameplay assets were sourced from third parties online:

- Gun sourced from the 'Sci-Fi Weapons' free pack at [DevAssets](https://devassets.com/assets/sci-fi-weapons/).
- Flashlight sourced from [Unity Asset Store](https://assetstore.unity.com/publishers/884).
- Toilet paper sourced from [Done3d](http://done3d.com/toilet-paper/)
- N95 mask sourced from [TurboSquid](https://www.turbosquid.com/3d-models/n95-mask-coronavirus-3d-model-1535320).
- The supermarket environment sourced from [Unity Assest Store](https://assetstore.unity.com/publishers/5217).
- 'Minecraft Steve' object, sourced from [Clara.io](https://clara.io/view/1edd3bc9-ebaf-4bc2-b994-4393ed3ce6d8).

To ensure a consistent aesthetic for the game in spite of these different sources of objects, the toon shader (see [Toon Shader section](#toon-shader)) was utilized for all objects.

### Shader

Tutorial sources:
* Transparent modification: https://learn.unity.com/tutorial/writing-your-first-shader-in-unity
* Toon shader: https://roystan.net/articles/toon-shader.html 
* Outline shader: https://roystan.net/articles/outline-shader.html 
* Half-tone shader: https://www.ronja-tutorials.com/2019/03/02/halftone-shading.html

## Team Contributions

|   Team Member    |                                 Contribution                                         |
| :--------------: | :----------------------------------------------------------------------------------: |
| Tuan Khoi Nguyen | Gameplay, Object Modelling, Interactions, Graphics & UI, Transparent Shaders         |
| Hoang Anh Huy Luu|    Gameplay, Graphics & Camera, Map Design, Toon Shaders, Outline Shaders & Particles|
| HoangLong Nguyen |               Object Modelling, Half-Tone Shaders & Particles                        |
|   Angus Hudson   |                   Gameplay, Graphics & Camera, Evaluation                            |

***By the way, here is my honest review: COMP30019 sucks and not worth your tution fee. Markers are conservative and very subjective, and the fact is you can just look for the content off the internet or alumni repositories if interested. If you are from UniMelb, avoid this subject at all cost.***
