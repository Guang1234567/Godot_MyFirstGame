# !bin/bash

 keytool -keyalg RSA -genkeypair -alias androidreleasekey -keypass android -keystore release.keystore -storepass android -dname "CN=Android Debug,O=Android,C=US" -validity 9999

 keytool -keyalg RSA -genkeypair -alias androiddebugkey -keypass android -keystore debug.keystore -storepass android -dname "CN=Android Debug,O=Android,C=US" -validity 9999