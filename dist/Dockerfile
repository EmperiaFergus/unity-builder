ARG IMAGE
FROM $IMAGE
RUN ln -s /opt/unity/Editor/Data/PlaybackEngines/MacStandaloneSupport/Variations/macos_x64arm64_player_nondevelopment_mono /opt/unity/Editor/Data/PlaybackEngines/MacStandaloneSupport/Variations/macos_x64ARM64_player_nondevelopment_mono

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
RUN apt-get -y update
RUN apt-get -y install ed

ENTRYPOINT ["/entrypoint.sh"]
