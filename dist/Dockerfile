ARG IMAGE
FROM $IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


## Install Ruby.
#RUN \
#  apt-get update && \
#  apt-get install -y ruby
## Download the ruby-build code

ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD patchkit /patchkit
RUN chmod -R +x /patchkit
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
#ADD steps/patchkit/patchkit-tools.sh /steps/patchkit/patchkit-tools.sh
#RUN chmod -R +x /steps/patchkit/patchkit-tools.sh

ENTRYPOINT ["/entrypoint.sh"]
