## Build Version: 0.1.4.1
## |Menus

### Change Log (new changes since 0.1.4.0)
`01.` Background textures now center properly  
`02.` Screenscale is not saved if it is automatically generated (so that if you change window size it doesn't use the larger scale)  
`03.` Vending Machine only uses one texture for the background instead of four (slightly more efficient)  
`04.` New bars mocked up  

### Known Bug List
`01.` Background textures aren't centered properly (top left corner of the texture is in the center of the screen)  
`02.` Multithreading breaks down when too many enemies are being updated. (off by default)  
`03.` Vending Machine menu still slows the game down a lot (~30%) I think this is due to the transparency  

### Controls
`         A:` Move left  
`         D:` Move right  
`         Q:` Shield  
`         E:` Shockwave (hold&release for stronger shockwave)  
`         G:` End frame-by-frame mode  
`         N:` Enter frame-by-frame mode (and next frame when in frame-by-frame mode)  
`         R:` Use vending machine  
`         K:` Spawn some dudes!  
`Left Shift:` Roll  
`     Enter:` Attack  
`    Tilde~:` Open developer console  
`    Escape:` Quits to previous menu (exits the game on main menu)  