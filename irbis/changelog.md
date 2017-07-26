## Build Version: 0.1.3.3
## |Vending Machine Menu

### Change Log (new changes since 0.1.2.0)
`01.` Completely overhauled the Camera system (somewhat still in progress)  
`02.` All menu titles now in HIGH DEF (smooth lines!)  
`03.` Player collider now rounds properly (numbers 17.5-18.5 round to 18 rather than numbers 18-19 rounding to 18)  
`04.` Fixed rounding in player collision (it wasn't rounding at all)  
`05.` Fixed some scaling bugs with new camera system  
`06.` Game should now automatically start in windowed fullscreen mode  
`07.` Fixed minor bug in collision (stemmed from changing positional rounding)  
`08.` Collision will now automatically push the play out of collided objects, should they get stuck  
`09.` The player can now walljump by pressing a directional key and jump key in either order, even before touching the wall  
`10.` Camera shake now uses lerp  
`11.` Walljump bahavior has been improved even more  
`12.` MULTITHREADING!  
`13.` Some multithreading bug fixes  
`14.` screenscale based on 480 pixel base instead of 960
`15.` Tiny bug fix with vending machine menu
`16.` Vending machine menu is now fully functional
`17.` Added ability to be able to generate tooltips (this is used in vending machine menus)

### Known Bug List
`01.` Backgrounds don't seem to center properly when screenscale is over 2  
`02.` Most menus are completely and totally broken.  
`03.` Multithreading breaks down when too many enemies are being updated.  

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