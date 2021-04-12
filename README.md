# YouBeat

YouBeat is a Jubeat-style game designed for midi controllers, specifically, the Novation Launchpad. It's written in pure C#, utilising SFML and Otter.
Being purely 2D, it's ultra-lightweight, and will run on pretty much anything. Here it is running on a Surface Go!

![YouBeat Title Screen](/Images/youbeat_title.jpg)
![YouBeat Menu](/Images/youbeat_menu.jpg)
![YouBeat Main Game Screen 1](/Images/youbeat_game1.jpg)
![YouBeat Main Game Screen 2](/Images/youbeat_game2.jpg)
![YouBeat Main Game Screen 3](/Images/youbeat_game3.jpg)
![YouBeat High Score Entry](/Images/youbeat_hiscore.jpg)

[Images here when I upload them]

Check the releases if you just want to try the game. It comes with a (terrible, metronome only) sample beatmap to give you an idea of how it plays.

## Hardware

YouBeat *requires* a launchpad at present. There is no alternative input method. Of the launchpad models, the only one I have confirmed as working is the Mini Mk3, 
which I own and developed on. However, the launchpad-dot-net library, which I forked for this project, should support any launchpad which has RGB lighting. 
Certainly, the Launchpad X, Launchpad Pro and Launchpad Mini Mk3 appear to share a lot of similarities, so if you're buying one for this (which, please do not do, god), 
any of them should work. The MiniMk3 is cheap and really nice, though, highly recommend.

In theory, supporting non-RGB Launchpads would be possible, but the experience would be sub-par, I don't think it'd be easy to distinguish the different timing states.

##YouBeat Mapper

![YouBeat Mapper Main Screen](/Images/youbeatmapper.png)

Included in the release/solution is a mapper for making your own songs. A core goal of this project was to provide the mapping tools with the game in all instances - 
in my eyes this promotes a healthy scene and should encourage more people to make maps. It's how OSU! does it, after all.

That said, the mapper is not exactly slick. It is a separate winforms app, but it does the job. You can use a launchpad to input notes, or use the big grid to input - with fine-grain 
timing control available using the property grid on the right. Loading images is as simple as picking a jpg or png with the file dialog. You can adjust the metadata of the 
track with the properties grid to the right, and select the difficulty you're editing with the dropdown above it. On save, a few checks will be performed to ensure the track is 
valid and will load correctly into YouBeat. If you don't define beatsets for all difficulties, the excluded difficulties won't be shown in the YouBeat UI.

## Info for the Jubeat-familiar

The socring system in YouBeat does not follow that of Jubeat at all and is arguably quite naive. Rearchitecting the scoring system to more closely match 
Jubeat would be a good improvement. 

Neither YouBeat or the mapper currently support hold notes. The mapper has an importer for a text-based format for some other Jubeat sim I found floating around online 
- the format used .bsc, .adv and .ext text files to describe the different difficulty levels. If that sounds familiar to you, good news, this has *very* beta support for importing them.

