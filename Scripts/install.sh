#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
unity_url="http://download.unity3d.com/download_unity/960ebf59018a/UnityDownloadAssistant-5.3.5f1.dmg"
echo 'Downloading from $unity_url: '
curl -o Unity.dmg "$unity_url"
hdiutil attach "$(pwd)/Unity.pkg"
mount
ls -lh
echo 'Installing Unity.pkg'
# sudo installer -verbose -allowUntrusted -dumplog -package Unity.pkg -target /
hdiutil detach "$(pwd)/Unity.pkg"
mount
