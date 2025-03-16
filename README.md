Link to executable *v0.0.7 (built for Windows): https://drive.google.com/file/d/1gdzYTzuW6O3OWjNUiQeD0_0HXVcW_mKT/view?usp=sharing

*Only tested on Windows and Linux. Not designed for phones or consoles.

**Game production is currently on hold as I'm in school from 2/1/2025 to 8/1/2025**

**Pray for my soul as I will be trying to speed run 18 classes in 6 months**

**Class Status: 6/18 almost half way done and in the 2nd month!**

### Windows:
1. Download the folder, and unzip it
2. Run the "Knight and Slime.exe"

### Linux or Mac Proton layers
1. Download the folder, and unzip it
2. In your Steam Library, click "Add a Game" and "Add a Non-Steam Game"
3. Click "Browse" and navigate to the "Knight and Slime.exe"
4. On the game page that is newly added to your Steam Library, click on the gear icon and go to "Properties"
5. Under the "Compatibility" tab, check the box to Force a compatibility layer, and then select a Proton version. I am currently testing on Proton 9.0-4 without any bugs.
6. Launch the game through Steam

## Dev Note
My current goal while I have free time is to make a level every week. I will also try to update this build link every week with the new levels (probably on Fridays, for my loyal game testers). If you encounter any bugs (or want to share any ideas for expansion or level designs) feel free to let me know at my dev email: onceaknight611@gmail.com or at my discord: onceaknight

Link to demo video of v0.0.1: https://drive.google.com/file/d/1bpuz1WCvNb4yPK0JK__P0Rkk7dMU9KpM/view?usp=drive_link

I plan to pretty much never update this video until the game is published (had to make it for my class).

# Overview

This repo contains everything inside the Assets folder of my game Knight and Slime that I'm making inside of Unity. 
The main reason for this repo is that it allows me to edit my scripts from my phone and now it lets other people access the game when I bother to update the builds.

# Branches
While in order to run the game itself its done through the link above. Here's an explanation of the branches.

- Main: contains the current stable version of the game that has been released
- Working: The branch that I'm actively working on. That means things may be broken or in the middle of changes. (If my team ever grows we will each have one of these)
- Test: **This branch will be made once there is a difference between players and testers.** This is where changes go before they are confirmed to be stable and are moved into the main branch.

## Git Notes

I'm using an extension called git for Unity to facilitate uploading my project directly inside of Unity to this remote repo.
I'm also using Git LFS to store my large binary files such as images and sound in this repo. 
Since my free Git LFS storage is almost up in the future if more remote Git LFS storage is needed for this or future projects the best free option would be to set up a "remote repo" inside of Google Drive.

## Dev Team Recruitment

I intend to see this game through to publication eventually and to meet that goal I will need to recruit people with the talents required for my team.

These are the spots open on my dev team:

### Game dev/Programmers:

- must be familiar with Git or GitHub or is very willing to learn how to use it and the etiquette. GitHub is the only way we could work together so this is a must!
- familiar with Unity or any game engine
- familiar with any version of C or any object-oriented language
- Contact me and I'll put in the effort to have the whole unity project stored on this GitHub not just the assets folder then just feel free to request to branch and push

### Graphic Artist/Designer: 

- ideally likes working with pixel art
- can make sprite sheets (If needed I can teach this, its really not that hard)
- ideally already has graphic design software that they are familiar with or are willing to learn on their own
- bonus! likes cute slimes and medieval-themed art

### Audio Engineer/Mixer:

- can mix multiple layers of music
- familiar with things like fading, etc.
- ideally has some software they are familiar with or are willing to learn on their own
- bonus! Can make their own music and know some of the game industry tricks for looping and the like
- *note* I'm currently using Unity for my game which isn't great for crazy cool music stuff. I'm in the process of finding an extension for Unity to fix this.

## Version Changes:

v0.0.7: Added level 8. Slime can now absorb keys and spit them out later. With updated guide. New inventory display for the Slime. Game icon improved. Temporary solution for the slime changing colors when it absorbs a liquid.

v0.0.6: Added level 7, featuring keys and locked doors usable by the knight. Improved the splash screen logos. Redid the collectable and liquid classes, they work much better now.

v0.0.5.3: New player displays in game. Level complete time now effects your score. Added keys and locked doors to be used in level 7. Added splash screen. 

v0.0.5.1: Cutscene is only on the first level. Skip button added to all cutscenes. High score notice. Menu bug fixes, can no longer access pause menu during death or level complete, can't access the menu during cutscenes.

v0.0.5: Added level 6. Guide screen graphics changed. Credits Screen added. Minor menu bugs fixed.

v0.0.4: Added level 5. Fixed more saving and high-score display bugs. Made falling into tunnels as the slime smoother. Fine-tuned box objects.

v0.0.3: Added levels 3 and 4. Created falling platform objects. Can't hold down jump anymore. Created removable objects. switches can either control 2 objects or trigger 2 events. fixed score display bugs.

v0.0.2: Added level 2. Improved UI. Changed saving priority. Camera improved. Added .exe icon, name, and company name. And other stuff I don't remember anymore.

v0.0.1: The final project build for my 385 game dev class. main menu, level select, saving, 1 level, basic settings.
