# BepInEx-MethodLogger

A simple plugin for BepInEx 6.0 for IL2CPP that allows the logging of method calls.

It uses reflection to determine the methods of specified classes, and then logs calls 
to those methods along with the arguements passed in whenever they are called. 

There is some simple filtering based on number of arguemenets and a whitelist / 
blacklist to determine what methods are logged, but it's pretty basic. I might 
end up adding more depending on whether I need it or not, but it's pretty 
simple to modify.

### Usage Instructions

Edit `types` to include the types you'd like to log, the minimum number of arguements 
in order to log calls to it (to exclude calls every frame like FixedUpdate, Update, ect) 
and a whitelist / blacklist of method names to log. 

Then, just build the plugin and put it into your `BepInEx/plugins` folder and it should 
work with any IL2CPP Game using BepInEx 6.0. It may work with other versions, but it is untested.
