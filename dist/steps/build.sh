#!/usr/bin/env bash

#
# Set project path
#

UNITY_PROJECT_PATH="$GITHUB_WORKSPACE/$PROJECT_PATH"
echo "Using project path \"$UNITY_PROJECT_PATH\"."

#
# Display the name for the build, doubles as the output name
#

echo "Using build name \"$BUILD_NAME\"."

#
# Display the build's target platform;
#

echo "Using build target \"$BUILD_TARGET\"."

#
# Display build path and file
#

echo "Using build path \"$BUILD_PATH\" to save file \"$BUILD_FILE\"."
BUILD_PATH_FULL="$GITHUB_WORKSPACE/$BUILD_PATH"
CUSTOM_BUILD_PATH="$BUILD_PATH_FULL/$BUILD_FILE"

#s3InstallerPath="s3InstallerPath"
#s3InstallerAccessKey="$s3InstallerAccessKey"
#s3InstallerSecretKey="$s3InstallerSecretKey"
#s3InstallerRegion="s3InstallerRegion"
#s3InstallerBucketName="$s3InstallerBucketName"
#
#SERVER_INSTALLER_PATH="$SERVER_INSTALLER_PATH"
#serverDevelopmentAddress="$serverDevelopmentAddress"
#serverProductionAddress="$serverProductionAddress"
#serverTimeout="$serverTimeout"



#
# Set the build method, must reference one of:
#
#   - <NamespaceName.ClassName.MethodName>
#   - <ClassName.MethodName>
#
# For example: `BuildCommand.PerformBuild`
#
# The method must be declared static and placed in project/Assets/Editor
#

if [ -z "$BUILD_METHOD" ]; then
  # User has not provided their own build command.
  #
  # Use the script from this action which builds the scenes that are enabled in
  # the project.
  #
  echo "Using built-in build method."
  # Create Editor directory if it does not exist
  mkdir -p "$UNITY_PROJECT_PATH/Assets/Editor/"
  # Copy the build script of Unity Builder action
  cp -R "/UnityBuilderAction/Assets/Editor/" "$UNITY_PROJECT_PATH/Assets/Editor/"
  # Set the Build method to that of UnityBuilder Action
  BUILD_METHOD="UnityBuilderAction.Builder.BuildProject"
  # Verify recursive paths
  ls -Ralph "$UNITY_PROJECT_PATH/Assets/Editor/"
  #
else
  # User has provided their own build method.
  # Assume they also bring their own script.
  #
  echo "Using build method \"$BUILD_METHOD\"."
  #
fi

#
# Create Android keystore, if needed
#

if [[ -z $ANDROID_KEYSTORE_NAME || -z $ANDROID_KEYSTORE_BASE64 ]]; then
  echo "Not creating Android keystore."
else
  echo "$ANDROID_KEYSTORE_BASE64" | base64 --decode > "$UNITY_PROJECT_PATH/$ANDROID_KEYSTORE_NAME"
  echo "Created Android keystore."
fi

#
# Pre-build debug information
#

echo ""
echo "###########################"
echo "#    Custom parameters    #"
echo "###########################"
echo ""

echo "$CUSTOM_PARAMETERS"

echo ""
echo "###########################"
echo "#    Current build dir    #"
echo "###########################"
echo ""

echo "Creating \"$BUILD_PATH_FULL\" if it does not exist."
mkdir -p "$BUILD_PATH_FULL"
ls -alh "$BUILD_PATH_FULL"

echo ""
echo "###########################"
echo "#    Project directory    #"
echo "###########################"
echo ""

ls -alh "$UNITY_PROJECT_PATH"

echo "###########################"
echo "#        Installers       #"
echo "###########################"
echo $ls
echo $PWD
$s3InstallerPath "/github/workspace/dist/installers/S3Installer.asset"
$serverInstallerPath "${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/ServerInstaller.asset"

#echo "VARIABLES:"
#echo "1"
#echo "$s3InstallerAccessKey"
#echo "2"
#echo "$s3InstallerSecretKey"
#echo "3"
#echo "$s3InstallerRegion"
#echo "4"
#echo "$s3InstallerBucketName"
#echo "5"
#echo "$serverDevelopmentAddress"
#echo "6"
#echo "$serverProductionAddress"
#echo "7"
#echo "$serverTimeout"
#echo "8"
#echo "$BUILD_PATH_FULL"
#echo "9"
#echo "$UNITY_PROJECT_PATH"
#echo "###"
#echo ""

echo "1"
sed -i 's|ServerDevelopmentAddress|'"$serverDevelopmentAddress"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/ServerInstaller.asset
echo "2"
sed -i 's|ServerProductionAddress|'"$serverProductionAddress"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/ServerInstaller.asset
echo "3"
sed -i 's|1234|'"$serverTimeout"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/ServerInstaller.asset

#S3
echo "4"
sed -i 's|S3SecretKey|'"$s3InstallerSecretKey"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/S3Installer.asset
echo "5"
sed -i 's|S3AccessKey|'"$s3InstallerAccessKey"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/S3Installer.asset
echo "6"
sed -i 's|S3InstallerRegion|'"$s3InstallerRegion"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/S3Installer.asset

echo "${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/ServerInstaller.asset)"
echo "${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/S3Installer.asset)"

sed -i 's|S3InstallerRegion|'"$s3InstallerRegion"'|g' ${UNITY_PROJECT_PATH}/Assets/GameAssets/Resources/Installers/S3Installer.asset

echo "###########################"
echo "#      SIDGIN CONFIG      #"
echo "###########################"


echo "IF OSX"
if [[ ${BUILD_TARGET} != *"OSX"* ]];then
  echo "not building for OSX"
  echo 1
  sed -i 's|selectedDefinition: OSX|'"selectedDefinition: Win64"'|g' ${UNITY_PROJECT_PATH}/Assets/SIDGIN/EditorResources/StandaloneBuildSettingsData.asset
else
  echo "building for OSX"
  sed -i 's|selectedDefinition: Win64|'"selectedDefinition: OSX"'|g' ${UNITY_PROJECT_PATH}/Assets/SIDGIN/EditorResources/StandaloneBuildSettingsData.asset
fi
echo "$(${UNITY_PROJECT_PATH}/Assets/SIDGIN/EditorResources/StandaloneBuildSettingsData.asset)"
#this is required as we get a error about the build function not existing even when it's correct
#something to do with the library folder?
echo ""
echo "###########################"
echo "#     Pregen project      #"
echo "###########################"
echo ""
unity-editor \
  -nographics \
  -logfile /dev/stdout \
  -quit \
  -customBuildName "$BUILD_NAME" \
  -projectPath "$UNITY_PROJECT_PATH" \
  -buildVersion "$VERSION" \
#
# Build
#
echo ""
echo "###########################"
echo "#    Building project     #"
echo "###########################"
echo ""

# Reference: https://docs.unity3d.com/2019.3/Documentation/Manual/CommandLineArguments.html

unity-editor \
  -nographics \
  -logfile /dev/stdout \
  -quit \
  -customBuildName "$BUILD_NAME" \
  -projectPath "$UNITY_PROJECT_PATH" \
  -buildTarget "$BUILD_TARGET" \
  -customBuildTarget "$BUILD_TARGET" \
  -customBuildPath "$CUSTOM_BUILD_PATH" \
  -executeMethod "$BUILD_METHOD" \
  -buildVersion "$VERSION" \
  -androidVersionCode "$ANDROID_VERSION_CODE" \
  -androidKeystoreName "$ANDROID_KEYSTORE_NAME" \
  -androidKeystorePass "$ANDROID_KEYSTORE_PASS" \
  -androidKeyaliasName "$ANDROID_KEYALIAS_NAME" \
  -androidKeyaliasPass "$ANDROID_KEYALIAS_PASS" \
  $CUSTOM_PARAMETERS

# Catch exit code
BUILD_EXIT_CODE=$?

# Display results
if [ $BUILD_EXIT_CODE -eq 0 ]; then
  echo "Build succeeded";
else
  echo "Build failed, with exit code $BUILD_EXIT_CODE";
fi

#
# Permissions
#

# Make a given user owner of all artifacts
if [[ -n "$CHOWN_FILES_TO" ]]; then
  chown -R "$CHOWN_FILES_TO" "$BUILD_PATH_FULL"
  chown -R "$CHOWN_FILES_TO" "$UNITY_PROJECT_PATH"
fi

# Add read permissions for everyone to all artifacts
chmod -R a+r "$BUILD_PATH_FULL"
chmod -R a+r "$UNITY_PROJECT_PATH"

# Add execute permissions to specific files
if [[ "$BUILD_TARGET" == "StandaloneOSX" ]]; then
  OSX_EXECUTABLE_PATH="$BUILD_PATH_FULL/StandaloneOSX.app/Contents/MacOS/*"
  chmod +x "$OSX_EXECUTABLE_PATH"
fi
#
# Results
#

echo ""
echo "###########################"
echo "#       Build output      #"
echo "###########################"
echo ""

ls -alh "$BUILD_PATH_FULL"
