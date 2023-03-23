# Empathy_AI
 Empathy AI allows for a human-AI interaction without words. Unlike prompt-based interactions this project allows understanding through facial expressions. Whatever emotion the human user might be feeling, the AI will recognize and adjust it's color and sound to the human emotion, showing empathy.
 
 The application was built in Unity. Facial feature recognition was done using [FaceOSC](https://github.com/kylemcdonald/ofxFaceTracker/releases/). An OSC messages with facial feature parameters are sent to [Wekinator](http://www.wekinator.org) where they are mapped to "emotions" or numbers. These are sent to Unity via [UnityOSC](https://thomasfredericks.github.io/UnityOSC/). In Unity the input values from Wekinator are categorized into emotions and the screen is colored in a color matching the emotion. In addition the emotion parameters are sent to a ChucK program via OSC from Unity where audio output is synthesized. The sound and colors match the human facial expression.

![](image.png)
