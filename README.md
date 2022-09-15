# POE Trade Announcer

This app simply announces when a trade happens in-game using Microsoft TTS. This operates from your logs/Client.txt file, so I'd recommend backing up your current Client.txt file then wiping the file to boost performance. It's slightly hard to not load the entire .txt file everytime you open it, this is why I recommend the wipe(that file can get big!).

## Inspiration/Usage

I like lying down on my bed, and I don't like waiting at my computer for trades to go off. So with this TTS application, you can watch Netflix in bed and know exactly when someone is messaging you! Innovation!

## How to compile

You simply need to load this project's .sln file in Microsoft Visual Studio, then press the >Start button. If you don't have the .sln or it doesn't work you can also use the .csproj file.

After that search for the /bin folder in the project's directory and you'll find the project's .exe.

## Some notes

You cannot read from a file from different sources concurrently. Path Of Exile uses the logs/Client.txt file during normal operations for their game, so this app copies that file and operates from the newly created file. That new file is deleted and remade every 5 seconds. This seems like a scuffed solution because it is, but it is an offline + non API method of getting reliable notifications!
