Script and info from;
https://forum.unity.com/threads/path-to-the-final-ipa-file.754331/#post-5527900

0) Only follow this after you are able to generate a iOS build on Unity Cloud Build (setup p12 certs etc.)

1) In the iOS cloud config click 'Environment Variables' and add new environment variables for target, username and password (TARGET_NAME, ITUNES_USERNAME and ITUNES_PASSWORD). 
You need to get a one-off password from Apple that only works on Application Loader - not the personal appleid password. (see https://support.apple.com/en-gb/HT204397)

You need to provide it with (can be set via the Cloud Build config on the Cloud Build website)
ITUNES_USERNAME - Basically email
ITUNES_PASSWORD - App generated password
TARGET_NAME - Name of the target in the Unity Cloud build (e.g. "Default IOS")
The other env variable(s) like WORKSPACE are set by Cloud Build

2) In the iOS cloud config click 'Advanced Options' and in the Post-Build Script path put "*path_to_this_script*/post-build-push-ipa-appstore.bash"
eg. "Assets/BuildScripts/Post Build/post-build-push-ipa-appstore.bash"

3) Run build - it will be available in testflight if successfull - otherwise check the logs on Unity Cloud Build.


known issues:
The first time uploading you might get an error;
"Error: code 1190 (App Store operation failed. No suitable application records were found. 
Verify your bundle identifier ‘com.Virsabi.EsbjergKlimaklar’ is correct.)"
and in case you are sure the correct identifier is set in both Unity iOS build settings, the certificates, 
and on appstore connect, then you may be able to solve the issue by first uploading to appstore connect manually from a mac 
thjrough xcode - that atleast solved it for me. (mu@virsabi.com)