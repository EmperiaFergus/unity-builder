ARG IMAGE
FROM $IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


# Install Local ruby
RUN git clone https://github.com/rbenv/rbenv.git ~/.rbenv \
  &&  echo 'export PATH="$HOME/.rbenv/bin:$PATH"' >> ~/.bashrc \
  &&  echo 'eval "$(rbenv init -)"' >> ~/.bashrc

ENV HOME /home/jenkins # Change this dir as needed.
ENV PATH "$HOME/.rbenv/bin:$HOME/.rbenv/shims:$PATH"
ENV RUBY_VERSION 2.6.3

RUN mkdir -p "$(rbenv root)"/plugins \
    && git clone https://github.com/rbenv/ruby-build.git "$(rbenv root)"/plugins/ruby-build

RUN rbenv install $RUBY_VERSION

RUN rbenv global $RUBY_VERSION && rbenv versions && ruby -v


ADD default-build-script /UnityBuilderAction
ADD steps /steps
RUN chmod -R +x /steps
ADD patchkit /patchkit
RUN chmod -R +x /patchkit
ADD entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
RUN ls

ENTRYPOINT ["/entrypoint.sh"]
