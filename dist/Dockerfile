ARG IMAGE
FROM $IMAGE

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
ADD patchkit /steps/patchkit/patchkit-tools.sh
RUN sudo chmod -R +x /steps/patchkit/patchkit-tools.sh
#ADD patchkit /patchkit
#RUN chmod -R +x /patchkit/patchkit-tools
RUN ls

ENTRYPOINT ["/entrypoint.sh"]
