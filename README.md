# IRBIS
## Build Version: `0.2.0.14` | omg grass  


### Known Bug List  
`01.` Multithreading breaks down when too many enemies are being updated. (off by default)  
`02.` Vending Machine menu still slows the game down a lot (~30%) I think this is due to the transparency  
`03.` Camera swing causes weird things and has been disabled for the time being  
`04.` Boss strike attack during bury phase sometimes hits twice  
`05.` Tailwhip and strike seem to not trigger their knockbacks occasionally  
`06.` Player death causes freeze with multithreading (multithreading will get completely reworked in the future)  


### Important Notes  
`01.` Try out C0B0! Our first boss!  
`02.` C0O0 is a test level. There is no content there, only an extremely early version of a 'survival mode'  
`03.` There is no death screen (yet!)  
`04.` Some animations are missing. The lizard boss has none, and only uses concept art. And the player is missing attack and roll animations.  
`05.` Move the torch by clicking on the screen (change the darkness of the scene with console command `darkness={float}`)

### Controls  
`     A:` Move left  
`     D:` Move right  
` Space:` Jump  
`     Q:` Shield  
`     E:` Shockwave (hold&release for stronger shockwave)  
`S+Jump:` Slam  
`     N:` Enter frame-by-frame mode (and next frame)  
`     G:` Exit frame-by-frame mode  
`     R:` Use  
`     K:` Spawn some dudes!  
`LShift:` Roll  
` Enter:` Attack  
` Tilde~` Open developer console  
`Escape:` Quits to previous menu (exits the game on main menu)  


### Playable Version  
You can download a playable version of the latest build of the game here: [Ln2.co](https://Ln2.co/#download)  
(windows only right now, I'm afraid)  


### Run Requirements  
`1.0` Microsoft .NET framework 4.5 (included in windows 10)  
`1.5` If you are unsure if your system has .NET 4.5, you can find an installer here: https://www.microsoft.com/en-us/download/details.aspx?id=30653  
`2.0` OpenGL 2.0 and support for framebuffers  
`2.5` It is most likely that your system has OpenGL pre-installed. However, if you are unsure, visit: https://www.khronos.org/opengl/wiki/Getting_Started  
`3.0` If you are still having touble running the game, please contact me! You can find me here: Darius@Ln2.co  


### Support Irbis
You can support the development of Irbis by visiting [patreon.com/Ln2](https://www.patreon.com/Ln2)  