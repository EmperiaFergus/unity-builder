ARG IMAGE
FROM $IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"

ARG RUBY_PATH=/usr/local/
ARG RUBY_VERSION=2.6.0

FROM drecom/ubuntu-base:16.04 AS rubybuild
ARG RUBY_PATH
ARG RUBY_VERSION
RUN git clone git://github.com/rbenv/ruby-build.git $RUBY_PATH/plugins/ruby-build \
&&  $RUBY_PATH/plugins/ruby-build/install.sh
RUN ruby-build $RUBY_VERSION $RUBY_PATH

FROM ubuntu:16.04
LABEL maintainer "Drecom Technical Development Department <pr_itn@drecom.co.jp>"
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
        libmysqlclient-dev \
        redis-server \
        libsqlite3-dev
COPY --from=rubybuild $RUBY_PATH $RUBY_PATH
CMD [ "irb" ]


ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD patchkit /patchkit
RUN chmod -R +x /patchkit
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
RUN ls

ENTRYPOINT ["/entrypoint.sh"]
