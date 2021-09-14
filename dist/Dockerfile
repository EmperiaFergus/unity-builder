ARG IMAGE
FROM $IMAGE

ARG RUBY_PATH=/usr/local/
ARG RUBY_VERSION=2.6.0
ARG GAME_CI_UNITY_EDITOR_IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


# Install Ruby.
RUN apt-get update \
&&  apt-get upgrade -y --force-yes \
&&  apt-get install -y --force-yes \
    libssl-dev \
    libreadline-dev \
    zlib1g-dev \
    wget \
    curl \
    git \
    build-essential \
&&  apt-get clean \
&&  rm -rf /var/cache/apt/archives/* /var/lib/apt/lists/*

ARG RUBY_PATH
ARG RUBY_VERSION
RUN git clone git://github.com/rbenv/ruby-build.git $RUBY_PATH/plugins/ruby-build \
&&  $RUBY_PATH/plugins/ruby-build/install.sh
RUN ruby-build $RUBY_VERSION $RUBY_PATH

# inject ruby and additional dependencies in game-ci's unity editor image
FROM $GAME_CI_UNITY_EDITOR_IMAGE

ARG RUBY_PATH
ENV PATH $RUBY_PATH/bin:$PATH
RUN apt-get update && \
    apt-get install -y \
        git \
        curl \
        gcc \
        make \
        libssl-dev \
        zlib1g-dev \
        libsqlite3-dev
COPY --from=rubybuild $RUBY_PATH $RUBY_PATH
#installed ruby


ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD patchkit /patchkit
RUN chmod -R +x /patchkit
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
#ADD steps/patchkit/patchkit-tools.sh /steps/patchkit/patchkit-tools.sh
#RUN chmod -R +x /steps/patchkit/patchkit-tools.sh
RUN ls

ENTRYPOINT ["/entrypoint.sh"]
