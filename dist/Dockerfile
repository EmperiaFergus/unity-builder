ARG IMAGE
FROM $IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


RUN git clone https://github.com/rbenv/ruby-build.git && \
  PREFIX=/usr/local ./ruby-build/install.sh && \
  ruby-build -v 2.4.1 /usr/local && \
  gem install bundler -v <VERSION HERE> --no-ri --no-rdoc && bundle install

ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD patchkit /patchkit
RUN chmod -R +x /patchkit
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
RUN ls

ENTRYPOINT ["/entrypoint.sh"]
