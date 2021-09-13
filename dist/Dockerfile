ARG IMAGE
FROM $IMAGE

LABEL "com.github.actions.name"="Unity - Builder"
LABEL "com.github.actions.description"="Build Unity projects for different platforms."
LABEL "com.github.actions.icon"="box"
LABEL "com.github.actions.color"="gray-dark"

LABEL "repository"="http://github.com/webbertakken/unity-actions"
LABEL "homepage"="http://github.com/webbertakken/unity-actions"
LABEL "maintainer"="Webber Takken <webber@takken.io>"


# Install Ruby.
#RUN \
#  apt-get update && \
#  apt-get install -y ruby
# Download the ruby-build code



# Install ruby-build
RUN cd /tmp/ruby-build-* && ./install.sh && cd / && rm -rfv /tmp/ruby-build-master

# Install ruby
RUN ruby-build -v 1.9.3-p429 /usr/local

# Install gems
RUN gem install bundler rubygems-bundler --no-rdoc --no-ri

# Regenerate binstubs
RUN gem regenerate_binstubs


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
