# Manual Tests

In order to prevent features from breaking as the project progresses, most of them are written out here. 

To merge a branch with dev all of the following tests must pass on it. 

If a feature isn't described here it isn't guaranteed that it will work in future commits as there is no easy way to check if it works. 

Feel free to add tests here or edit them according to your judgement, just remember to note it in your commit message.

## Main menu
- Open Scenes/MainMenu.unity file. You should be presented with three buttons: *start*, *settings* and *quit*.
- After clicking on settings four dropdown options should appear: *mode* and *simulation mode*, *scenes* and *targets* as well as back button.
- Build the project with menu scene selected as main scene and test if clicking *exit* closes the window. Alternatively you can check if `Application.Quit()` is called after clicking the button.

## Free swim mode
- Open *Scenes/MainMenu.unity* file.
- Set mode to *player* and simulation mode to *free swim* in settings.
- Go back and click *start*.
- A scene should open.
- Check if you're inside of the pool, you can see upper water surface and you can recognise that you're under water by camera effects.
- Check if following controls work:
	- `wasd` to move forwards, left, backwards and right
	- `qe` to rotate left and right
	- `rf` to move upwards and downwards
	- go to an object on layer *ball*, press and hold `t`, it should follow the boat
	- torpedo is left out for now as it has no visual indication
Debug screen:
	- Press `b` button while moving.
	- It should pause the simulation and display debug screen with the data exchanged with python. 
	- Judge if the values there are correct. 
	- Press `v' to close debug screen and resume the simulation.
	- Do it five times while moving around.
- Go to step one and select different scene, test for each scene.

## Data collection mode
- Open Scenes/MainMenu.unity file.
- Set mode to *player* and simulation mode to *data collection* in settings.
- Go back and click *start*.
- A scene should open.
- You should see quickly changing scene views.
- Press `b` button
	- It should pause the simulation and display debug screen.
	- Check if you're inside of the pool, you can see upper water surface and you can recognise that you're under water by camera effects.
	- You should see a green rectangle around an object.
	- Verify that the rectangle approximates the area covered by the object on screen. 
	- Check if the object is a target in academy.
	- Judge if the floating point values displayed on the screen are correct. 
	- Resume the simulation.
	- Click `b` 4 more times and repeat the previous checks.
- Go to step one and select different scene, test for each scene and targets.

## Academy settings
- Open one of the simulation scenes.
- Open academy object.
- Set FocusedCamera and CollectData to one and open the scene.
- Run the scene and check if the generated scene views are correct.
- Set EnableNoise to zero, there shouldn't be any objects from the noise folder on the scene.
- Set EnableNoise to one, the objects from noise folder should be placed on the scene. 
- Test if rest of the options work according to description in *README->Essential Academy parameters->Reset Parameters*
